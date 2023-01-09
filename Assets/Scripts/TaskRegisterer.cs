using Firebase.Firestore;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.Assertions;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class TaskRegisterer : MonoBehaviour
{
    FirebaseAuth _auth;
    FirebaseFirestore _db;
    ListenerRegistration _listenerRegistration;

    int _tasks = 0;
    bool _initialized = false;

    private void Start()
    {
        InitializeFirebase();
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
        //InitializeFirebase();

        var _userData = new UserData
        {
            Tasks = _tasks + 1
        };

        newTask.Index = _tasks;

        var Firestore = FirebaseFirestore.DefaultInstance;
        Firestore.Collection(path: "users_sheet").Document(path: GetUserID()).SetAsync(_userData);
        Firestore.Collection(path: "users_sheet").Document(path: GetUserID()).Collection(path: "Tasks").Document(path: newTask.Index.ToString()).SetAsync(newTask);
    }

    public void UpdateTask(TaskData taskToUpdate)
    {
        //InitializeFirebase();

        var Firestore = FirebaseFirestore.DefaultInstance;
        Firestore.Collection(path: "users_sheet").Document(path: GetUserID()).Collection(path: "Tasks").Document(path: taskToUpdate.Index.ToString()).SetAsync(taskToUpdate);
    }

    public IEnumerator GetConflictedTaks(TaskData taskToAlocate, Action<List<TaskData>> OnGetConflictedTasks)
    {
        //InitializeFirebase();

        List<TaskData> conflictedTasks = new List<TaskData>();

        yield return StartCoroutine(GetConflictedTaskInDay(taskToAlocate, (x) => conflictedTasks = x));

        OnGetConflictedTasks?.Invoke(conflictedTasks);
    }

    private IEnumerator GetConflictedTaskInDay(TaskData taskToAlocate, Action<List<TaskData>> OnGetConflictedTasks)
    {
        var Firestore = FirebaseFirestore.DefaultInstance;
        List<TaskData> conflictedTasks = new List<TaskData>();

        var query = GetTaskCollection(Firestore).WhereNotEqualTo("Index", taskToAlocate.Index);
        bool finishThread = false;

        query.GetSnapshotAsync().ContinueWithOnMainThread(querySnapshot =>
        {
            Assert.IsNotNull(querySnapshot);

            var resultsList = querySnapshot.Result.ToList();
            foreach (DocumentSnapshot result in resultsList)
            {
                TaskData task = result.ConvertTo<TaskData>();

                if (task.StartTime >= taskToAlocate.EndTime) continue;
                if (task.EndTime <= taskToAlocate.StartTime) continue;

                conflictedTasks.Add(task);
            }

            finishThread = true;
        });

        yield return new WaitUntil(() => finishThread == true);

        OnGetConflictedTasks?.Invoke(conflictedTasks);
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
        if(_listenerRegistration!= null) _listenerRegistration.Stop();
    }

}
