using UnityEngine;
using UnityEngine.UI;

public class WavesSurvived : MonoBehaviour
{
    public Text waveText;
    public SpawnSet spawnScript;

    void Update()
    {
        if (waveText != null && spawnScript != null)
        {
            waveText.text = "Number of waves survived: " + spawnScript.waveNum;
        }
        else
        {
            Debug.Log("h");
        }
    }
}
