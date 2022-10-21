using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class FirebaseAuthenticator : MonoBehaviour
{
    public static FirebaseAuthenticator instance;
    public DependencyStatus dependecyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;

    public string currentUser;
    
    private void Awake() {
        if(instance==null){
            instance = this;
        }else{
            Destroy(gameObject);
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

    void Start()
    {

    }

    void InitializeFirebase(){
        auth = FirebaseAuth.DefaultInstance;
    }

    public void DefineUser(string str)
    {
        currentUser = str;
    }
}
