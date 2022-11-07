using Firebase.Firestore;


[FirestoreData]
public struct TaskController{
    [FirestoreProperty]
    public string Name{get;set;}
    [FirestoreProperty]
    public string Description{get;set;}
    [FirestoreProperty]
    public bool isFix{get;set;}
    [FirestoreProperty]
    public int Day{get;set;}
    [FirestoreProperty]
    public int Month{get;set;}
    [FirestoreProperty]
    public int Year{get;set;}
    [FirestoreProperty]
    public int Hour{get;set;}
    [FirestoreProperty]
    public int Min{get;set;}
    [FirestoreProperty]
    public int Priority{get;set;}
    [FirestoreProperty]
    public int DurationH{get;set;}
    [FirestoreProperty]
    public int DurationMin{get;set;}

}
