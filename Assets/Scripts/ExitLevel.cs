using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Level finish.");
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        var currentLevelIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentLevelIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(currentLevelIndex + 1);
            FindObjectOfType<ScenePersist>().Reset();
        }
        else
        {
            Debug.Log("Congratulations! You beat the game.");
            
            // Restart at the first level
            SceneManager.LoadScene(0);

            FindObjectOfType<ScenePersist>().Reset();
            FindObjectOfType<GameSession>().Reset();
        }
    }
}
