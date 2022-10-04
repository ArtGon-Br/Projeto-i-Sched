using System.Collections;
using System.Collections.Generic;
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
    [Space]
    [SerializeField] UnityEvent OnSuccesfullyLogged;

    public TMP_InputField emailInputfield, passwordInputfield;
    //public TMP_Text warningLoginText, confirmLoginText;
    //public GameObject verifyEmailMessage;

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
                return "Missing Email";
            case AuthError.MissingPassword:
                return "Missing Password";
            case AuthError.InvalidEmail:
                return "Invalid e-mail or password";
            case AuthError.WrongPassword:
                return "Invalid e-mail or password";
            case AuthError.UserNotFound:
                return "Account does not exist";
            case AuthError.UnverifiedEmail:
                return "Not Verified email. Please Verify it.";
            default:
                return "The error could not be found";
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
        LogMessage("Successfully logged!");
        Invoke("GoToMainPage", 1f);
    }

    void GoToMainPage()
    {
        OnSuccesfullyLogged.Invoke();
    }
}
