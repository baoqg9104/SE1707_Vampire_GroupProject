using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button tutorialButton;
    public Button settingsButton;
    public Button exitButton;
    public GameObject optionsPanel;
    public Button newGameButton;
    public Button continueButton;
    public Button backButton;

    void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        tutorialButton.onClick.AddListener(() => SceneManager.LoadScene("Tutorial"));
        settingsButton.onClick.AddListener(OnSettingsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        newGameButton.onClick.AddListener(OnNewGameClicked);
        continueButton.onClick.AddListener(OnContinueClicked);
        backButton.onClick.AddListener(OnBackClicked);

        optionsPanel.SetActive(false);
    }

    void OnPlayClicked()
    {
        optionsPanel.SetActive(true);
    }

    void OnNewGameClicked()
    {
        PlayerPrefs.DeleteKey("LastLevel");
        PlayerPrefs.DeleteKey("LevelScore_Level1");
        PlayerPrefs.DeleteKey("LevelScore_Level2");
        PlayerPrefs.DeleteKey("LevelScore_Level3");
        PlayerPrefs.DeleteKey("LevelScore_Level4");
        PlayerPrefs.DeleteKey("LevelScore_FinalBoss");
        

        SceneManager.LoadScene("Level1");
    }

    void OnContinueClicked()
    {
        string lastLevel = PlayerPrefs.GetString("LastLevel", "Level1");
        SceneManager.LoadScene(lastLevel);
    }

    void OnBackClicked()
    {
        optionsPanel.SetActive(false);
    }

    void OnSettingsClicked()
    {
        Settings.Instance.gameObject.SetActive(true);
    }
    
    void OnExitClicked()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}