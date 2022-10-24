using System.Collections;
using UnityEngine;
using TMPro;

public class UserLabel : MonoBehaviour
{
    [SerializeField] TMP_Text labelField;
    
    private IEnumerator Start()
    {
        if (FirebaseAuthenticator.instance.User == null)
        {
            yield return new WaitForSeconds(2f);
            if (FirebaseAuthenticator.instance.User == null) yield break;
        }

        labelField.text = FirebaseAuthenticator.instance.User.DisplayName;
    }
}
