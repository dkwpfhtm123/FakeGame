using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMgr : MonoBehaviour
{
    public GameObject PlayerPowerObject;

    public GameObject SmallEnemyPrefab;
    public List<GameObject> SmallEnemyPool;
    public Transform PlayerTransform
    {
        set;
        get;
    }

    public bool BoomActive;
    public bool PlayerReviving;

    public static GameMgr Instance = null;

    public float PlayerScore;
    public float PlayerPower;

    private Transform transformCache;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SmallEnemyPool = new List<GameObject>();

        BoomActive = false;
        PlayerReviving = false;

        PlayerScore = 0.0f;
        PlayerPower = 0.0f;

        // 몬스터 풀 생성
        for (int i = 0; i < 10; i++)
        {
            GameObject smallEnemy = GameObject.Instantiate(SmallEnemyPrefab);
            smallEnemy.name = "SmallEnemy_" + i.ToString();
            smallEnemy.SetActive(false);
            SmallEnemyPool.Add(smallEnemy);
        }

        transformCache = GameObject.Find("LeftSpawnPoint").GetComponent<Transform>();
        //     StartCoroutine(EnemySpawn());
    }

    private IEnumerator EnemySpawn()
    {
        foreach (GameObject enemy in SmallEnemyPool)
        {
            yield return new WaitForSeconds(0.5f);

            if (enemy.activeSelf == false)
            {
                enemy.transform.position = transformCache.position;
                enemy.SetActive(true);
            }
        }
    }

    public void Boom()
    {
        StartCoroutine(BoomActiveChange());
    }

    private IEnumerator BoomActiveChange()
    {
        BoomActive = true;
        yield return new WaitForSeconds(2.0f); // 코루틴 : 여기서 StartCoroutine 으로 되돌아간 후 다음 명령 수행. 2초후 BoomActive = false 실행 / StopCoroutine 일 경우 BoomActive를 실행하지 않고 멈춤.
        BoomActive = false;
    }

    public void KillPlayer(GameObject player)
    {
        StartCoroutine(RevivePlayer(player));
        Destroy(player.gameObject);
        // 파워,체력을 잃어버린다 구현.
    }

    private IEnumerator RevivePlayer(GameObject playerObject)
    {
        GameObject playerPrefab = playerObject;
        GameObject newPlayer = GameObject.Instantiate(playerObject);
        newPlayer.transform.localPosition = new Vector3(0, -1.0f, 0);
        newPlayer.transform.localRotation = Quaternion.identity;

        PlayerTransform = newPlayer.transform;

        PlayerReviving = true;

        yield return new WaitForSeconds(2.0f);

        PlayerReviving = false;

        // 조금 움직인다 구현.

        /*    while (true)
            {
                newPlayer.transform.Translate(Vector3.up * Time.deltaTime , Space.Self);

                yield return new WaitForSeconds(2.0f);
            } */
    }

    public void ScoreChange()
    {
        PlayerScore += 100;
        Debug.Log("Score : " + PlayerScore);
    }

    public void PowerChange()
    {
        PlayerPower += 1;

        if (PlayerPower > 4)
        {
            PlayerPower = 4.0f;
            Debug.Log("Max Power");
        }
        else {
            Debug.Log("Power : " + PlayerPower);
            PowerUp(PlayerPower);
        }

        if (PlayerReviving == true)
        {
            PlayerPower = 0.0f;
        }
    }

    private float powerAngle = 0.0f;

    // 90도 추가 구현중 과부하걸림.
    public void PowerUp(float powerLevel)
    {
        GameObject playerPower = Instantiate(PlayerPowerObject);
        PlayerPowerUp powerCtrl = playerPower.GetComponent<PlayerPowerUp>();

        //     playerPower.transform.localPosition = transform.localPosition + Vector3.up;
        playerPower.transform.localRotation = Quaternion.identity;
        playerPower.transform.localScale = Vector3.one;

        powerCtrl.radius = 1.0f;
        powerCtrl.angle = (powerAngle + 90.0f) * (powerLevel - 1) * Mathf.Deg2Rad;
        powerCtrl.playerTransform = PlayerTransform;

        powerAngle = powerCtrl.angle;
    }
}
