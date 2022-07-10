using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed = 10.0f;
    public float fireRange = 300.0f;
    public float damage = 10.0f;

    private Transform tr;
    private Vector3 spawnPoint;

    private void Start()
    {
        tr = this.GetComponent<Transform>();
        spawnPoint = tr.position;
    }

    private void Update()
    {
        tr.Translate(Vector3.forward * Time.deltaTime * speed);

        if((spawnPoint-tr.position).sqrMagnitude > fireRange)
        {
            StartCoroutine(this.DestroyBullet());
        }
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
    }
}
