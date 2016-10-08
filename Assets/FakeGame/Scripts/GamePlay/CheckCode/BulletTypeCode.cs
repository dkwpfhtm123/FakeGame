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

    public class BulletTypeCode : MonoBehaviour
    {
        public BulletType BulletTypeCheck;
    }
}