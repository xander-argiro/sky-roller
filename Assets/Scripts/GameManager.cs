using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public string score = "0";
    public TMP_Text scoreText;

    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        score = "0";
        scoreText.text = "Score: 0";
    }

    void Update()
    {
        updateScore();
    }

    void updateScore()
    {
        if (PlayerMovement.GAME_OVER)
        {
            return;
        }
        
        score = Mathf.Round(player.position.z).ToString();
        scoreText.text = "Score: " + score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerMovement.GAME_OVER = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
