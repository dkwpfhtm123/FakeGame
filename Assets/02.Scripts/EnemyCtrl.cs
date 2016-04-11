using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour {

    public int HP = 5;

    EnemyTypeScript enemy;
    BulletTypeScript bullet;

    void Start () {
        enemy = GetComponent<EnemyTypeScript>();

        /* InvokeRepeating이 아니라 코루틴으로 재구현 */
        StartCoroutine(AttackPlayer());
        StartCoroutine(TopMove());
    }



    IEnumerator TopMove()
    {
 //       iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("start"), "time", 5, "easetype", iTween.EaseType.easeOutCubic));
        yield return new WaitForSeconds(2.0f);
 //       iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("top"), "time", 10, "easetype", iTween.EaseType.linear , "looptype", iTween.LoopType.loop));
    }

    IEnumerator AttackPlayer()
    {
        while (true)
        {
            //@ while문 밖에서 판단하기
            if (enemy.EnemyTypeCheck == EnemyTypeScript.EnemyType.SmallEnemy1)
            {
                yield return new WaitForSeconds(2.0f); // 공격 텀
                EnemyAttackType.Instance.FireType1(this.transform, EnemyAttackType.AttackType.RedAttack);
            }


            if (enemy.EnemyTypeCheck == EnemyTypeScript.EnemyType.SmallEnemy2)
            {
                yield return new WaitForSeconds(3.0f);
                EnemyAttackType.Instance.FireType2(this.transform, EnemyAttackType.AttackType.BlueAttack);
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

        //@ 한 번 확인
        if (HP < 0)
        {
            DeadEnemy();
        }
    } 

    //@ 동사 명사
    void DeadEnemy()
    {
        ItemSpawn.instance.SpawnItem(this.transform , ItemSpawn.ItemType.PowerItem);
        Destroy(this.gameObject);
    }
}
