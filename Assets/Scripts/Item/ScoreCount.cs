using UnityEngine;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int scoreCount;
    public Text scoreText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + scoreCount.ToString();
    }
}
