using UnityEngine;
using Firebase.Firestore;

public class FirestoreManager : MonoBehaviour
{
    [SerializeField] string userPath = "users_sheet";       // caminho do dado (provis�rio)

    private FirebaseFirestore firestore;            // firestore � usado como vari�vel apra todas as opera��es, tanto escrita como leitura
    private ListenerRegistration listenerReg;       // vari�vel apra registrar eventos no banco de dados

    public void RestoreData()
    {
        firestore = FirebaseFirestore.DefaultInstance;

        // Listener permite que a cada atualiza��o no dado do banco, essa fun��o seja chamada
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
            // Interrompe o recebimento de informa��o ap�s o objeto ser removidoda cena
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
