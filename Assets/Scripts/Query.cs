using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Auth;
using TMPro;
using System;

public class Query : MonoBehaviour
{
    [SerializeField] TMP_InputField input;
    [SerializeField] Transform viewport;
    [SerializeField] Transform task;
    static FirebaseFirestore db;
    Transform[] clones;

    public static bool searchingEnd;
    private static List<TaskData> tasksFounded = new List<TaskData>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initializing querry module...");
        db = FirebaseFirestore.DefaultInstance;

        task.gameObject.SetActive(false);

        GetFirstData();
    }

    private void GetFirstData()
    {
        deleteClones();

        CollectionReference trfRef = db.Collection("tarefas");

        trfRef = db.Collection("users_sheet").Document(FirebaseAuth.DefaultInstance.CurrentUser.UserId).Collection("Tasks");

        Debug.Log("CAMINHO: " + trfRef);

        trfRef.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                InstantiateTasks(documentSnapshot);
            }
        });
    }
    
    //create trigram from string
    public string[] triGram(string str) {
        List<string> trigrams = new List<string>();
        trigrams.Add(str);
        for(int i = 0; i < str.Length - 2; i++) {
            trigrams.Add(str.Substring(i, 3));
        }
        return trigrams.ToArray();
    }

    // get fb data
    public void GetData()
    {
        string inputText = input.text.ToString();

        deleteClones();
        if(inputText == "") {
            GetFirstData();
            return;
        }

        Debug.Log(string.Format("Querying by {0}...", char.ToUpper(inputText[0]) + inputText.Substring(1)));
        CollectionReference trfRef = db.Collection("tarefas");
        // Array.ForEach(triGram(char.ToUpper(inputText[0]) + inputText.Substring(1)), Debug.Log);
        Firebase.Firestore.Query query = trfRef.WhereEqualTo("Texto", char.ToUpper(inputText[0]) + inputText.Substring(1));
        query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                InstantiateTasks(documentSnapshot);                
            }
        });
    }

    private void deleteClones()
    {
        clones = viewport.GetComponentsInChildren<Transform>();
        foreach (Transform clone in clones) {
            if (clone != viewport) {
                Destroy(clone.gameObject);
            }
        }
    }

    private void InstantiateTasks(DocumentSnapshot documentSnapshot)
    {
        Debug.Log(string.Format("Document {0} returned by query Texto={1}", documentSnapshot.Id, input.text.ToString()));
        Dictionary<string, object> details = documentSnapshot.ToDictionary();

        Transform taskTransform = Instantiate(task, viewport);

        taskTransform.Find("Data").GetComponent<TMP_Text>().text = details["StartTime"].ToString();
        taskTransform.Find("Hora").GetComponent<TMP_Text>().text = details["Name"].ToString();
        taskTransform.Find("Texto").GetComponent<TMP_Text>().text = details["Description"].ToString();
        taskTransform.gameObject.SetActive(true);
    }

    #region Static
    public static void SearchForExistentTasksThroughDate(DateTime date)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var Db = FirebaseFirestore.DefaultInstance;

        tasksFounded.Clear();
        tasksFounded = new List<TaskData>();
        searchingEnd = false;

        CollectionReference trfRef = Db.Collection(path: "users_sheet").Document(path: auth.CurrentUser.UserId.ToString()).Collection(path: "Tasks");

        Firebase.Firestore.Query query = trfRef;

        query.GetSnapshotAsync().ContinueWith((querySnapshotTask) =>
        {
            if (querySnapshotTask.IsCanceled) return;

            List<DocumentSnapshot> tasksFound;
            
            tasksFound = querySnapshotTask.Result.Documents.Where(t => DateTime.Parse(t.ToDictionary()["StartTime"].ToString()).Date == date).ToList();

            if (tasksFound.Count > 0) print($"{date} > {tasksFound.Count}");
            foreach (var t in tasksFound)
            {
                Dictionary<string, object> details = t.ToDictionary();
                TaskData task = SetNewTask(details);
                tasksFounded.Add(task);
            }
            searchingEnd = true;
        });
    }

    public static List<TaskData> GetTasksFounded() 
    { 
        List<TaskData> tasks = new List<TaskData>(tasksFounded);
        return tasks; 
    }

    private static TaskData SetNewTask(Dictionary<string, object> details)
    {
        TaskData task = new TaskData();
        task.Name = details["Name"].ToString();
        task.Description = details["Description"].ToString();
        task.StartTime = DateTime.Parse(details["StartTime"].ToString());
        task.EndTime = DateTime.Parse(details["EndTime"].ToString());
        return task;
    }
    #endregion
}
