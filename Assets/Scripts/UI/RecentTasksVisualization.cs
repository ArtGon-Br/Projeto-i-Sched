using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using UnityEngine;
using UnityEditor.PackageManager.UI;
using Michsky.UI.ModernUIPack;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEditor.VersionControl;

public class RecentTasksVisualization : MonoBehaviour
{
    private int days = 1;
    private bool searchingEnd;
    private DateTime Now;
    List<TaskData> recentTasks;

    [SerializeField] int MaxTasks;
    [SerializeField] GameObject waiting;
    [SerializeField] Transform parentTransformForInstantation;
    [SerializeField] TaskUI prefabTask;

    [SerializeField] WindowManager window;


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Initialize());
    }

    public void Refresh()
    {
        if (window.currentWindowIndex != 0) return;
        StartCoroutine(Initialize());
    }

    IEnumerator Initialize()
    {
        parentTransformForInstantation.DeleteChildren();
        waiting.SetActive(true);

        days = 1;
        Now = DateTime.Now;
        recentTasks = new List<TaskData>();
        searchingEnd= false;

        SearchForExistentTasksThroughDate(Now.Date);

        yield return new WaitUntil(() => searchingEnd);

        if (recentTasks.Count >= MaxTasks) yield return null;
        else StartCoroutine(SearchForMoreDays(Now.AddDays(1)));
    }

    IEnumerator SearchForMoreDays(DateTime day)
    {
        SearchForExistentTasksThroughDate(day);

        yield return new WaitUntil(() => searchingEnd);

        day = day.AddDays(1);
        if (days > 7 || recentTasks.Count >= MaxTasks) yield return null;
        else StartCoroutine(SearchForMoreDays(day));
    }

    void SearchForExistentTasksThroughDate(DateTime date)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var Db = FirebaseFirestore.DefaultInstance;

        CollectionReference trfRef = Db.Collection(path: "users_sheet").Document(path: auth.CurrentUser.UserId.ToString()).Collection(path: "Tasks");

        Firebase.Firestore.Query query = trfRef;

        query.GetSnapshotAsync().ContinueWithOnMainThread((querySnapshotTask) =>
        {
            if (querySnapshotTask.IsCanceled) return;

            foreach (DocumentSnapshot documentSnapshot in querySnapshotTask.Result.Documents)
            {
                Dictionary<string, object> details = documentSnapshot.ToDictionary();

                Timestamp timeStamp = (Timestamp)details["StartTime"];
                DateTime dateTime = timeStamp.ToDateTime();

                if (date.ToShortDateString() == dateTime.ToShortDateString())
                {
                    InstantiateTasks(documentSnapshot);
                    //recentTasks.Add(task);
                }
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

        var obj = Instantiate(prefabTask, parentTransformForInstantation);
        obj.SetTask(task, true);

        waiting.SetActive(false);
    }
}
