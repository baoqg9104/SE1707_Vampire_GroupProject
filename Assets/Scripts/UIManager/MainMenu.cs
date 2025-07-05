using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button settingsButton;
    public Button exitButton;
    // public GameObject selectLevelPanel;

    void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    void OnPlayClicked()
    {
        // selectLevelPanel.SetActive(true);
        SceneManager.LoadScene("Level 1 - Bao");
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