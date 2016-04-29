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
    // 만약 monobehavior을 상속받지 않는다면 언제 onDestory가 호출되는지 질문.
    private static test_manager_1 instance;
    public static test_manager_1 Instance
    {
        get
        {
            if(instance == null)
                instance = FindObjectOfType(typeof(test_manager_1)) as test_manager_1;
            
            return instance;
        }
        private set
        {
            instance = value;
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
