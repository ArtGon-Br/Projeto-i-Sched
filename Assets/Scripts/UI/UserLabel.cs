using UnityEngine;
using TMPro;

public class UserLabel : MonoBehaviour
{
    [SerializeField] TMP_Text labelField;

    private void Start()
    {
        labelField.text = FirebaseAuthenticator.instance.User.DisplayName;
    }
}
