using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> persistentObjects;

    static bool hasSpawned = false;

    private void Awake()
    {
        if(!hasSpawned)
        {
            SpawnObjects();
            hasSpawned = true;
        }
    }

    void Start()
    {
        
    }

    void SpawnObjects()
    {
        foreach(var obj in persistentObjects)
        {
            GameObject instance =  Instantiate(obj);
            DontDestroyOnLoad(instance);
        }
    }
}
