using Firebase.Firestore;
using System;

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
}
