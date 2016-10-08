using UnityEngine;
using System.Collections;

namespace Fake.LineSpread
{
    public class LineSpreadManager : MonoBehaviour
    {
        private static LineSpreadManager instance;
        public static LineSpreadManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<LineSpreadManager>();

                return instance;
            }
        }

        public bool OnCollision;
        public bool WaitTime;

        void Start()
        {
            OnCollision = false;
            WaitTime = false;
        }

        void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}