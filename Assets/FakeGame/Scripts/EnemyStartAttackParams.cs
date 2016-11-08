using UnityEngine;

namespace Fake.Enemy
{
    public sealed class EnemyStartAttackParams
    {
        public Transform SpawnTransform { get; private set; }
        public float AttackTime { get; private set; }
        public float BulletSpeed { get; private set; }
        public EnemyAttackKinds.AttackType AttackType { get; private set; }
        public EnemyAttackTypeDelegate Attack { get; private set; }

        public EnemyStartAttackParams(Transform spawnTrnasform, float attackTime, float bulletSpeed, EnemyAttackKinds.AttackType attackType, EnemyAttackTypeDelegate attack)
        {
            SpawnTransform = spawnTrnasform;
            AttackTime = attackTime;
            BulletSpeed = bulletSpeed;
            AttackType = attackType;
            Attack = attack;
        }

        public EnemyStartAttackParams(Transform spawnTrnasform, float bulletSpeed, EnemyAttackKinds.AttackType attackType, EnemyAttackTypeDelegate attack)
        {
            SpawnTransform = spawnTrnasform;
            BulletSpeed = bulletSpeed;
            AttackType = attackType;
            Attack = attack;
        }
        // 인자가 다른 새로 생성자 하나 생성
    }
}