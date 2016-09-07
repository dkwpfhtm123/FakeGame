using UnityEngine;
using System.Collections;

public class Network : MonoBehaviour {


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
        using (var www = new WWW("http://127.0.0.1:8000/hello/"))
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
    }
}
