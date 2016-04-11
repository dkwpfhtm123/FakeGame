using UnityEngine;
using System.Collections;

public class PlayerBullet : MonoBehaviour {
    public float Damage;
    public float Speed;

    Transform transformcachee;

	void Start () {

        transformcachee = GetComponent<Transform>();

        // GetComponent<Rigidbody>().AddForce(Vector2.up); // 부드러운 속도로 발사됨
    }
	
	void Update () {
        transformcachee.Translate(Vector2.up * Time.deltaTime * Speed , Space.Self); // 일정한 속도로 발사됨.
    }
}
