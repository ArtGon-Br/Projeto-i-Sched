using Firebase.Firestore;

/// <summary>
/// Struct basica para testar funcionalidade com o banco de dados
/// </summary>
[FirestoreData]
public struct UserData
{
    [FirestoreProperty]
    public string Username { get; set; }

    [FirestoreProperty]
    public string Password { get; set; }
    [FirestoreProperty]
    public int Tasks{ get; set;} 
}
