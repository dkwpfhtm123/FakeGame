using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour
{
    // Boom관련 변수들
    public GameObject BoomPrefab;    // 프리팹
    public int BoomCount = 3;        // 남은 갯수
    private float boomTime = 2.0f;   // 지속시간
    private GameObject boomObject;   // 게임오브젝트 저장
    private bool boomCheck = false;  // 폭탄이 켜져있는지 체크

    private Transform transformCache;
    private float moveSpeed = 5.0f;

    private ItemTypeScript item = null;

    void Start()
    {
        transformCache = GetComponent<Transform>();

        GameMgr.Instance.PlayerTransform = transformCache;
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // 부활하는 중이 아닐때
        // 약간 움직이는거 수정할것 - GameMgr부분
        if (GameMgr.Instance.PlayerReviving == false)
        {

            Vector2 moveDir = (Vector2.up * vertical) + (Vector2.right * horizontal);

            if (Input.GetKeyDown(KeyCode.X))
            {
                // 폭탄이 활성화되지 않았을때
                if (boomCheck == false)
                {
                    boomCheck = true;
                    StartCoroutine(Boom());
                }
            }

            // 쉬프트 눌렀을때 느려짐 빨라짐
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                moveSpeed /= 2.0f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                moveSpeed *= 2.0f;
            }

            // 폭탄이 있을때 폭탄이 플레이어와 같이 움직임.
            if (boomObject != null)
            {
                boomObject.transform.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);
            }

            transformCache.Translate(moveDir * Time.deltaTime * moveSpeed, Space.Self);
        }
    }

    private IEnumerator Boom()
    {
        boomObject = (GameObject)Instantiate(BoomPrefab, this.transform.position, Quaternion.identity);
        GameMgr.Instance.Boom();
        yield return new WaitForSeconds(boomTime);
        Destroy(boomObject);
        boomCheck = false;
        BoomCount--;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        if (coll.gameObject.GetComponent<ItemTypeScript>())
        {
            item = coll.gameObject.GetComponent<ItemTypeScript>();

            if (item.ItemTypeCheck == ItemTypeScript.ItemTy.ScoreItem)
            {
                GameMgr.Instance.ScoreChange();
                Destroy(coll.gameObject);
            }

            else if (item.ItemTypeCheck == ItemTypeScript.ItemTy.PowerItem)
            {
                GameMgr.Instance.PowerChange();
                Destroy(coll.gameObject);
            }
        }

        if (GameMgr.Instance.PlayerReviving == false)
        {
            if (coll.gameObject.GetComponent<ThisIsEnemyBullet>()) // 적 탄을 구별하기위해 하나는 남겨둠.
            {
                GameMgr.Instance.KillPlayer(gameObject);
            }
        }
    }
}
