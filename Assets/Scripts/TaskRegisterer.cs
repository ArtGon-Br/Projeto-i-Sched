using Firebase.Firestore;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.Assertions;
using System.Collections;
using System;
using System.Linq;

public class TaskRegisterer : MonoBehaviour
{
    FirebaseAuth _auth;
    FirebaseFirestore _db;
    ListenerRegistration _listenerRegistration;

    int _tasks = 0;
    bool _initialized = false;

    private void Start()
    {
        //InitializeFirebase();
    }

    private void InitializeFirebase()
    {
        if (_initialized) return;

        _tasks = 0;
        _auth = FirebaseAuth.DefaultInstance;
        _db = FirebaseFirestore.DefaultInstance;
        _listenerRegistration = _db.Collection(path: "users_sheet").Document(path: _auth.CurrentUser.UserId.ToString()).Listen(snapshot =>
        {
            UserData data = snapshot.ConvertTo<UserData>();
            _tasks = data.Tasks;
        });

        _initialized = true;
    }

    public void RegisterTask(TaskData newTask)
    {
        InitializeFirebase();

        var _userData = new UserData
        {
            Tasks = _tasks + 1
        };

        var Firestore = FirebaseFirestore.DefaultInstance;
        Firestore.Collection(path: "users_sheet").Document(path: GetUserID()).SetAsync(_userData);
        Firestore.Collection(path: "users_sheet").Document(path: GetUserID()).Collection(path: "Tasks").Document(path: _tasks.ToString()).SetAsync(newTask);
    }

    public IEnumerator GetConflictedTaks(TaskData taskToAlocate, Action<int> OnGetConflictedTasks)
    {
        InitializeFirebase();

        int count = 0;

        var Firestore = FirebaseFirestore.DefaultInstance;
        var query = GetTaskCollection(Firestore).WhereEqualTo("Day", taskToAlocate.Day);
        bool finishThread = false;

        query.GetSnapshotAsync().ContinueWithOnMainThread(querySnapshot =>
        {
            Assert.IsNotNull(querySnapshot);
            
            DateTime targetStartTime = new DateTime(int.Parse(taskToAlocate.Year), int.Parse(taskToAlocate.Month), int.Parse(taskToAlocate.Day),
                                            taskToAlocate.Hour, taskToAlocate.Min, 0);

            DateTime endStartTime = targetStartTime.AddHours(taskToAlocate.HourDuration).AddMinutes(taskToAlocate.MinutesDuration);

            var resultsList = querySnapshot.Result.ToList();
            foreach(DocumentSnapshot result in resultsList)
            {
                TaskData task = result.ConvertTo<TaskData>();

                DateTime start = new DateTime(int.Parse(task.Year), int.Parse(task.Month), int.Parse(task.Day), task.Hour, task.Min, 0);
                DateTime end = start.AddHours(task.HourDuration).AddMinutes(task.MinutesDuration);

                if (start >= endStartTime) continue;
                if (end <= targetStartTime) continue;

                count++;
            }

            finishThread = true;
        });

        yield return new WaitUntil(() => finishThread == true);

        OnGetConflictedTasks?.Invoke(count);
    }

    private CollectionReference GetTaskCollection(FirebaseFirestore Firestore)
    {
        return Firestore.Collection(path: "users_sheet").Document(path: GetUserID()).Collection(path: "Tasks");
    }

    private string GetUserID()
    {
        return _auth.CurrentUser.UserId.ToString();
    }

    private void OnDestroy()
    {
        _listenerRegistration.Stop();
    }

}
