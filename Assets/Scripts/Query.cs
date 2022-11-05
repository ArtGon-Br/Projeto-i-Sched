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
}
