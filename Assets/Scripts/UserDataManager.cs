using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Auth;
using Firebase.Extensions;

public class UserDataManager : MonoBehaviour
{
    FirebaseAuth _auth;
    FirebaseFirestore _db;
    ListenerRegistration _listenerRegistration;

    int _tasks;
    bool _initialized = false;

    private void InitializeFirebase() {
        _auth = FirebaseAuth.DefaultInstance;
        _db = FirebaseFirestore.DefaultInstance;
        _initialized = true;
    }

    public void AddFixedTask(FixedTaskData newTask) {
        if (!_initialized) InitializeFirebase();
        var Firestore = FirebaseFirestore.DefaultInstance;
        Firestore.Collection(path: "agendas").Document(path: GetUserID()).Collection(path: "fixed").Document().SetAsync(newTask);
    }

    public void AddDynamicTask(DynamicTaskData newTask) {
        if (!_initialized) InitializeFirebase();
        var Firestore = FirebaseFirestore.DefaultInstance;
        Firestore.Collection(path: "agendas").Document(path: GetUserID()).Collection(path: "dynamic").Document().SetAsync(newTask);
    }

    public async void DeleteFixedTask(string taskID) {
        if (!_initialized) InitializeFirebase();
        var Firestore = FirebaseFirestore.DefaultInstance;
        DocumentReference taskRef = Firestore.Collection(path: "agendas").Document(path: GetUserID()).Collection(path: "fixed").Document(taskID);
        await taskRef.DeleteAsync();
    }

    public async void DeleteDynamicTask(string taskID) {
        if (!_initialized) InitializeFirebase();
        var Firestore = FirebaseFirestore.DefaultInstance;
        DocumentReference taskRef = Firestore.Collection(path: "agendas").Document(path: GetUserID()).Collection(path: "dynamic").Document(taskID);
        await taskRef.DeleteAsync();
    }

    public CollectionReference GetFixedRef() {
        if (!_initialized) InitializeFirebase();
        var Firestore = FirebaseFirestore.DefaultInstance;
        return Firestore.Collection(path: "agendas").Document(path: GetUserID()).Collection(path: "fixed");
    }

    public CollectionReference GetDynamicRef() {
        if (!_initialized) InitializeFirebase();
        var Firestore = FirebaseFirestore.DefaultInstance;
        return Firestore.Collection(path: "agendas").Document(path: GetUserID()).Collection(path: "dynamic");
    }

    private string GetUserID()
    {
        return _auth.CurrentUser.UserId.ToString();
    }
}
