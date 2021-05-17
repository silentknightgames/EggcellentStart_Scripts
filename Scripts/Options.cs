using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    public AudioMixer audioMixer;
    [SerializeField] private Slider bgmSlider, effectSlider;

    private void Start()
    {
        float bgmVol, effectVol;

        audioMixer.GetFloat("bgm", out bgmVol);
        bgmSlider.value = bgmVol;

        audioMixer.GetFloat("effect", out effectVol);
        effectSlider.value = effectVol;
    }

    public void ShowSettings()
    {
        optionPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideSettings()
    {
        optionPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void SetBGM(float volume)
    {
        audioMixer.SetFloat("bgm", volume);
    }

    public void SetEffects(float volume)
    {
        audioMixer.SetFloat("effect", volume);
    }
}
