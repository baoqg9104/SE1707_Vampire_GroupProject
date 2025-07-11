using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPanelManager : MonoBehaviour
{
    public TextMeshProUGUI[] levelsScore;
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI highScoreText;
    public Button mainMenuButton;
    public Button quitButton;

    void Start()
    {
        if (mainMenuButton != null) mainMenuButton.onClick.AddListener(GoToMainMenu);
        if (quitButton != null) quitButton.onClick.AddListener(QuitGame);
    }

    void OnEnable()
    {
        PlayerPrefs.DeleteKey("LastLevel");

        int totalScore = 0;

        for (int i = 0; i < levelsScore.Length - 1; i++)
        {
            string levelKey = "LevelScore_Level" + (i + 1);
            int score = PlayerPrefs.GetInt(levelKey, 0);
            levelsScore[i].text = score + "";
            totalScore += score;
        }

        int finalBossScore = PlayerPrefs.GetInt("LevelScore_FinalBoss", 0);

        levelsScore[levelsScore.Length - 1].text = finalBossScore + "";

        totalScore += finalBossScore;
        totalScoreText.text = totalScore + "";

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (totalScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", totalScore);
            PlayerPrefs.Save();
            highScoreText.text = totalScore + "";
        }
        else
        {
            highScoreText.text = highScore + "";
        }
    }
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}