using Firebase.Firestore;
using System;
using Unity.Notifications.Android;
[FirestoreData]
public struct TaskData
{
    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string Description { get; set; }

    [FirestoreProperty]
    public int Priority { get; set; }

    [FirestoreProperty]
    public DateTime StartTime { get; set; }

    [FirestoreProperty]
    public DateTime EndTime { get; set; }

    [FirestoreProperty]
    public int Index { get; set; }
    
    [FirestoreProperty]
    public int MyNotificationID {get; set;} 
}
