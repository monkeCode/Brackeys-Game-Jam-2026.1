using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour
{
    public string MainMenuScene = "MainMenu";
    public void ToMainMenu()
    {
        SceneManager.LoadScene(MainMenuScene);
    }
}
