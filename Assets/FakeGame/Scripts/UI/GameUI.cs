using UnityEngine;
using UnityEngine.UI;

namespace Fake.UI
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

            AppearStar(LifeStar, playerLife);
        }

        public void CheckPlayerPower(int checkPower)
        {
            playerPower = checkPower;

            AppearStar(PowerStar, playerPower);
        }

        public void DisappearAllStar(Image[] star)
        {
            for (int i = 0; i < 5; i++)
            {
                star[i].enabled = false;
            }
        }

        public void AppearStar(Image[] star, int num)
        {
            DisappearAllStar(star);

            for (int i = 0; i < num; i++)
            {
                star[i].enabled = true;
            }
        }

        public void AppearGameOver()
        {
            GameOver.gameObject.SetActive(true);
        }

        public void HideGameOver()
        {
            GameOver.gameObject.SetActive(false);
        }
    }
}