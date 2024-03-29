using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
/// <summary>
/// Credit신을 관리하는 스크립트
/// </summary>
/// <remarks>
/// 플레이어의 움직임, 물체와의 상호작용, 다음 신으로 넘어가능 기능을 포함합니다.
/// </remarks>
/// @date last change 2023/05/15
/// @author MW
///
public class CreditManager : MonoBehaviourPunCallbacks
{
    #region Field
    private static CreditManager instance;

    public GameObject player;
    Start_Player move;

    [SerializeField] GameObject door;

    #endregion

    #region Property
    public static CreditManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CreditManager>();
            }
            return instance;
        }
    }

    #endregion
    #region Methods
    void UpdateWork()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    /**
     * @brief 플레이어를 포톤에서 연결 해제시키고 Start 씬으로 돌려보냅니다.
     */
    public void Load()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Game_Start");
    }

    public void DoorActive()
    {
        door.SetActive(true);
    }

    #endregion



    #region Unity Events
    private void Start()
    {
        move = player.GetComponent<Start_Player>();
        move.enabled = true;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        UpdateManager.SubscribeToUpdate(UpdateWork);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        UpdateManager.UnsubscribeFromUpdate(UpdateWork);
    }
    #endregion
}
