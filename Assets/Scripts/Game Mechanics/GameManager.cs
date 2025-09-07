using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool gameOver = false; 
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        gameOver = false; 
    }
    public void GameOver()
    {
        gameOver = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ResetGame()
    {
        gameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
