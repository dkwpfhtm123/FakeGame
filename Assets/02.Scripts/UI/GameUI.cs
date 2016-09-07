using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUI : MonoBehaviour
{
    public Text MaxScore;
    public Text NewScore;
    public Text PlayerLife;
    public Text PlayerPower;

    private int maxScore;
    private int newScore;
    private int playerLife;
    private int playerPower;

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

        if (maxScore < newScore)
        {
            maxScore = newScore;
            MaxScore.text = "MAX SCORE " + maxScore.ToString();

            PlayerPrefs.SetInt("MAX_SCORE", maxScore);
            PlayerPrefs.Save();
        }
    }

    public void CheckPlayerLife(int checkLife)
    {
        playerLife += checkLife;
        string stars = CheckStars(checkLife);

        PlayerLife.text = "LIFE    " + stars;
    }

    public void CheckPlayerPower(int checkPower)
    {
        playerPower += checkPower;
        string stars = CheckStars(checkPower);

        PlayerPower.text = "POWER   " + stars;
    }

    private string CheckStars(int number)
    {
        string star = "★ ";
        string manyStars = "";

        int check = number;
        while (check > 0)
        {
            manyStars += star;
            check--;
        }

        return manyStars;
    }
}
