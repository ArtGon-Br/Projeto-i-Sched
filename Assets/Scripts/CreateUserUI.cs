using UnityEngine;

public class CreateUserUI : MonoBehaviour
{
    [SerializeField] FirestoreManager firestoreManager;
    [SerializeField] string username;
    [SerializeField] string password;

    private void Start()
    {
        firestoreManager.CreateUser(username, password);
    }
}
