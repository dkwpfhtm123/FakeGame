using UnityEngine;
using System.Collections;

namespace Fake
{
    public enum BulletType
    {
        RedKnife,
        BlueKnife,
        PurpleCircle,
        PlayerBullet,
        PlayerBoom,
    }

    public class BaseBullet : MonoBehaviour
    {
        private bool destroyWhenHitWall;
        public bool DestroyWhenHitWall
        {
            private set
            {
                destroyWhenHitWall = value;
            }
            get
            {
                return destroyWhenHitWall;
            }
        }

        public float BulletDamage;
        public float BulletSpeed;
        public Vector2 BulletDirection;

        public BulletType BulletTypeCheck;

        public void SetBaseBullet(float bulletDamage, float bulletSpeed, Vector2 bulletDirection, bool destroyWhenHitWall)
        {
            BulletDamage = bulletDamage;
            BulletSpeed = bulletSpeed;
            BulletDirection = bulletDirection;
            DestroyWhenHitWall = destroyWhenHitWall;
        }
    }
}
