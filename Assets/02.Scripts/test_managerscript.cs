using UnityEngine;
using System.Collections;

public class test_managerscript : MonoBehaviour
{
    public static test_managerscript Instance = null;

    public bool oncollision;
    public bool waitTime;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        oncollision = false;
        waitTime = false;
    }
}
