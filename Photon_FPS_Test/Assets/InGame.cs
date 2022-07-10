using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using System;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class InGame : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI txtRoomName;
    public Transform[] spawnPoints;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        txtRoomName.text = PhotonNetwork.CurrentRoom.Name;

        OnJoinPlayer();
    }

    private void OnJoinPlayer()
    {
        int index = UnityEngine.Random.Range(0, spawnPoints.Length);
        PhotonNetwork.Instantiate("Player", spawnPoints[index].position, spawnPoints[index].rotation);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("SC_Main");
    }
}
