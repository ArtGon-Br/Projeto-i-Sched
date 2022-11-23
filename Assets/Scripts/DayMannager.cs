using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using System.Text;

public class DayMannager : MonoBehaviour
{
    private List<TaskData> tasks;
    [SerializeField] int year;
    [SerializeField] int day;
    [SerializeField] int month;
    private bool ready;
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
    void SetUI(bool visuable)
    {
        // Filho do botao do dia possui uma imagem se mostra se existe tasks ou nao
        Color color;
        ColorUtility.TryParseHtmlString("#00FA27", out color);
        color.a = visuable == true ? 0.25f: 0;
        transform.GetChild(1).GetComponent<Image>().color = color;
    }

    public void isReady() => ready = true;
    public void setTasks(List<TaskData> tasks) => this.tasks = tasks;
    public void setDate(string Day, string Mes, string Ano)
    {
        int.TryParse(Ano, out year);
        month = months[Mes];
        int.TryParse(Day, out day);

        StartCoroutine(UpdateDay());
    }
    public void AddTask(string name, string hour, string minutes, string description)
    {
        TaskData newTask = new TaskData();
        newTask.Name = name;
        newTask.Hour = int.Parse(hour);
        newTask.Min = int.Parse(minutes);
        newTask.Description = description;

        if (tasks == null) tasks = new List<TaskData>();
        tasks.Add(newTask);
    }
    private IEnumerator UpdateDay()
    {
        tasks = new List<TaskData>();

        StringBuilder builder = new StringBuilder();
        builder.Append(day.ToString());
        builder.Append("/");
        builder.Append(month.ToString());
        builder.Append("/");
        builder.Append(year.ToString());

        ready = false;
        Query.SearchForExistentTasks(builder.ToString(), "Data", this);

        yield return new WaitUntil(() => ready);

        SetUI(tasks.Count > 0);
    }
}
