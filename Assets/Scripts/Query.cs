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
    [SerializeField] QueryViewUI queryViewUIPrefab;
    static FirebaseFirestore db;
    Transform[] clones;

    public static bool searchingEnd;
    private static List<TaskData> tasksFounded = new List<TaskData>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initializing query module...");
        db = FirebaseFirestore.DefaultInstance;

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
        Firebase.Firestore.Query query = trfRef.WhereEqualTo("Name", char.ToUpper(inputText[0]) + inputText.Substring(1));
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

        QueryViewUI queryView = Instantiate(queryViewUIPrefab, viewport);
        Timestamp timeStamp = (Timestamp)details["StartTime"];
        DateTime dateTime = timeStamp.ToDateTime();
        queryView.UpdateText(dateTime, details["Name"].ToString());
    }
}
