using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
/// <summary>
/// UI 관리 클래스
/// </summary>
/// <remarks>
/// 기존 흩어져 오브젝트에 각각 붙어있던 UI의 스크립트를 하나로 모아서 재정의했습니다.
/// 
/// </remarks>
/// @date last change 2023/05/08
/// @author MW
/// @class UI

public class UIManager : MonoBehaviourPunCallbacks
{
    /**
     * @param sysmsg,systxt 다른 플레이어가 나갔을 경우 메세지를 띄우는 오브젝트
     * @param pvimage 1인칭과 3인칭 카메라 전환에 따른 UI 표시 이미지
     * @param pause_ui 일시정지 UI
     * @param setting_ui 설정 UI
     */
    #region Fields
    private static UIManager instance;

    [SerializeField] GameObject sysmsg;
    [SerializeField] GameObject systxt;
    [SerializeField] Image pvimage;

    [SerializeField] GameObject pause_ui;
    [SerializeField] GameObject setting_ui;
    [SerializeField] Sprite change_to_tpv;
    [SerializeField] Sprite change_to_fpv;
    #endregion

    #region Property
    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    
    public GameObject PauseUI{ get { return pause_ui; } }
    public GameObject SettingUI { get { return setting_ui; } }
    #endregion

    #region Methods


    public void SettingBtnClicked()
    {
        setting_ui.SetActive(true);
    }

    /// <summary>
    /// 메인메뉴로 이동하는 버튼 클릭 이벤트입니다.
    /// </summary>
    public void MainMenuBtnClicked()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length > 1)
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[1]);
        }

        LeaveRoom();
    }

    /// <summary>
    /// 게임 종료 버튼 클릭 이벤트입니다.
    /// </summary>
    public void QuitBtnClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

    /// <summary>
    /// 일시정지를 해제하고 게임으로 돌아가는 키입력 이벤트 입니다.
    /// </summary>
    public void ExitUI()
    {
        pause_ui.SetActive(false);
    }

    /// <summary>
    /// 게임을 일시정지하는 키입력 이벤트 입니다.
    /// </summary>
    public void PauseUICall()
    { 
        pause_ui.SetActive(true);
    }

    public void TpvModeImage()
    {
        pvimage.sprite = change_to_fpv;
    }

    public void FpvModeImage()
    {
        pvimage.sprite = change_to_tpv;
    }
    #endregion

    #region Photon
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        OnJoinedLobby();
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("Game_Lobby");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /**
     * @brief 상대 플레이어가 방을 나갔을 때, UI 좌하단에 상대 플레이어가 떠났음을 알리는 메세지를 띄우는 메소드
     */
    public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
    {
        base.OnPlayerLeftRoom(other);
        sysmsg.SetActive(true);

        systxt.GetComponent<Text>().text = $"{other.NickName} Leave Game.";
        Destroy(sysmsg, 2.5f);

    }
    #endregion
    #region Unity Events

    #endregion
}
