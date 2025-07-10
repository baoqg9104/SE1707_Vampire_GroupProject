using UnityEngine;
using UnityEngine.SceneManagement;

public class Endpoint : MonoBehaviour
{
    private bool isActivated = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}