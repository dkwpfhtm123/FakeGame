using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Fake
{
    namespace UI
    {
        public class GameUI : MonoBehaviour
        {
            public Image[] LifeStar = new Image[5];
            public Image[] PowerStar = new Image[5];

            public Text MaxScore;
            public Text NewScore;
            public Text PlayerLife;
            public Text PlayerPower;

            public Image GameOver;

            private int maxScore;
            private int newScore;
            private int playerLife;
            private int playerPower;

            void Start()
            {
                Screen.SetResolution(Screen.width, Screen.width * 16 / 9, true);

                maxScore = PlayerPrefs.GetInt("MAX_SCORE", 0);
                MaxScore.text = maxScore.ToString();

                newScore = 0;

                CheckScore(0);
            }

            public void CheckScore(int plusScore)
            {
                newScore += plusScore;
                NewScore.text = newScore.ToString();

                if (maxScore < newScore)
                {
                    maxScore = newScore;
                    MaxScore.text = maxScore.ToString();

                    PlayerPrefs.SetInt("MAX_SCORE", maxScore);
                    PlayerPrefs.Save();
                }
            }

            public void CheckPlayerLife(int checkLife)
            {
                playerLife = checkLife;

                Show(LifeStar, playerLife);
            }

            public void CheckPlayerPower(int checkPower)
            {
                playerPower = checkPower;

                Show(PowerStar, playerPower);
            }

            public void HideAll(Image[] star)
            {
                for (int i = 0; i < 5; i++)
                {
                    star[i].enabled = false;
                }
            }

            public void Show(Image[] star, int num)
            {
                HideAll(star);

                for (int i = 0; i < num; i++)
                {
                    star[i].enabled = true;
                }
            }

            public void AppearGameOver()
            {
                GameOver.GetComponent<Transform>().localScale = Vector3.one;
            }

            public void HideGameOver()
            {
                GameOver.GetComponent<Transform>().localScale = Vector3.zero;
            }
        }
    }
}