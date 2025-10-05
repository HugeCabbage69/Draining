using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public string sceneName = ""; // put ur scene name here

    public void LoadSceneNow()
    {
        SceneManager.LoadScene(sceneName);
    }
}
