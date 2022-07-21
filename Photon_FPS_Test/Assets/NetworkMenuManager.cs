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
        //모든 클라이언트가 씬을 동기화 시키도록 설정
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // 포톤 서버 접속 함수
    public void OnClickConnect()
    {
        // 만약 연결이 안되어 있다면
        if (!PhotonNetwork.IsConnected)
        {
            // 포톤 서버로 연결해라
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            // 서버와 연결이 되어있을 때의 처리를 여기서 함
        }
    }

    private void Update()
    {
        // 서버 연결 과정을 출력해봄
        txtConnectionInfo.text = PhotonNetwork.NetworkClientState.ToString();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnDisconnected: " + cause);
    }

    // 마스터서버 접속을 성공했을 때 콜백
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
