using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 게임을 시작하기 전 대기방에서 게임을 준비하고 시작하는 처리를 맡은 스크립트
/// </summary>
/// 
/// @date last change 2023/05/27
/// @author LSM
/// @class Waiting_Room
public class Waiting_Room : MonoBehaviourPunCallbacks
{
    [SerializeField] Button StartGameButton;
    [SerializeField] GameObject ReadyGameButton;
    [SerializeField] GameObject StageSelection;
    [SerializeField] Button[] Stages = new Button[3];
    [SerializeField] Text playerStr;
    [SerializeField] Text Information;
    
    bool IsReady;
    

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
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

    void Start()
    {
        Hashtable props = new Hashtable
        {
            {"IsPlayerReady", false}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        ReadyGameButton.gameObject.SetActive(!PhotonNetwork.IsMasterClient);
    }

    // Update is called once per frame
    void UpdateWork()
    {
        if (PhotonNetwork.InRoom)
        {
            string playerName = "현재 방에 접속한 플레이어 : ";
            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                playerName += p.NickName + " ";
            }
            playerStr.text = playerName;
        }
        
        StartGameButton.interactable = CheckPlayersReady();
        if (CheckPlayersReady())
        {
            Information.text = "모든 플레이어가 준비되었습니다.\n게임을 시작할 수 있습니다.";
            for (int i = 0; i < Stages.Length; i++)
            {
                if (i == 0 && PlayerPrefs.GetFloat("Tutorial") == 0)
                {
                    //Stages[i].interactable = false;
                }
                else if (PlayerPrefs.GetFloat("Stage" + i + "Clear") == 0)
                {
                    //Stages[i].interactable = false;
                }
            }
        }
        else if(PhotonNetwork.IsMasterClient)
        {
            Information.text = "아직 모든 플레이어가 준비되지 않았습니다.\n게임을 시작할 수 없습니다.";
        }
        else
        {
            Information.text = "";
        }
    }

    // 방을 떠나는 함수
    public void OnLeaveGameButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length > 1)
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[1]);
        }
        PhotonNetwork.LeaveRoom();
    }

    // 튜토리얼 씬으로 옮겨주고 이 방에 다른 플레이어가 들어오지 못하도록 하는 함수
    public void OnStartGameButtonClicked()
    {
        StageSelection.SetActive(true);
    }

    // 방을 떠났을 때 씬을 옮기고 방장을 인계하는 작업을 해주는 함수
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        OnJoinedLobby();
        SceneManager.LoadScene("Game_Lobby"); 
    }

    public void OnReadyGameButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            return;
        }
        else
        {
            if (IsReady)
            {
                IsReady = false;
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { "IsPlayerReady", false } });
            }
            else
            {
                IsReady = true;
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { "IsPlayerReady", true } });
            }
        }
    }

    // 방장이 모든 플레이어가 레디했는지 확인하는 함수
    private bool CheckPlayersReady()
    {
        int count = 0;

        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        for(int i=1; i<PhotonNetwork.PlayerList.Length; i++)
        {
            object isPlayerReady;
            
            if (PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue("IsPlayerReady", out isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                {
                    return false;
                }
                else
                {
                    count += 1;
                }
            }
            else
            {
                return false;
            }
        }

        if (count >= 0)
            return true;
        else
            return false;
    }

    public void TutorialClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        //PhotonNetwork.LoadLevel("Demo Stage_v1");
        PhotonNetwork.LoadLevel("Tutorial");
    }

    public void Stage1Clicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("Stage_1");
    }

    public void Stage2Clicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("Stage_2");
    }

    public void Stage3Clicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        //PhotonNetwork.LoadLevel("Stage_3");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        ReadyGameButton.gameObject.SetActive(!PhotonNetwork.IsMasterClient);
    }
}
