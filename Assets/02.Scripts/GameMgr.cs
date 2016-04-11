using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMgr : MonoBehaviour {

    public GameObject SmallEnemyPrefab;
    public List<GameObject> SmallEnemyPool;
    public Transform PlayerTransform;

    public bool BoomActive;
    public bool PlayerReviving;

    public static GameMgr Instance = null;

    public float PlayerScore;
    public float PlayerPower;

    private Transform transformCache;

    public void GetPlayerTransform(Transform spot)
    {
        PlayerTransform = spot;
    }

    void Awake()
    {
        Instance = this;
    }

    public void BoomEvent()
    {
        StartCoroutine(Boom());
        StopCoroutine(Boom());
    }

    IEnumerator Boom()
    {
        BoomActive = true;
        yield return new WaitForSeconds(2.0f);
        BoomActive = false;
    }

    void Start () {
        SmallEnemyPool = new List<GameObject>();

        BoomActive = false;
        PlayerReviving = false;

        PlayerScore = 0.0f;
        PlayerPower = 0.0f;

        //@ Transform 조정하기
        //@ Player가 능동적으로 설정하기

        // 플레이어의 Transform 확인
        //     PlayerTransform = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();


        // 몬스터 풀 생성
        for (int i = 0; i < 10; i++)
        {
            GameObject SmallEnemy = GameObject.Instantiate(SmallEnemyPrefab);
            SmallEnemy.name = "SmallEnemy_" + i.ToString();
            SmallEnemy.SetActive(false);
            SmallEnemyPool.Add(SmallEnemy);
        } 

        transformCache = GameObject.Find("LeftSpawnPoint").GetComponent<Transform>();
   //     StartCoroutine(EnemySpawn());
   //     StopCoroutine(EnemySpawn());

    }

    IEnumerator EnemySpawn()
    {
        foreach (GameObject Enemy in SmallEnemyPool)
        {
            yield return new WaitForSeconds(0.5f);

            //@ 원칙과 소신
            if (Enemy.activeSelf == false)
            {
                Enemy.transform.position = transformCache.position;
                Enemy.SetActive(true);
            }
        }
    }

    public void PlayerDie(GameObject player)
    {
        StartCoroutine(RevivePlayer(player));
        Destroy(player.gameObject);
        // 파워,체력을 잃어버린다 구현.
    }

    IEnumerator RevivePlayer(GameObject playerObject)
    {
        GameObject PlayerPrefab = playerObject;
        GameObject newPlayer = GameObject.Instantiate(playerObject);
        newPlayer.transform.localPosition = new Vector3(0, -1.0f, 0);
        newPlayer.transform.localRotation = Quaternion.identity;

        GetPlayerTransform(newPlayer.transform);

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


    public void GetScore()
    {
        PlayerScore += 100;
        Debug.Log("Score : " + PlayerScore);
    }

    public void GetPower()
    {
        PlayerPower += 1;
        Debug.Log("Power : " + PlayerPower);
    }
}
