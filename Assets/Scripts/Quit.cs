using UnityEngine;

public class Quit : MonoBehaviour
{
    public void OnQuitButtonPressed()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // For Editor testing
#endif
    }
}
