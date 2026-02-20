using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioMixer _mixer;
    public string MainMenuScene = "MainMenu";
    void Awake()
    {
        sfxSlider.maxValue = 0f;
        musicSlider.maxValue = 0f;
        sfxSlider.minValue = -80f;
        musicSlider.minValue = -80f;
        _mixer.GetFloat("Music", out float volume);
        musicSlider.value = volume;
        _mixer.GetFloat("SFX", out volume);
        sfxSlider.value = volume;
    }
    public void MusicChanged()
    {
        float volume = musicSlider.value;
        _mixer.SetFloat("Music", volume);
    }

    public void SfxChanged()
    {
        float volume = sfxSlider.value;
        _mixer.SetFloat("SFX", volume);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }
}
