using Firebase.Firestore;


[FirestoreData]
public struct TaskData
{
    [FirestoreProperty]
    public string Name { get; set; }

    [FirestoreProperty]
    public string Description { get; set; }

    [FirestoreProperty]
    public bool isFix { get; set; }

    [FirestoreProperty]
    public string Day { get; set; }

    [FirestoreProperty]
    public string Month { get; set; }

    [FirestoreProperty]
    public string Year { get; set; }

    [FirestoreProperty]
    public int Hour { get; set; }

    [FirestoreProperty]
    public int Min { get; set; }

    [FirestoreProperty]
    public int Priority { get; set; }

    [FirestoreProperty]
    public int HourDuration { get; set; }

    [FirestoreProperty]
    public int MinutesDuration { get; set; }

    [FirestoreProperty]
    public int DayTo { get; set; }
    [FirestoreProperty]
    public string MonthTo { get; set; }
    [FirestoreProperty]
    public string YearTo { get; set; }

}
