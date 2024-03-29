using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 게임 시작씬에서 모든 처리를 담당하고 로딩 씬으로 넘겨주는 역할
/// </summary>
/// <remarks>
/// 닉네임을 설정하고 게임 로딩씬으로 넘어가는 역할을 맡은 스크립트입니다.
/// 이외의 모든 처리를 담당했습니다
/// </remarks>
public class Scene_Load : MonoBehaviourPunCallbacks
{
    public InputField Nickname;
    public GameObject player;
    [SerializeField] Text warning;
    Start_Player move;

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

    private void Start()
    {
        move = player.GetComponent<Start_Player>();
    }

    void UpdateWork()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    /// <summary>
    /// 입력이 시작되면 플레이어 비활성화
    /// </summary>
    public void input()
    {
        move.enabled = false;
    }
    /// <summary>
    /// 입력이 끝나면 플레이어 활성화
    /// </summary>
    public void endInput()
    {
        move.enabled = true;
    }
    /// <summary>
    /// 닉네임을 적고 버튼을 누르면 이 함수가 실행되며 로딩씬으로 넘어갑니다.
    /// </summary>
    public void Load()
    {
        bool pass = true;

        PhotonNetwork.LocalPlayer.NickName = Nickname.text;

        for(int i=0; i<Nickname.text.Length; i++)
        {
            if (Nickname.text[i] == ' ')
            {
                warning.text = "닉네임에 공백이 포함되어 있으면 안 됩니다";
                pass = false;
            }   
        }

        if (pass && Nickname.text != "")
            SceneManager.LoadScene("Game_Loading");
        else if (pass)
            warning.text = "닉네임은 최소 한 글자 이상이어야 합니다";
           
    }
}
