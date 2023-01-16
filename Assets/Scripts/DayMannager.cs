using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

public class DayMannager : MonoBehaviour
{
    private List<TaskData> tasks;
    private bool searchingEnd;
    [SerializeField] int year;
    [SerializeField] int day;
    [SerializeField] int month;
    [SerializeField] int tasksCount;
    private DateTime date;
    Dictionary<string, int> months = new Dictionary<string, int> { { "jan", 1 },    { "fev", 2 },   { "mar", 3 },
                                                                   { "abr", 4 },    { "mai", 5 },   { "jun", 6 },
                                                                   { "jul", 7 },    { "ago", 8 },   { "set", 9 },
                                                                   { "out", 10 },   { "nov", 11 },  { "dez", 12 } };

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (tasks.Count == 0) return;
            DateTime date = new DateTime(year, month, day);
            FindObjectOfType<DailyTasksVisualization>().Visualize(tasks, date);
        });
    }

    // Seta o componente Image caso o dia tenha alguma task, ou seja, deixa o dia no calendario com um tipo de highlight
    void SetUI()
    {
        // Filho do botao do dia possui uma imagem se mostra se existe tasks ou nao
        Color color;
        ColorUtility.TryParseHtmlString("#00FA27", out color);
        color.a = tasksCount > 0 ? 1f: 0;
        transform.GetChild(1).GetComponent<Image>().color = color;

        color = Color.white;
        GetComponent<Image>().color = color;
        transform.GetChild(0).GetComponent<Text>().color = color;
    }

    public void setDate(string Day, string Mes, string Ano)
    {
        int.TryParse(Ano, out year);
        month = months[Mes];
        int.TryParse(Day, out day);

        StartCoroutine(UpdateDay());
    }
    
    private IEnumerator UpdateDay()
    {
        date = new DateTime(year, month, day);
        searchingEnd = false;
        SearchForExistentTasksThroughDate(date);

        yield return new WaitUntil(() => searchingEnd);

        CheckTasksInSpeficDate();

        SetUI();
    }

    private void CheckTasksInSpeficDate()
    {
        if (tasks == null) tasks = new List<TaskData>();

        var correctTasks = tasks.Where(t => t.StartTime.Day == day)
                              .Where(t => t.StartTime.Month == month)
                              .Where(t => t.StartTime.Year == year).ToList();

        tasks = correctTasks;
    }
    void SearchForExistentTasksThroughDate(DateTime date)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var Db = FirebaseFirestore.DefaultInstance;

        tasks = new List<TaskData>();

        CollectionReference trfRef = Db.Collection(path: "users_sheet").Document(path: auth.CurrentUser.UserId.ToString()).Collection(path: "Tasks");

        Firebase.Firestore.Query query = trfRef;

        query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            if (querySnapshotTask.IsCanceled) return;

            List<DocumentSnapshot> tasksFound = new List<DocumentSnapshot>();

            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                Dictionary<string, object> details = documentSnapshot.ToDictionary();

                Timestamp timeStamp = (Timestamp)details["StartTime"];
                DateTime dateTime = timeStamp.ToDateTime();

                if (date.ToShortDateString() == dateTime.ToShortDateString())
                {
                    Debug.Log(string.Format("Document {0} returned by query", documentSnapshot.Id));
                    tasksCount++;
                    InstantiateTasks(documentSnapshot);
                }
            }

            if (tasksFound.Count > 0)
            {
                print($"{date} > {tasksFound.Count}");
            }
            searchingEnd = true;
        });
    }
    private void InstantiateTasks(DocumentSnapshot documentSnapshot)
    {
        Dictionary<string, object> details = documentSnapshot.ToDictionary();
        TaskData task = new TaskData();
        task.Name = details["Name"].ToString();
        task.Description = details["Description"].ToString();

        Timestamp timeStamp = (Timestamp)details["StartTime"];
        DateTime dateTime = timeStamp.ToDateTime();

        task.StartTime = dateTime;

        timeStamp = (Timestamp)details["EndTime"];
        dateTime = timeStamp.ToDateTime();

        task.EndTime = dateTime;

        tasks.Add(task);
    }
}
