using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour
{
    public int HP = 5;
    public delegate void EnemyAttackTypeDelegate(Transform Tr, EnemyAttackType.AttackType Type);

    public enum EnemyType
    {
        SmallEnemy1,
        SmallEnemy2,
        SmallEnemy3
    }

    public EnemyType EnemyTypeCheck;

    private BulletTypeScript bulletType;

    void Start()
    {
        StartCoroutine(AttackPlayer());
     //   StartCoroutine(MoveEnemy()); 임시 주석상태
    }

    private IEnumerator MoveEnemy()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("start"), "time", 5, "easetype", iTween.EaseType.easeOutCubic));
        yield return new WaitForSeconds(2.0f);
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("top"), "time", 10, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.loop));
    }

    private IEnumerator AttackPlayer()
    {
        System.Action<Transform, EnemyAttackType.AttackType> attack;

        if (EnemyTypeCheck == EnemyType.SmallEnemy1)
        {
            attack = EnemyAttackType.Instance.FireConeType;
            while (true)
            {
                yield return new WaitForSeconds(2.0f); // 공격 텀
                attack(transform, EnemyAttackType.AttackType.RedAttack);
            }
        }
        else if (EnemyTypeCheck == EnemyType.SmallEnemy2)
        {
            attack = EnemyAttackType.Instance.FireBoomType;
            while (true)
            {
                yield return new WaitForSeconds(2.5f);
                attack(transform, EnemyAttackType.AttackType.BlueAttack);
            }
        }
        else if (EnemyTypeCheck == EnemyType.SmallEnemy3)
        {
            attack = EnemyAttackType.Instance.FireSinType;
            yield return new WaitForSeconds(0.5f);
            attack(transform, EnemyAttackType.AttackType.PurpleCircle);
        }
    }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<BulletTypeScript>())
        {
            bulletType = coll.gameObject.GetComponent<BulletTypeScript>();

            if (bulletType.BulletTypeCheck == BulletType.PlayerBullet)
            {
                Destroy(coll.gameObject);
                HP--;
            }
        }
        else if (coll.gameObject.GetComponent<ThisIsBoom>())
        {
            // 충돌하는 동안으로 바꿔야함.
            HP--;
        }

        if (HP < 0)
        {
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        ItemSpawn.Instance.SpawnItem(transform, ItemSpawn.ItemTypeObject.PowerItem);
        Destroy(gameObject);
    }
}
