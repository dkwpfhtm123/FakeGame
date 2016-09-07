
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMgr : MonoBehaviour
{
    private static GameMgr instance;
    public static GameMgr Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameMgr>();

            return instance;
        }
    }

    public GameObject SmallEnemyPrefab;
    public List<GameObject> SmallEnemyPool;
    public Transform PlayerTransform
    {
        get;
        set;
    }

    public int PlayerScore;

    private Transform transformCache;

    void Start()
    {
        SmallEnemyPool = new List<GameObject>();

        PlayerScore = 0;
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
