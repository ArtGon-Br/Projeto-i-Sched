using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;


public class RegisterAuth : MonoBehaviour
{
    [SerializeField] Color errorColor = Color.red;

    public TMP_InputField userNameRegisterField, emailRegisterField, passwordRegisterField, confirmPasswordField;
    public TMP_Text       messageBox;

    private void Awake()
    {
        messageBox.enabled = false;
    }

    public void RegisterButton(){
        StartCoroutine(StartRegister(emailRegisterField.text,passwordRegisterField.text, userNameRegisterField.text));
    }

    void LogMessage(string message)
    {
        LogMessage(message, Color.white);
    }

    void LogMessage(string message, Color color)
    {
        messageBox.enabled = true;
        messageBox.color = color;
        messageBox.text = message;
    }

    private IEnumerator StartRegister(string email, string password, string userName){
        if(!CheckRegistrationFieldsAndReturnForErrors()){
            var RegisterTask = FirebaseAuthenticator.instance.auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if(RegisterTask.Exception != null)
            {
                HandleRegisterErrors(RegisterTask.Exception);
            }
            else
            {
                StartCoroutine(RegisterUser(RegisterTask, userName, email, password));
            }
        }
    }

    bool CheckRegistrationFieldsAndReturnForErrors(){
        if(userNameRegisterField.text == ""){
            LogMessage("Nome de usuario vazio", errorColor);
            return true;
        }
        if(passwordRegisterField.text != confirmPasswordField.text)
        {
            LogMessage("Senhas diferentes", errorColor);
            return true;
        }
        else{
            return false;
        }
    }

    void HandleRegisterErrors(System.AggregateException registerException){
        Debug.LogWarning(message: $"Failed to register task with {registerException}");
        FirebaseException firebaseEx = registerException.GetBaseException() as FirebaseException;
        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

        LogMessage(DefineRegisterErrorMessage(errorCode), errorColor);
    }

    string DefineRegisterErrorMessage(AuthError errorCode){
        switch (errorCode)
        {
            case AuthError.MissingEmail:
                return "E-mail não preenchido";
            case AuthError.MissingPassword:
                return "Por favor insira a senha";
            case AuthError.WeakPassword:
                return "Senha fraca";
            case AuthError.InvalidEmail:
                return "E-mail inválido";
            case AuthError.EmailAlreadyInUse:
                return "Este e-mail já está cadastrado";
            default:
                return "Erro não especificado!";
        }
    }

    private IEnumerator RegisterUser(System.Threading.Tasks.Task<Firebase.Auth.FirebaseUser> registerTask, string displayName, string email, string password){
        FirebaseAuthenticator.instance.User = registerTask.Result;

        if(FirebaseAuthenticator.instance.User != null){
            UserProfile profile  = new UserProfile{DisplayName = displayName};
            var ProfileTask = FirebaseAuthenticator.instance.User.UpdateUserProfileAsync(profile);

            yield return new WaitUntil (predicate: () => ProfileTask.IsCompleted);

            if(ProfileTask.Exception != null){
                HandleProfileCreationErrors(ProfileTask.Exception);
            }else{
                Debug.Log("User set successfully");
                LogMessage("Usuário criado. confirmação de e-mail pendente. Por favor, verifique sua caixa de entrada e confirme o e-mail.");
                SendEmailToUser(FirebaseAuthenticator.instance.User);
            }
        }

        void SendEmailToUser(FirebaseUser user)
        {
            user.SendEmailVerificationAsync().ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendEmailVerificationAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendEmailVerificationAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Email sent successfully.");
            }
            );
        }

        void HandleProfileCreationErrors(System.AggregateException profileException){
             Debug.LogWarning(message: $"Failed to register taks with {profileException}");
             FirebaseException firebaseEx = profileException.GetBaseException() as FirebaseException;
             AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
             LogMessage("Falha ao registrar usuário", errorColor);
        }
    }
}
