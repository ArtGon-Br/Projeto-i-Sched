using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Image loadImage;
    public static bool loading;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject.transform.parent.gameObject);
        StartCoroutine(LoadMainSceneAuto());
    }


    IEnumerator LoadMainSceneAuto()
    {
        yield return null;

        var operation = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Single);
        loading = true;

        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            var progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
                yield return new WaitForSeconds(2);
                Debug.Log("Loading completed");
                loading = false;
                Destroy(gameObject.transform.parent.gameObject);
            }
            yield return null;
        }
    }
}
