using UnityEngine;
using System.Collections;

namespace Fake
{
    public class BaseBullet : MonoBehaviour
    {
        public bool DestroyWhenHitWall
        {
            set;
            get;
        }

        public GameObject bulletObject;
    }
}
