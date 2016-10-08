using UnityEngine;
using System.Collections;

namespace Fake.Player
{
    public class PlayerBullet : BaseBullet
    {
        public float Damage;
        public float Speed;
        //          public BaseBullet.BulletType type;

        private Transform transformCache;

        void Start()
        {
            transformCache = GetComponent<Transform>();
        }

        void Update()
        {
            transformCache.Translate(Vector2.up * Time.deltaTime * Speed, Space.Self); // 일정한 속도로 발사됨.
        }
    }
}