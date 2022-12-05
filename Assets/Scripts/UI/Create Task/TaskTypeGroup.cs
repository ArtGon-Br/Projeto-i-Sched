using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTypeGroup : MonoBehaviour
{
    [SerializeField] bool fixedTaskGroup;

    public void SwitchVisibility(bool check)
    {
        gameObject.SetActive(fixedTaskGroup ? check : !check);
    }
}
