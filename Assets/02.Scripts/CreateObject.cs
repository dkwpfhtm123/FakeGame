using UnityEngine;
using System.Collections;

public class CreateObject : MonoBehaviour
{
    public GameObject PlayerObject;
    public GameObject Boss;
    public GameObject EnemyObject;

    private Vector2 PlayerSpawnPoint;

    private Transform transformCache;

    private Vector2 leftUp;
    private Vector2 leftDown;
    private Vector2 rightUp;
    private Vector2 rightDown;

    private float radius;

    void Start()
    {
        radius = 5.0f;
        transformCache = GetComponent<Transform>();

        leftUp = new Vector2(transformCache.localPosition.x - radius, transformCache.localPosition.y + radius);
        leftDown = new Vector2(leftUp.x, -leftUp.y);
        rightUp = new Vector2(-leftUp.x, leftUp.y);
        rightDown = new Vector2(-leftUp.x, -leftUp.y);

        PlayerSpawnPoint = new Vector2(transformCache.localPosition.x, transformCache.localPosition.y - radius * 0.5f);

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        CreatePlayer();

        yield return new WaitForSeconds(3.0f); // 대기시간 이후 적생성
    }

    public void CreatePlayer()
    {
        GameObject playerObject = Instantiate(PlayerObject);
        Player.PlayerCtrl set = playerObject.GetComponent<Player.PlayerCtrl>();

        playerObject.transform.localPosition = new Vector2(-3, -3);
        playerObject.transform.localRotation = Quaternion.identity;
        playerObject.transform.localScale = Vector2.one;

        set.Setup(3, 1, 3); // life, power, boom

        GameMgr.Instance.PlayerTransform = playerObject.transform;
        Enemy.AttackKinds.Instance.CheckBoom = set;
    }
}
