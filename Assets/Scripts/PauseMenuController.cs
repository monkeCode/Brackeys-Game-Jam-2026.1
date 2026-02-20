using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController Instance { get; private set; }
    public GameObject pauseMenuPanel;
    public bool Paused => pauseMenuPanel.activeSelf;
    public string MainMenuScene = "MainMenu";
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

    public void ToMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }
}
