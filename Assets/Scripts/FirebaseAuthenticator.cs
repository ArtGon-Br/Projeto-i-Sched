using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System;

public class FirebaseAuthenticator : MonoBehaviour
{
    public static FirebaseAuthenticator instance;
    public DependencyStatus dependecyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    public static bool dontRememberMe;

    [SerializeField] GameObject loadingScreen;

    private void Awake() {
        if(instance==null){
            instance = this;
        }else{
            Destroy(gameObject);
            return;
        }
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependecyStatus = task.Result;
            if(dependecyStatus == DependencyStatus.Available){
                InitializeFirebase();
            }else{
                Debug.LogError("could not resolve all firebase dependencies" + dependecyStatus);
            }
        });
    }

    void InitializeFirebase(){
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != User)
        {
            bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && User != null)
            {
                Debug.Log("Signed out " + User.UserId);
            }
            User = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + User.UserId);
                AutoLogin();
            }
        }
    }

    void AutoLogin()
    {
        Debug.LogFormat("User Signed in successfully: {0} {1}", FirebaseAuthenticator.instance.User.DisplayName, FirebaseAuthenticator.instance.User.Email);
        Instantiate(loadingScreen);
    }

    public void Logout()
    {
        FirebaseAuth.DefaultInstance.SignOut();
    }

    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    private void OnApplicationQuit()
    {
        if (dontRememberMe)
            FirebaseAuth.DefaultInstance.SignOut();
    }

}
