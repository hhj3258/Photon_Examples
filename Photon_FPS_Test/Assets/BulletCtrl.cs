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

    // �Ҹ� �ʱ�ȭ
    // lag = time lag. ���� �ð� - RPC �߽����� �߽� ����� ���� �ð�
    public void InitializeBullet(Player owner, Vector3 originalDirection, float lag)
    {
        Owner = owner;

        transform.forward = originalDirection;

        Rigidbody rigidbody = GetComponent<Rigidbody>();

        // �ӵ� = ���� * �ӷ�
        rigidbody.velocity = originalDirection * 100.0f;
        // �ʱ� �Ѿ� ���� ��ġ += �ӵ� * Ÿ�ӷ�
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
