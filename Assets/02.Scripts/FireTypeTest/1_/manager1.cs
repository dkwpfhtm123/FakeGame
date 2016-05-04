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

public class test_manager_1 : MonoBehaviour
{
    private static test_manager_1 instance;
    public static test_manager_1 Instance
    {
        get
        {
            //@ 코드 정리
            //@ <> 사용해보기
            if(instance == null)
                instance = FindObjectOfType(typeof(test_manager_1)) as test_manager_1;
            
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
