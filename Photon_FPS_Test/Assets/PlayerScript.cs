using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviourPun
{
    public float speed = 5.0f;
    public float rotSpeed = 120.0f;
    private float curHp;
    public float maxHp = 100.0f;
    public float bulletOffset = 0.0f;

    public Transform gunOffset;
    public GameObject BulletPrefab;
    private Transform tr;
    public Transform CameraPivot;
    public Slider hpBar;

    private new Rigidbody rigidbody;

    private void Start()
    {
        if (!photonView.IsMine)
            return;

        tr = GetComponent<Transform>();
        Camera.main.GetComponent<CameraMove>().target = CameraPivot;

        hpBar = GameObject.FindWithTag("HP Bar").GetComponent<Slider>();
        curHp = maxHp;

        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        tr.Translate(Vector3.forward * v * Time.deltaTime * speed);
        tr.Rotate(Vector3.up * h * Time.deltaTime * rotSpeed);

        bool fire = Input.GetMouseButton(0);
        if (fire)
        {
            // RpcTarget.AllViaServer = RPC ȣ���� �������� ��û�ϰ� ������ '��'�� ������ ��� Ŭ���̾�Ʈ���� '����'��� ���ش�
            photonView.RPC("Fire", RpcTarget.AllViaServer, rigidbody.position, rigidbody.rotation);
        }
    }

    [PunRPC]
    public void Fire(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);
        GameObject bullet;
        
        /** Use this if you want to fire one bullet at a time **/
        // �Ҹ� ����
        bullet = Instantiate(BulletPrefab, gunOffset.position, Quaternion.identity) as GameObject;
        // �Ҹ� �ʱ�ȭ ����
        bullet.GetComponent<BulletCtrl>().InitializeBullet(photonView.Owner, (rotation * Vector3.forward), Mathf.Abs(lag));

        /** �� ���� �� ���� �Ѿ��� �߻��ϰ� ���� �� �߰� **/
        //Vector3 baseX = rotation * Vector3.right;
        //Vector3 baseZ = rotation * Vector3.forward;

        //Vector3 offsetLeft = -1.5f * baseX - 0.5f * baseZ;
        //Vector3 offsetRight = 1.5f * baseX - 0.5f * baseZ;

        //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetLeft, Quaternion.identity) as GameObject;
        //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
        //bullet = Instantiate(BulletPrefab, rigidbody.position + offsetRight, Quaternion.identity) as GameObject;
        //bullet.GetComponent<Bullet>().InitializeBullet(photonView.Owner, baseZ, Mathf.Abs(lag));
    }

    //private void Shooting()
    //{
    //    Vector3 bulletPosition = gunOffset.position + gunOffset.forward * bulletOffset;
    //    PhotonNetwork.Instantiate("bullet", bulletPosition, gunOffset.rotation);
    //}

    public float HP
    {
        get { return curHp; }
        set
        {
            curHp = value;
            SetHpBar();
        }
    }

    public void SetHpBar()
    {
        if (!hpBar)
        {
            Debug.Log("hpbar�� ����");
            return;
        }

        hpBar.value = curHp / maxHp;

        if (curHp <= 0.0f)
        {
            PhotonNetwork.Disconnect();
        }
    }
}
