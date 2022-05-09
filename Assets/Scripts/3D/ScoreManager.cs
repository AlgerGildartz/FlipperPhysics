using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private int scoreValue = 10;
    private int currentScore;

    public static ScoreManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this);
        }
        Instance = this;
    }

    public void AddScore()
    {
        currentScore += scoreValue;
        UpdateText();
    }

    public void ResetScore() { 
        currentScore = 0;
        UpdateText();
    }

    private void UpdateText()
    {
        scoreText.text = "Score : " + currentScore;
    }
}
