using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;

public class Query : MonoBehaviour
{
    [SerializeField] Text input;
    [SerializeField] GameObject viewport;
    [SerializeField] GameObject task;
    FirebaseFirestore db;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Initializing querry module...");
        db = FirebaseFirestore.DefaultInstance;
    }

    // get fb data
    public void GetData()
        {
            Debug.Log(string.Format("Querying by {0}...", input.text.ToString()));
            CollectionReference trfRef = db.Collection("tarefas");
            Firebase.Firestore.Query query = trfRef.WhereEqualTo("Texto", input.text.ToString());
            query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
            {
                foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
                {
                    Debug.Log(string.Format("Document {0} returned by query Texto={1}", documentSnapshot.Id, input.text.ToString()));
                }
            });
        }
}
