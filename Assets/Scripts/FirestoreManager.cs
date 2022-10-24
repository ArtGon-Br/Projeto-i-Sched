using UnityEngine;
using Firebase.Firestore;

public class FirestoreManager : MonoBehaviour
{
    [SerializeField] string userPath = "users_sheet";       // caminho do dado (provisório)

    private FirebaseFirestore firestore;            // firestore é usado como variável apra todas as operações, tanto escrita como leitura
    private ListenerRegistration listenerReg;       // variável apra registrar eventos no banco de dados

    public void RestoreData()
    {
        firestore = FirebaseFirestore.DefaultInstance;

        // Listener permite que a cada atualização no dado do banco, essa função seja chamada
        listenerReg = firestore.Document(userPath).Listen(snapshot =>
        {
            var data = snapshot.ConvertTo<UserData>();

            print("Data Restored!");
            print($"Username: {data.Username} | Password: {data.Password}");
        });
    }

    private void OnDestroy()
    {
        if (listenerReg != null)
        {
            // Interrompe o recebimento de informação após o objeto ser removidoda cena
            listenerReg.Stop();
        }
    }

    public void CreateUser(string userName, string password)
    {
        firestore = FirebaseFirestore.DefaultInstance;

        // cria uma nova struct de user
        UserData newUser = new UserData
        {
            Username = userName,
            Password = password
        };

        // envia novo dado para o servidor
        firestore.Document(userPath).SetAsync(newUser);
    }
}
