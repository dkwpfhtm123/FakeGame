using UnityEngine;

namespace Fake.Player
{
    public class PlayerBullet : BaseBullet
    {
        private Transform transformCache;

        void Start()
        {
            transformCache = GetComponent<Transform>();
        }

        void Update()
        {
            transformCache.Translate(BulletDirection * Time.deltaTime * BulletSpeed, Space.Self); // 일정한 속도로 발사됨.
        }
    }
}