using UnityEngine;
using System.Collections;

namespace Fake
{
    public class TurnRoundObject : MonoBehaviour
    {
        private float angle;
        private Transform transformCache;
        //  private float degree;

        void Start()
        {
            transformCache = GetComponent<Transform>();
            angle = 0;
        }

        void Update()
        {
            //     degree = angle * Mathf.Deg2Rad;
            transformCache.localRotation = Quaternion.Euler(0, 0, -angle);
            angle += 3;
            angle %= 360;
        }
    }
}