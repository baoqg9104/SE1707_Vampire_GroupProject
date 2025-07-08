using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMenuBtn : MonoBehaviour
{
    void Start()
    {
        Button backToMenuButton = GetComponent<Button>();
        backToMenuButton.onClick.AddListener(OnBackToMenuClicked);
    }

    public void OnBackToMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}