using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;
    private int score = 0;
    
    void OnEnable()
    {
        PlayerController.OnScoreChange += AddScore;
    }

    void OnDisable()
    {
        PlayerController.OnScoreChange -= AddScore;
    }


    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "000000";
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

}
