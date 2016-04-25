using UnityEngine;
using System.Collections;

//싱글톤 패턴
//싱글턴 패턴
//단일체 패턴
//public sealed class Hello
//{
//    private static Hello instance;

//    public bool OnCollision;
//    public bool Foo;

//    public static Hello Instance
//    {
//        get
//        {
//            if (instance == null)
//                instance = new Hello();

//            return instance;
//        }
//    }

//    private Hello()
//    {
//    }

//}

public class test_managerscript : MonoBehaviour
{
    public static test_managerscript Instance = null;

    public bool OnCollision;
    public bool WaitTime;

    void Awake()
    {
        Instance = this;
    }

    //? Destroy할 때 Instance잘 초기화해주자
    //결자해지
    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void Start()
    {
        OnCollision = false;
        WaitTime = false;
    }
}
