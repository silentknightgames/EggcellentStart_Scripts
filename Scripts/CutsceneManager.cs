using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera, doorButton, arrow;
    [SerializeField] private AudioClip doorAudio;
    private AudioSource audioSource;

    private void Start()
    {
        StreetManager.goHome = 0;
        audioSource = GetComponent<AudioSource>();
    }

    public void MoveCamToTv()
    {
        mainCamera.transform.position = new Vector3(20, 0, -10);
    }

    public void MoveCamToOriginal()
    {
        mainCamera.transform.position = new Vector3(0, 0, -10);
    }

    public void EnableDoor()
    {
        doorButton.SetActive(true);
        arrow.SetActive(true);
    }

    public void DoorButtonHit()
    {
        audioSource.PlayOneShot(doorAudio, 1f);
        StartCoroutine(SceneLoader.Instance.LoadNextScene("StreetScene"));
    }
}
