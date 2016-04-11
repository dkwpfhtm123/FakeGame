using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
    public Vector2 Direction;
    public float BulletSpeed;
    public float Angle;

    private Transform transformCache;
    private Vector2 startingPosition;

    private bool firstFrame;

    BulletTypeScript bulletType;

    void Start()
    {
        transformCache = GetComponent<Transform>();

        bulletType = gameObject.GetComponent<BulletTypeScript>();

        firstFrame = true;

        //@ 복잡한 수식은 함수로 만듭시다.
        //@ 특정 값으로 설정하고 싶을 때는 Rotate가 아니라 localRotation을 직접 설정하자
        float degree = -Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, degree);
    }

    void Update()
    {
        if (firstFrame == false)
        {
            MoveBullet(Time.deltaTime);
        }

        //@ 전체적인 흐름을 조정하자
        if (GameMgr.Instance.BoomActive == true)
        {
            ItemSpawn.instance.SpawnItem(gameObject.transform, ItemSpawn.ItemType.ScoreItem);
            Destroy(gameObject);
        }

        //@ 전체적인 흐름을 조정하자
        if (bulletType.BulletTypeCheck == BulletTypeScript.BulletType.BlueKnife)
        {
            StartCoroutine(BoomBullet());
        }

        firstFrame = false;
    }

    public void MoveBullet(float deltaTime)
    {
        if (transformCache == null)
        {
            transformCache = GetComponent<Transform>();
            startingPosition = transformCache.localPosition;
        }

        Vector2 bulletPosition = transformCache.localPosition;

        var position = transformCache.localPosition;
        position.x += Direction.x * BulletSpeed * deltaTime;
        position.y += Direction.y * BulletSpeed * deltaTime;
        
        transformCache.localPosition = position;
    }

    IEnumerator BoomBullet()
    {
        //@ cache된게 있으면 cache된걸 씁시다
        yield return new WaitForSeconds(1.0f);
        EnemyAttackType.Instance.FireType1(this.transform, EnemyAttackType.AttackType.RedAttack);
        Destroy(this.gameObject);
    }
}