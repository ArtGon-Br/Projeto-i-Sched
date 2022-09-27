using Firebase.Firestore;

[FirestoreData]
public struct UserData
{
    [FirestoreProperty]
    public string Username { get; set; }

    [FirestoreProperty]
    public string Password { get; set; }
}
