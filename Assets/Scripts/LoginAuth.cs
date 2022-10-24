using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Firebase;
using Firebase.Auth;
using TMPro;

public class LoginAuth : MonoBehaviour
{
    [SerializeField] TMP_Text messageBox;
    [SerializeField] GameObject loginButton;
    [SerializeField] GameObject loadingCircle;
    [SerializeField] string dashboardScene = "Main";
    [Space]
    [SerializeField] UnityEvent OnSuccesfullyLogged;


    public TMP_InputField emailInputfield, passwordInputfield;

    private void Awake()
    {
        messageBox.enabled = false;
        loadingCircle.SetActive(false);
    }

    void LogMessage(string message)
    {
        messageBox.enabled = true;
        messageBox.text = message;
    }

    public void LoginButton(){
        StartCoroutine(StartLogin(emailInputfield.text, passwordInputfield.text));
    }

    private IEnumerator StartLogin(string email, string password){
        
        var LoginTask = FirebaseAuthenticator.instance.auth.SignInWithEmailAndPasswordAsync(email, password);

        // animar loading
        loginButton.SetActive(false);
        loadingCircle.SetActive(true);

        yield return new WaitUntil(predicate: ()=> LoginTask.IsCompleted);

        loadingCircle.SetActive(false);

        if (LoginTask.Exception != null)
        {
            HandleLoginErrors(LoginTask.Exception);
            loginButton.SetActive(true);
        }
        else
        {
            LoginUser(LoginTask);
        }
    }

    void HandleLoginErrors(System.AggregateException loginException){
        Debug.LogWarning(message: $"failed to login task with {loginException}");
        FirebaseException firebaseEx = loginException.GetBaseException() as FirebaseException;
        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

        LogMessage(DefineLoginErrorMessage(errorCode));
    }

    string DefineLoginErrorMessage(AuthError errorCode){
        switch (errorCode)
        {
            case AuthError.MissingEmail:
                return "E-mail não preenchido";
            case AuthError.MissingPassword:
                return "Senha não preenchida";
            case AuthError.InvalidEmail:
                return "E-mail e/ou senha não conferem";
            case AuthError.WrongPassword:
                return "E-mail e/ou senha não conferem";
            case AuthError.UserNotFound:
                return "Conta não existe";
            case AuthError.UnverifiedEmail:
                return "E-mail não verificado. Por favor verifique seu e-mail.";
            default:
                return "Erro não especificado";
        }
    }

    void LoginUser(System.Threading.Tasks.Task<Firebase.Auth.FirebaseUser> loginTask){
        FirebaseAuthenticator.instance.User = loginTask.Result;

        if (!FirebaseAuthenticator.instance.User.IsEmailVerified) 
        {
            LogMessage(DefineLoginErrorMessage(AuthError.UnverifiedEmail));
            loginButton.SetActive(true);
            return;
        }

        Debug.LogFormat("User Signed in successfully: {0} {1}", FirebaseAuthenticator.instance.User.DisplayName, FirebaseAuthenticator.instance.User.Email);

        // Printa mensagem de sucesso e chama função para logar
        LogMessage("Logado com sucesso!");
        Invoke("GoToMainPage", 1f);
    }

    void GoToMainPage()
    {
        OnSuccesfullyLogged.Invoke();
        SceneController.Instance.LoadScene(dashboardScene);
    }
}
