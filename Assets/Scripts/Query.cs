using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;

public class Query : MonoBehaviour
{
    [SerializeField] TMP_InputField input;
    [SerializeField] Transform viewport;
    [SerializeField] Transform task;
    FirebaseFirestore db;
    Transform[] clones;

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
        trfRef.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                InstantiateTasks(documentSnapshot); 
            }
        });
    } 

    // get fb data
    public void GetData()
    {
        string inputText = input.text.ToString();

        deleteClones();

        Debug.Log(string.Format("Querying by {0}...", char.ToUpper(inputText[0]) + inputText.Substring(1)));
        CollectionReference trfRef = db.Collection("tarefas");
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

        taskTransform.Find("Data").GetComponent<TMP_Text>().text = details["Data"].ToString();
        taskTransform.Find("Hora").GetComponent<TMP_Text>().text = details["Hora"].ToString();
        taskTransform.Find("Texto").GetComponent<TMP_Text>().text = details["Texto"].ToString();
        taskTransform.gameObject.SetActive(true);
    }

    #region Static
    public static IEnumerator SearchForExistentTasks(string input, string type, DayMannager manager)
    {
        bool ready = false;
        var Db = FirebaseFirestore.DefaultInstance;
        List<TaskData> tasks = new List<TaskData>();

        Debug.Log(string.Format("Querying by {0}...", char.ToUpper(input[0]) + input.Substring(1)));
        CollectionReference trfRef = Db.Collection("Tasks");

        if(type == "Data")
        {
            var inputs = input.Split('/');
            Firebase.Firestore.Query query = trfRef.WhereEqualTo("Day", int.Parse(inputs[0]))
                                                   .WhereEqualTo("Month", int.Parse(inputs[1]))
                                                   .WhereEqualTo("Year", inputs[2]);

            query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
            {
                foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
                {
                    Dictionary<string, object> details = documentSnapshot.ToDictionary();
                    var task = new TaskData();

                    task.Hour = (int)details["Hour"];
                    task.Name = details["Name"].ToString(); ;
                    task.Description = details["Description"].ToString();
                    Debug.Log(task.Name);
                    tasks.Add(task);
                }

                ready = true;
            });
        }

        

        yield return new WaitUntil(() => ready);
        manager.tasks = tasks;
        manager.isReady();
    }

    #endregion
}
