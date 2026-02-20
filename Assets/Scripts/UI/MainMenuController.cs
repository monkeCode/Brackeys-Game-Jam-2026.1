using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public string startScene = "testAlex";
    public string optionsScene = "OptionsMenu";

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }

    public void ToOptions()
    {
        SceneManager.LoadScene(optionsScene);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
