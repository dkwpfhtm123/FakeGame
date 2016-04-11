using UnityEngine;
using System.Collections;

public class ItemMove : MonoBehaviour {
    public float ScoreSpeed = 4.0f;
    public float PowerSpeed = 2.0f;

    private Transform transformCache;

    //@ modifier
    Vector2 direction;
    ItemTypeScript item;
    
    void Start()
    {
        item = gameObject.GetComponent<ItemTypeScript>();
    }

    // 폭탄쓸때 모든 탄이 스코어로 전환되어 플레이어한테 가는것으로 설정되어잇음.
    void Update()
    {

        if (item.ItemTypeCheck == ItemTypeScript.ItemTy.ScoreItem)
        {
            MoveScoreItem(Time.deltaTime);
            if (ScoreSpeed < 20.0f)
            {
                SpeedUp();
            }
        }

        if(item.ItemTypeCheck == ItemTypeScript.ItemTy.PowerItem)
        {
            MovePowerItem();
        }
    }

    void SpeedUp()
    {
        //      float Up = Random.Range(1.0f, 1.5f);
        ScoreSpeed *= 1.1f;
    }

    void MoveScoreItem(float deltaTime)
    {
        transformCache = GetComponent<Transform>();
        
        Transform playerTransform = GameMgr.Instance.PlayerTransform;
        Vector2 target = (playerTransform.localPosition - transformCache.localPosition).normalized;

        Vector2 position = transformCache.localPosition;
        position.x += target.x * ScoreSpeed * deltaTime;
        position.y += target.y * ScoreSpeed * deltaTime;
        
        transformCache.localPosition = position;
    } 

    void MovePowerItem()
    {
        StartCoroutine(MovePosition());
    }

    IEnumerator MovePosition()
    {
        transformCache = GetComponent<Transform>();
        Vector2 position = transformCache.localPosition;

        position.y += PowerSpeed * Time.deltaTime;
        transformCache.localPosition = position;

        yield return new WaitForSeconds(0.2f);

        CheckPowerSpeed();
    }

    //@ 함수 이름
    void CheckPowerSpeed()
    {
        if (PowerSpeed > 0.0f)
        {
            PowerSpeed -= 0.1f;
        }

        else if (PowerSpeed < 0.01f)
        {
            PowerSpeed -= 0.02f;
        }

        else if (PowerSpeed < -1.5f)
        {
            PowerSpeed = -1.5f;
        }
    }
}
