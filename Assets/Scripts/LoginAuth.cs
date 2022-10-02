using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;

public class LoginAuth : MonoBehaviour
{
    public TMP_InputField emailInputfield, passwordInputfield;
    public TMP_Text warningLoginText, confirmLoginText;
    public GameObject verifyEmailMessage;

    public Transform canvas;
    
    public void LoginButton(){
        StartCoroutine(StartLogin(emailInputfield.text, passwordInputfield.text));
    }

    private IEnumerator StartLogin(string email, string password){
        var LoginTask = FirebaseAuthenticator.instance.auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: ()=> LoginTask.IsCompleted);

        if (LoginTask.Exception != null){
            HandleLoginErrors(LoginTask.Exception);
        }
        else{
            LoginUser(LoginTask);
        }
    }

    void HandleLoginErrors(System.AggregateException loginException){
        Debug.LogWarning(message: $"failed to login task with {loginException}");
        FirebaseException firebaseEx = loginException.GetBaseException() as FirebaseException;
        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
        
        warningLoginText.text = DefineLoginErrorMessage(errorCode);
    }

    string DefineLoginErrorMessage(AuthError errorCode){
        switch (errorCode)
        {
            case AuthError.MissingEmail:
                return "Missing Email";
            case AuthError.MissingPassword:
                return "Missing Password";
            case AuthError.InvalidEmail:
                return "Invalid Email";
            case AuthError.WrongPassword:
                return "Invalid password";
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
            Debug.Log(DefineLoginErrorMessage(AuthError.UnverifiedEmail));

            verifyEmailMessage.SetActive(true);
            return;
        }

        Debug.LogFormat("User Signed in successfully: {0} {1}", FirebaseAuthenticator.instance.User.DisplayName, FirebaseAuthenticator.instance.User.Email);
        warningLoginText.text = "";
        confirmLoginText.text = "Successfully";
    }
}
