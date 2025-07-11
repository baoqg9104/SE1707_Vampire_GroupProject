using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endpoint : MonoBehaviour
{
    private bool isActivated = false;
    public GameObject winPanel;
    public ScoreManager scoreManager;

    void Start()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;

            SaveLevelScore();

            if (SceneManager.GetActiveScene().name == "FinalBoss")
            {
                DisplayWinPanel();
            }
            else
            {
                LoadNextLevel();
            }
        }
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private void SaveLevelScore()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        string levelKey = "LevelScore_" + currentSceneName;
        int currentScore = scoreManager.scoreCount;
        PlayerPrefs.SetInt(levelKey, currentScore);
        PlayerPrefs.Save();
    }

    private void DisplayWinPanel()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }
}