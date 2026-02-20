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
    public void MusicChanged()
    {
        float volume = musicSlider.value > 0.01f ? 20f * Mathf.Log10(musicSlider.value) : -80f;
        _mixer.SetFloat("Music", volume);
    }

    public void SfxChanged()
    {
        float volume = sfxSlider.value > 0.01f ? 20f * Mathf.Log10(sfxSlider.value) : -80f;
        _mixer.SetFloat("SFX", volume);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }
}
