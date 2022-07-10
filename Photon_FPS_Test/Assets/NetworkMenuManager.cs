using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class NetworkMenuManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI txtConnectionInfo;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnClickConnect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {

        }
    }

    private void Update()
    {
        txtConnectionInfo.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected: " + cause);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed");
        CreateRoom();
    }

    private void CreateRoom()
    {
        string roomName = "Room" + UnityEngine.Random.Range(0, 100);
        PhotonNetwork.CreateRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoined");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreate");
        PhotonNetwork.LoadLevel("SC_InGame");
    }

    
}
