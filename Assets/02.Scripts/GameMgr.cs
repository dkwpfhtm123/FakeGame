using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMgr : MonoBehaviour
{
    public static GameMgr Instance = null;

    public GameObject PlayerPowerObject;

    public GameObject SmallEnemyPrefab;
    public List<GameObject> SmallEnemyPool;
    public Transform PlayerTransform
    {
        set;
        get;
    }

    public bool OnGoingBoom
    {
        private set;
        get;
    }
    public bool RespawnPlayer
    {
        private set;
        get;
    }

    public float PlayerScore;
    public float PlayerPower;

    private Transform transformCache;
    private PlayerPowerUp powerUp = null; // 임시변수

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SmallEnemyPool = new List<GameObject>();

        OnGoingBoom = false;
        RespawnPlayer = false;

        PlayerScore = 0.0f;
        PlayerPower = 0.0f;

        for (int i = 0; i < 10; i++)         // 몬스터 풀 생성
        {
            GameObject smallEnemy = GameObject.Instantiate(SmallEnemyPrefab);
            smallEnemy.name = "SmallEnemy_" + i.ToString();
            smallEnemy.SetActive(false);
            SmallEnemyPool.Add(smallEnemy);
        }

        transformCache = GameObject.Find("LeftSpawnPoint").GetComponent<Transform>();
        //     StartCoroutine(EnemySpawn()); 임시 비활성화
    }

    private IEnumerator SpawnEnemy()
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
        StartCoroutine(StartBoom());
    }

    private IEnumerator StartBoom()
    {
        OnGoingBoom = true;
        yield return new WaitForSeconds(2.0f); // 코루틴 : 여기서 StartCoroutine 으로 되돌아간 후 다음 명령 수행. 2초후 BoomActive = false 실행 / StopCoroutine 일 경우 BoomActive를 실행하지 않고 멈춤.
        OnGoingBoom = false;
    }

    public void KillPlayer(GameObject player)
    {
        StartCoroutine(CreatePlayer(player));
        Destroy(player.gameObject);
        // 파워,체력을 잃어버린다 구현 예정.
    }

    private IEnumerator CreatePlayer(GameObject playerObject)
    {
        GameObject newPlayer = GameObject.Instantiate(playerObject);
        newPlayer.transform.localPosition = new Vector3(0, -1.0f, 0);
        newPlayer.transform.localRotation = Quaternion.identity;

        PlayerTransform = newPlayer.transform;

        RespawnPlayer = true;

        for (float i = 0; i < 2.0f; i += Time.deltaTime) // 위로 조금씩 올라간다 수정예정.
        {
            newPlayer.transform.Translate(Vector3.up * Time.deltaTime, Space.Self);
        }
        yield return new WaitForSeconds(2.0f);

        RespawnPlayer = false;
    }

    public void AddPlayerScore()
    {
        PlayerScore += 100;
        Debug.Log("Score : " + PlayerScore);
    }

    public void AddPlayerPower()
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

        if (RespawnPlayer == true)
        {
            PlayerPower = 0.0f;
        }
    }

    public void PowerUp(float powerLevel)
    {
        GameObject playerPower = Instantiate(PlayerPowerObject);
        PlayerPowerUp powerCtrl = playerPower.GetComponent<PlayerPowerUp>();

        playerPower.transform.localPosition = transform.localPosition;
        playerPower.transform.localRotation = Quaternion.identity;
        playerPower.transform.localScale = Vector3.one;

        powerCtrl.radius = 1.0f;
        if (powerUp != null)
        {
            powerCtrl.angle = (powerUp.angle + 90.0f);
        }
        else
        {
            powerCtrl.angle = 0;
        }
        powerCtrl.StartRotatePower(PlayerTransform);

        powerUp = powerCtrl;
    }
}
