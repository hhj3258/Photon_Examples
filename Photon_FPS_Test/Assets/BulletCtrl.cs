using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 10.0f;
    public float fireRange = 300.0f;
    public float damage = 10.0f;

    public Player Owner { get; private set; }

    private void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    private void Update()
    {
        Debug.Log("pos: {0}" + transform.position);
        Debug.Log("vel:" + GetComponent<Rigidbody>().velocity);
    }

    // 불릿 초기화
    // lag = time lag. 서버 시간 - RPC 발신자의 발신 당시의 서버 시간
    public void InitializeBullet(Player owner, Vector3 originalDirection, float lag)
    {
        Owner = owner;

        transform.forward = originalDirection;

        Rigidbody rigidbody = GetComponent<Rigidbody>();

        // 속도 = 방향 * 속력
        rigidbody.velocity = originalDirection * 100.0f;
        // 초기 총알 생성 위치 += 속도 * 타임랙
        rigidbody.position += rigidbody.velocity * lag;
    }

    IEnumerator DestroyBullet()
    {
        Destroy(this.gameObject);
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        
        Debug.Log("OnTriggerEnter:"+other.name);
        other.gameObject.GetComponent<PlayerScript>().HP -= damage;

        Destroy(gameObject);
    }
}
