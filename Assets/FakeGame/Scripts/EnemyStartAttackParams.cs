namespace Fake.Enemy
{
    public sealed class EnemyStartAttackParams
    {
        public float AttackTime { get; private set; }
        public float BulletSpeed { get; private set; }
        public EnemyAttackKinds.AttackType? AttackType { get; private set; }
        public EnemyAttackTypeDelegate Attack { get; private set; }

        public EnemyStartAttackParams(float attackTime, float bulletSpeed, EnemyAttackKinds.AttackType? attackType, EnemyAttackTypeDelegate attack)
        {
            AttackTime = attackTime;
            BulletSpeed = bulletSpeed;
            AttackType = attackType;
            Attack = attack;
        }

        public static EnemyStartAttackParams RedAttack(float attackTime, float bulletSpeed)
        {
            return new EnemyStartAttackParams(0, 0, EnemyAttackKinds.AttackType.RedAttack);
        }
        // 인자가 다른 새로 생성자 하나 생성
    }
}