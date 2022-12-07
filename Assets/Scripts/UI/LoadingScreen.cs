using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Image loadImage;
    private int prevScene;
    public int scene;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        prevScene = scene = 0;
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
            loadImage.fillAmount += 0.1f;
            if (loadImage.fillAmount == 1) loadImage.fillAmount = 0;

            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(6);
                DestroyGameObject();
                Debug.Log("Loading completed");
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }
    public void DestroyGameObject() => Destroy(gameObject);
}
