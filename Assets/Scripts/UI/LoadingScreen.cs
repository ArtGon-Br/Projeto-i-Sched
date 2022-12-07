using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Image loadImage;

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
                Destroy(gameObject.transform.parent.gameObject);
            }
            yield return null;
        }
    }
}
