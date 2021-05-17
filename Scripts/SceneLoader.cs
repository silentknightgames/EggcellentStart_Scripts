using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator Loader;

    #region Singleton

    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public IEnumerator LoadNextScene(string sceneName)
    {
        Loader.SetTrigger("LoadOut");

        yield return new WaitForSeconds(0.75f);

        SceneManager.LoadScene(sceneName);
    }
}
