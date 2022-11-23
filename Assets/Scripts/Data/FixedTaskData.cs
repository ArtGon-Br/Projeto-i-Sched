using Firebase.Firestore;


[FirestoreData]
public struct FixedTaskData
{
    [FirestoreProperty]
    public string Title { get; set; }

    [FirestoreProperty]
    public string Description { get; set; }

    [FirestoreProperty]
    public object Deadline { get; set; }
}
