using UnityEngine;
using System.Collections;

public class LSManager : MonoBehaviour
{
    private static LSManager instance;
    public static LSManager Instance
    {
        get
        {
            //@ 코드 정리
            //@ <> 사용해보기
            if (instance == null)
                instance = FindObjectOfType<LSManager>();
            
            return instance;
        }
    }

    public bool OnCollision;
    public bool WaitTime;

    void Start()
    {
        OnCollision = false;
        WaitTime = false;
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
