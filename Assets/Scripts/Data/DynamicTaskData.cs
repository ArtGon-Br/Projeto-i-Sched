using Firebase.Firestore;

[FirestoreData]
public struct DynamicTaskData
{
    [FirestoreProperty]
    public string Title { get; set; }

    [FirestoreProperty]
    public string Description { get; set; }

    [FirestoreProperty]
    public string Difficult { get; set; }

    [FirestoreProperty]
    public bool Done { get; set; }

    [FirestoreProperty]
    public object DueDate { get; set; }

    [FirestoreProperty]
    public object Deadline { get; set; }
    
    [FirestoreProperty]
    public float Duration { get; set; }
}
