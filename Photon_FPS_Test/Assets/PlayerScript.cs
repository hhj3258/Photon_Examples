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

    public Transform trGun;
    public GameObject bullet;
    private Transform tr;
    public Transform CameraPivot;
    public Slider hpBar;

    private void Start()
    {
        if (!photonView.IsMine)
            return;

        tr = GetComponent<Transform>();
        Camera.main.GetComponent<CameraMove>().target = CameraPivot;

        hpBar = GameObject.FindWithTag("HP Bar").GetComponent<Slider>();
        curHp = maxHp;
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        tr.Translate(Vector3.forward * v * Time.deltaTime * speed);
        tr.Rotate(Vector3.up * h * Time.deltaTime * rotSpeed);

        bool fire = Input.GetMouseButtonDown(0);
        if (fire)
        {
            Shooting();
        }
    }

    private void Shooting()
    {
        Vector3 bulletPosition = trGun.position + trGun.forward * bulletOffset;
        PhotonNetwork.Instantiate("bullet", bulletPosition, trGun.rotation);
    }

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
            Debug.Log("hpbar°¡ ¾øÀ½");
            return;
        }

        hpBar.value = curHp / maxHp;

        if (curHp <= 0.0f)
        {
            PhotonNetwork.Disconnect();
        }
    }
}
