using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private float reloadLevelDelay = 1f;
    [SerializeField] private int coinScoreValue = 25;
    [SerializeField] private int enemyScoreValue = 100;
    [SerializeField] private int playerLives = 3;
    private int score = 0;

    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        // Guarantee that we only have one GameSession
        // throughout the entire game (Singleton Pattern)
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        livesText.text = "Lives: " + playerLives.ToString();
        scoreText.text = "Score: " + score.ToString("D3");
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            StartCoroutine(TakeLife());
        }
        else
        {
            StartCoroutine(GameOver());
        }
    }

    IEnumerator TakeLife()
    {
        yield return new WaitForSecondsRealtime(reloadLevelDelay);

        playerLives -= 1;
        livesText.text = "Lives: " + playerLives.ToString();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSecondsRealtime(reloadLevelDelay);

        Debug.Log("Game Over!");

        // Restart at the first level
        SceneManager.LoadScene(0);
        FindObjectOfType<ScenePersist>().Reset();
        Destroy(gameObject);
    }

    public void UpdateScore(string source)
    {
        if (source == "Coin")
        {
            score += coinScoreValue;
            scoreText.text = "Score: " + score.ToString("D3");
        }
        else if (source == "Enemy")
        {
            score += enemyScoreValue;
            scoreText.text = "Score: " + score.ToString("D3");
        }
    }

    public void Reset()
    {
        Destroy(gameObject);
    }
}
