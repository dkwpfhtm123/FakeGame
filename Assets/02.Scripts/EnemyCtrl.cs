using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour
{
    public int HP = 5;

    public enum EnemyType
    {
        SmallEnemy1,
        SmallEnemy2
    }
    public EnemyType EnemyTypeCheck;

    private BulletTypeScript bullet;

    void Start()
    {
        StartCoroutine(AttackPlayer());
        StartCoroutine(TopMove());
    }

    private IEnumerator TopMove()
    {
        //       iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("start"), "time", 5, "easetype", iTween.EaseType.easeOutCubic));
        yield return new WaitForSeconds(2.0f);
        //       iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("top"), "time", 10, "easetype", iTween.EaseType.linear , "looptype", iTween.LoopType.loop));
    }

    private IEnumerator AttackPlayer()
    {
        //@ 위임, delegate
        
        if (EnemyTypeCheck == EnemyType.SmallEnemy1)
        {
            while (true)
            {
                yield return new WaitForSeconds(2.0f); // 공격 텀
                EnemyAttackType.Instance.ConFireType(this.transform, EnemyAttackType.AttackType.RedAttack);
            }
        }

        if (EnemyTypeCheck == EnemyType.SmallEnemy2)
        {
            while (true)
            {
                yield return new WaitForSeconds(3.0f);
                EnemyAttackType.Instance.BoomFireType(this.transform, EnemyAttackType.AttackType.BlueAttack);
            }
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<BulletTypeScript>())
        {
            bullet = coll.gameObject.GetComponent<BulletTypeScript>();
        }

        if (bullet.BulletTypeCheck == BulletTypeScript.BulletType.PlayerBullet)
        {
            Destroy(coll.gameObject);
            HP--;
        }

        // 충돌하는 동안으로 바꿔야함.
        else if (coll.gameObject.GetComponent<ThisIsBoom>())
        {
            HP--;
        }

        if (HP < 0)
        {
            killEnemy();
        }
    }

    private void killEnemy()
    {
        ItemSpawn.Instance.SpawnItem(transform, ItemSpawn.ItemType.PowerItem);
        Destroy(gameObject);
    }
}
