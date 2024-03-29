using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 간편 멀티테스트 환경
/// </summary>
/// <remarks>
/// 작은 단위의 멀티테스트와 디버깅을 돕는 환경이며, 실제 통합플로우를 하나의 씬으로 축약한 형태
/// </remarks>
/// @author 이은수 
/// @date last change 2023/02/23
public class EasyPhotonTest : MonoBehaviourPunCallbacks
{
    #region fields

    public InputField nickInput, roomInput; 
    [SerializeField, Tooltip("테스트씬의 빌드 넘버")] int sceneNum = 1; 

    #endregion

    #region methods

    /// <summary>
    /// 닉네임과 커스텀프로퍼티를 설정하고, 서버 접속 시도
    /// </summary>
    public void Load()
    {
        PhotonNetwork.LocalPlayer.NickName = nickInput.text; 
        Connect(); 
    }

    /// <summary>
    /// 포톤 온라인 서버 연결 함수
    /// </summary>
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings(); 
    }
    /// <summary>
    /// 포톤 온라인 서버 해제 함수
    /// </summary>
    public void Disconnect()
    {
        PhotonNetwork.Disconnect(); 
    }

    #endregion

    #region photon callbacks

    /// <summary>
    /// 마스터 서버에 연결되었을 때, 로비 진입 시도하는 콜백 함수
    /// </summary>
    public override void OnConnectedToMaster() 
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby(); 
    }
    /// <summary>
    /// 마스터 서버의 로비에 진입했을 때, 룸 생성 및 참가하게 하는 콜백 함수
    /// </summary>
    public override void OnJoinedLobby() 
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinOrCreateRoom(roomInput.text + "test room", new RoomOptions { MaxPlayers = 2 , PublishUserId = true }, null); 
    }
    /// <summary>
    /// 룸에 들어왔을 때, 테스트 씬을 로드하는 콜백 함수
    /// </summary>
    public override void OnJoinedRoom() 
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(sceneNum);
    }
    /// <summary>
    /// 서버 접속에 실패했을 때, 서버 접속을 재시도하는 콜백 함수
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause) 
    {
        base.OnDisconnected(cause);
        Debug.Log(cause);
        Connect(); 
    }

    #endregion

    #region unity events

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // auto synchronization of scene
    }
    void UpdateWork()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        UpdateManager.SubscribeToUpdate(UpdateWork);
    }

    #endregion
}
