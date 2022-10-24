using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActionsUtility : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneController.Instance.LoadScene(sceneName);
    }


}
