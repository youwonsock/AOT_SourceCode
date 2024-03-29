using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
/// <summary>
/// 룸에 대한 설정들을 맡습니다
/// </summary>
/// <remarks>
/// 멀티플레이를 위한 방을 열거나 방에 들어갈 수 있도록 해주는 스크립트입니다.
/// </remarks>
/// 
/// @date last change 2023/05/27
/// @author LSM
/// @class CreateAndJoin
public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;

    /// <summary>
    /// 방을 만드는 함수입니다
    /// </summary>
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    /// <summary>
    /// 방에 참가하는 함수입니다
    /// </summary>
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    /// <summary>
    /// 씬 전환 함수입니다
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel("Game_Waiting_Room");
    }
}
