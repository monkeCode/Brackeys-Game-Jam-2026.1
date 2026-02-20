using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController Instance { get; private set; }
    public GameObject pauseMenuPanel;
    public bool Paused => pauseMenuPanel.activeSelf;
    public string MainMenuScene = "MainMenu";
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioMixer _mixer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        sfxSlider.maxValue = 0f;
        musicSlider.maxValue = 0f;
        sfxSlider.minValue = -80f;
        musicSlider.minValue = -80f;
        _mixer.GetFloat("Music", out float volume);
        musicSlider.value = volume;
        _mixer.GetFloat("SFX", out volume);
        sfxSlider.value = volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            pauseMenuPanel.SetActive(!Paused);
        }
        ControlPause();
        // Debug.Log(Paused);
    }

    void ControlPause()
    {
        if (Paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
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
