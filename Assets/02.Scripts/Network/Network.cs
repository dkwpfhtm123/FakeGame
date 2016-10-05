using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Fake { 
public class Network : MonoBehaviour
{
    public Text[] HighScore;

    struct Ranker
    {
        public readonly int Rank;
        public readonly string Name;
        public readonly int Score;
        public readonly string Date;
        public Ranker(int rank, string name, int score, string date)
        {
            Rank = rank;
            Name = name;
            Score = score;
            Date = date;
        }
    }

        IEnumerator Start()
        {
            /*     using (var www = new WWW("http://127.0.0.1:8000/hello/"))
                 {
                     yield return www;
                     var response = www.text;
                     Debug.Log(response);

                     var result = Halak.JValue.Parse(response);
                     foreach (var rankerValue in result.Array())
                     {
                         var ranker = new Ranker(
                             rankerValue["rank"],
                             rankerValue["name"],
                             rankerValue["score"],
                             rankerValue["date"]);

                         Debug.Log(ranker.Rank + " / " + ranker.Name + " / " + ranker.Score + " / " + ranker.Date);
                     }
                 }
                 */

            using (var www = new WWW("http://127.0.0.1:8000/getscore/"))
            {
                yield return www;
                var response = www.text;

                var result = Halak.JValue.Parse(response);

                Ranker[] data = new Ranker[9];
                int length = 0;

                for (int i = 0; i < data.Length; i++)
                {
                    HighScore[i].text = (i + 1).ToString() + "  NON  0";
                }

                foreach (var rankerValue in result.Array())
                {
                    var ranker = new Ranker(
                        rankerValue["rank"],
                        rankerValue["name"],
                        rankerValue["score"],
                        rankerValue["date"]);

                    data[length] = new Ranker(ranker.Rank, ranker.Name, ranker.Score, ranker.Date);
                    length++;
                }

                for (int i = 0; i < length; i++)
                {
                    for (int x = 0; x < length; x++)
                    {
                        Ranker temp;
                        if (data[x].Score < data[x + 1].Score)
                        {
                            temp = data[x];
                            data[x] = data[x + 1];
                            data[x + 1] = data[x];
                        }
                        x++;
                    }
                }

                for (int i = 0; i < length; i++)
                {
                    HighScore[i].text = (i + 1).ToString() + "  " + data[i].Name + "  " + data[i].Score.ToString();
                }

            }
        }
    }
}
