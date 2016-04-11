using UnityEngine;
using System.Collections;

public class EnemyAttackType : MonoBehaviour {

    public GameObject RedKnife;
    public GameObject BlueKnife;

    public float BulletSpeed;

    public enum AttackType
    {
        RedAttack,
        BlueAttack
    }

    //@ 대소문자
    public static EnemyAttackType Instance = null;

    void Awake()
    {
        Instance = this;
    }


    //@ 동사 명사
    public void FireType1(Transform spawnTransform , AttackType attackType) // 원뿔형
    {
        float oneshot = 5.0f;
        float angle = 60.0f * Mathf.Deg2Rad;
        float anglePlus = angle / (oneshot - 1);
        angle *= 0.5f;
        BulletSpeed = 2.0f;

        while (0 < oneshot)
        {
            Vector2 targetDirection = BulletTurn(angle , spawnTransform);
            BulletSetTransform(targetDirection, spawnTransform, attackType);

            oneshot--;
            angle -= anglePlus;
        }
    }

    public void FireType2(Transform spawnTransform , AttackType enemyType) // 무작위 발사 후 원뿔형 폭파
    {
        float oneshot = 5.0f;

        while (0 < oneshot)
        {
            float angle = Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
            BulletSpeed = Random.Range(1.0f, 2.0f);

            Vector2 targetDirection = BulletTurn(angle , spawnTransform);
            BulletSetTransform(targetDirection, spawnTransform, enemyType);

            oneshot--;
        }
    }

    // sin 그래프를 따라서 나가는 탄 구현중. 지금은 실행하면 유니티가 멈춤.
    public void FireType3(Transform spawnTransform , AttackType enemyType)
    {
        float oneshot = 5.0f;
        while (0 < oneshot)
        {
            SinType(spawnTransform);
        }
    }

    void SinType(Transform spawnTransform)
    {
        float dx = 0.0f;
        while (dx < 360.0f)
        {
            dx = 0.0f * Mathf.Deg2Rad;
            float dy = Mathf.Sin(dx);

            GameObject bullet = GameObject.Instantiate(RedKnife);
            Transform bulletTransformCache = bullet.transform;
            EnemyBullet obj = bullet.GetComponent<EnemyBullet>();

            bulletTransformCache.localPosition = spawnTransform.localPosition;
            bulletTransformCache.localRotation = Quaternion.identity;
            bulletTransformCache.localScale = Vector3.one;

            Vector2 direction = new Vector2(dx, dy);

            obj.Direction = direction;
            obj.BulletSpeed = 3.0f;
            obj.MoveBullet(Time.deltaTime);

            dx += 0.1f;
        }
    } 

    // 총알 회전
    Vector2 BulletTurn(float angle , Transform spawnTransform)
    {
        Transform playerTransform = GameMgr.Instance.PlayerTransform;
        Vector2 targetDirection = (playerTransform.transform.position - spawnTransform.transform.position).normalized;

        Vector2 targetVector = targetDirection;

        targetDirection.x = targetVector.x * Mathf.Cos(angle) - targetVector.y * Mathf.Sin(angle);
        targetDirection.y = targetVector.x * Mathf.Sin(angle) + targetVector.y * Mathf.Cos(angle);

        return targetDirection;
    }

    void BulletSetTransform(Vector2 targetDirection, Transform spawnTransform , AttackType enemyType)
    {
        // 총알을 생성합니다.
        GameObject Bullet = null;   
        if (enemyType == AttackType.RedAttack)
        {
            Bullet = GameObject.Instantiate(RedKnife);

        }
        
        //@ else의 소중함
        else if (enemyType == AttackType.BlueAttack)
        {
            Bullet = GameObject.Instantiate(BlueKnife);
        }

        //@ 변수 이름
        Transform bulletTransformCache = Bullet.transform;
        EnemyBullet obj = Bullet.GetComponent<EnemyBullet>();

        bulletTransformCache.localPosition = spawnTransform.localPosition;
        bulletTransformCache.localRotation = Quaternion.identity;
        bulletTransformCache.localScale = Vector3.one;

        obj.Direction = targetDirection.normalized;
        obj.BulletSpeed = BulletSpeed;
        obj.MoveBullet(Time.deltaTime);
    }



    /*  
             private float LastShootTime;
             LastShootTime = Time.time;

    void Fire() // 원형 발사
  {
      float currentTime = Time.time;
      while(currentTime - LastShootTime >= 0.01f)
      {
          GameObject Bullet = GameObject.Instantiate(SmallEnemyAttack);
          Transform Bullet_transform = Bullet.transform;
          EnemyBulletCtrl obj = Bullet.GetComponent<EnemyBulletCtrl>();

          Vector2 targetDirection = new Vector2(0.0f, 0.0f);
          Vector2 Curve = new Vector2(Mathf.Cos(angle), -Mathf.Sin(angle));

          targetDirection.x += Curve.x;
          targetDirection.y += Curve.y;

          Bullet_transform.localPosition = this.gameObject.transform.localPosition;
          Bullet_transform.localRotation = Quaternion.identity;
          Bullet_transform.localScale = Vector3.one;

          obj.direction = targetDirection;
          obj.bulletSpeed = speed;
          obj.MoveBullet(Time.deltaTime);

          LastShootTime += 0.009f;
          angle += 0.1f;

      } 

  } */
}
