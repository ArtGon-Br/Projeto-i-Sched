using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;


public class RegisterAuth : MonoBehaviour
{
    public TMP_InputField userNameRegisterField, emailRegisterField, passwordRegisterField, confirmPasswordField;
    public TMP_Text       warningRegisterText;
    public void RegisterButton(){
        StartCoroutine(StartRegister(emailRegisterField.text,passwordRegisterField.text, userNameRegisterField.text));
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
            warningRegisterText.text = "Nome de usuario vazio";
            return true;
        }
        if(passwordRegisterField.text != confirmPasswordField.text){
            warningRegisterText.text = "Senhas diferentes";
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

        warningRegisterText.text = DefineRegisterErrorMessage(errorCode);
    }

    string DefineRegisterErrorMessage(AuthError errorCode){
        switch (errorCode)
        {
            case AuthError.MissingEmail:
                return "Missing email";
            case AuthError.MissingPassword:
                return "Missing password";
            case AuthError.WeakPassword:
                return "Weak password";
            case AuthError.InvalidEmail:
                return "Invalid Email";
            case AuthError.EmailAlreadyInUse:
                return "Emails already in use";
            default:
                return "Error could not be found";
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
            }
        }

        void HandleProfileCreationErrors(System.AggregateException profileException){
             Debug.LogWarning(message: $"Failed to register taks with {profileException}");
             FirebaseException firebaseEx = profileException.GetBaseException() as FirebaseException;
             AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
             warningRegisterText.text = "Username set failed";
        }
    }
}
