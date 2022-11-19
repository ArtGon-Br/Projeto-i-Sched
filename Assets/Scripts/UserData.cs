using Firebase.Firestore;

/// <summary>
/// Struct basica para testar funcionalidade com o banco de dados
/// </summary>
[FirestoreData]
public struct UserData
{
    [FirestoreProperty]
    public int Tasks {get; set;}
}
