using UnityEngine;
using Firebase.Firestore;

public class FirestoreManager : MonoBehaviour
{
    [SerializeField] string userPath = "users_sheet";

    public void CreateUser(string userName, string password)
    {
        // cria uma nova struct de user
        UserData newUser = new UserData
        {
            Username = userName,
            Password = password
        };
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(userPath).SetAsync(newUser);
    }
}
