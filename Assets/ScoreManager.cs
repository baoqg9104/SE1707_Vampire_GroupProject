using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int scoreCount;
    public TextMeshProUGUI scoreText; // Change Text to TextMeshProUGUI

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       // Update the score text
        scoreText.text = "Score: " + scoreCount.ToString();
    }
}
