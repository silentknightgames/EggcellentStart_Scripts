using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(SceneLoader.Instance.LoadNextScene("Cutscenes"));
    }

}
