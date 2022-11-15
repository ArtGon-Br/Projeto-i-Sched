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
    public string Day{get;set;}
    [FirestoreProperty]
    public string Month{get;set;}
    [FirestoreProperty]
    public string Year{get;set;}
    [FirestoreProperty]
    public int Hour{get;set;}
    [FirestoreProperty]
    public int Min{get;set;}
    [FirestoreProperty]
    public int Priority{get;set;}
    [FirestoreProperty]
    public string DurationH{get;set;}
    [FirestoreProperty]
    public string DurationMin{get;set;}

}