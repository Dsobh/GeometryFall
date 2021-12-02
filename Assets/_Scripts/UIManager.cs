using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text multiplierText;
    private int score = 0;
    
    void OnEnable()
    {
        PlayerController.OnScoreChange += AddScore;
        PlayerController.OnMultiplierChange += UpdateMultiplier;
    }

    void OnDisable()
    {
        PlayerController.OnScoreChange -= AddScore;
        PlayerController.OnMultiplierChange -= UpdateMultiplier;
    }


    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "000000";
        multiplierText.text = "x1";
    }

    private void AddScore(int points)
    {
        score += points;
        if(score <0)
        {
            score = 0;
        }
        RefreshScoreText();
    }

    private void RefreshScoreText()
    {
        scoreText.text = BuildScoreString();
    }

    private string BuildScoreString()
    {
        string newText = score.ToString();
        while(newText.Length < 6)
        {
            newText = "0" + newText;
        }
        return newText;
    }

    private void UpdateMultiplier(int multiplier)
    {
        multiplierText.text = "x" + multiplier;
    }

}
