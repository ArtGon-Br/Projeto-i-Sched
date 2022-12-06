using Firebase.Firestore;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.Assertions;
using System.Collections;
using System;
using System.Linq;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

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

        if(taskToAlocate.isFix)
        {
            DateTime startDateTime = new DateTime(int.Parse(taskToAlocate.Year), int.Parse(taskToAlocate.Month), int.Parse(taskToAlocate.Day),
                                        taskToAlocate.Hour, taskToAlocate.Min, 0);

            DateTime endDateTime = new DateTime(int.Parse(taskToAlocate.YearTo), int.Parse(taskToAlocate.MonthTo), int.Parse(taskToAlocate.DayTo),
                                        taskToAlocate.Hour, taskToAlocate.Min, 0);

            DateTime currentTime = startDateTime;

            while (currentTime <= endDateTime)
            {
                if(RepeatInDayOfWeek(taskToAlocate.RepeatDays, currentTime.DayOfWeek))
                {
                    yield return StartCoroutine(GetConflictedTaskInDay(taskToAlocate, currentTime, (x) => count = x));
                }

                currentTime.AddDays(1);
            }
        }
        else
        {
            DateTime targetStartTime = new DateTime(int.Parse(taskToAlocate.Year), int.Parse(taskToAlocate.Month), int.Parse(taskToAlocate.Day),
                                            taskToAlocate.Hour, taskToAlocate.Min, 0);

            yield return StartCoroutine(GetConflictedTaskInDay(taskToAlocate, targetStartTime, (x) => count = x));
        }

        OnGetConflictedTasks?.Invoke(count);
    }

    private IEnumerator GetConflictedTaskInDay(TaskData taskToAlocate, DateTime targetStartTime, Action<int> OnGetConflictedTasks)
    {
        var Firestore = FirebaseFirestore.DefaultInstance;
        int count = 0;

        var query = GetTaskCollection(Firestore).WhereEqualTo("Day", targetStartTime.Day);
        bool finishThread = false;

        query.GetSnapshotAsync().ContinueWithOnMainThread(querySnapshot =>
        {
            Assert.IsNotNull(querySnapshot);

            DateTime targetEndTime = targetStartTime.AddHours(taskToAlocate.HourDuration).AddMinutes(taskToAlocate.MinutesDuration);

            var resultsList = querySnapshot.Result.ToList();
            foreach (DocumentSnapshot result in resultsList)
            {
                TaskData task = result.ConvertTo<TaskData>();

                DateTime start = new DateTime(int.Parse(task.Year), int.Parse(task.Month), int.Parse(task.Day), task.Hour, task.Min, 0);
                DateTime end = start.AddHours(task.HourDuration).AddMinutes(task.MinutesDuration);

                if (start >= targetEndTime) continue;
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

    private bool RepeatInDayOfWeek(string repeatDays, DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Monday:
                return repeatDays[0] == '1';
            case DayOfWeek.Tuesday:
                return repeatDays[1] == '1';
            case DayOfWeek.Wednesday:
                return repeatDays[2] == '1';
            case DayOfWeek.Thursday:
                return repeatDays[3] == '1';
            case DayOfWeek.Friday:
                return repeatDays[4] == '1';
            case DayOfWeek.Saturday:
                return repeatDays[5] == '1';
            case DayOfWeek.Sunday:
                return repeatDays[6] == '1';
        }

        return false;
    }

    private void OnDestroy()
    {
        if(_listenerRegistration!= null) _listenerRegistration.Stop();
    }

}
