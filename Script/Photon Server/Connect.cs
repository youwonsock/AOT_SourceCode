using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

/// <summary>
/// 로딩 화면에서 서버 접속을 담당하는 스크립트
/// </summary>
/// <remarks>
/// 서버 접속을 시도하는 역할을 맡습니다
/// </remarks>
/// @date last change 2023/05/27
/// @author LSM
/// @class Connect
public class Connect : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Start함수에서 서버 접속을 시도합니다
    /// </summary>
    void Start()
    {
        ConnectToServer();
        // 서버 접속 시도
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    /// <summary>
    /// 서버 접속에 성공하면 로비씬으로 이동합니다
    /// </summary>
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        SceneManager.LoadScene("Game_Lobby");
    }

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// 서버 접속에 실패하면 접속에 실패한 이유를 로그로 띄우고 다시 서버 접속을 시도합니다
    /// </summary>
    /// <param name="cause"></param>
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("연결 끊김");
        Debug.Log(cause);
        ConnectToServer();
    }
}