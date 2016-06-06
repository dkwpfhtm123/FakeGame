using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour
{
    public Text MaxScore;
    public Text NewScore;

    private int maxScore;
    private int newScore;

    void Start()
    {
        maxScore = PlayerPrefs.GetInt("MAX_SCORE", 0);
        MaxScore.text = "MAX SCORE " + maxScore.ToString();

        newScore = 0;

        CheckScore(0);
    }

    public void CheckScore(int plusScore)
    {
        newScore += plusScore;
        NewScore.text = "NEW SCORE " + newScore.ToString();

        if(maxScore < newScore)
        {
            maxScore = newScore;
            MaxScore.text = "MAX SCORE " + maxScore.ToString();

            PlayerPrefs.SetInt("MAX_SCORE", maxScore);
        }
    }
}
