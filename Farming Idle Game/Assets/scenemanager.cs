using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemanager : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            LoadMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            LoadGame();
        }
    }
    // Load scene by build index (0 = menu, 1 = game)
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Optional: specifically load the main game (scene 1)
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    // Optional: go back to menu (scene 0)
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Optional: quit game
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}