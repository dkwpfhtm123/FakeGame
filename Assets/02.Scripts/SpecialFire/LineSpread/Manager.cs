using UnityEngine;
using System.Collections;

namespace LineSpread
{
    public class Manager : MonoBehaviour
    {
        private static Manager instance;
        public static Manager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<Manager>();

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
