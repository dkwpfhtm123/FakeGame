
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fake
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<GameManager>();

                return instance;
            }
        }

        public GameObject SmallEnemyPrefab;
        public List<GameObject> SmallEnemyPool;
        public Transform PlayerTransform
        {
            get;
            set;
        }

        public int PlayerScore;

        private Transform transformCache;

        void Start()
        {
            SmallEnemyPool = new List<GameObject>();

            PlayerScore = 0;
        }

        void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}