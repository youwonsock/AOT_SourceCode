using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
/// <summary>
/// 플레이어가 닿을 경우 두 플레이어를 Ending신으로 옮기기 위한 스크립트
/// </summary>
/// /// @date last change 2023/05/19
/// @author MW
/// 
public class Stage2toEnding : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject endinginfo;
    EndingInfo edio;

    void Start()
    {
        edio = endinginfo.GetComponent<EndingInfo>();
        GameManager.Instance.goal = 0;
    }

    [PunRPC]
    public void SetEndingSelect(int i)
    {
        switch(i)
        {
            case 1:
                edio.End_select = 1;
                break;
            case 2:
                edio.End_select = 2;
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine && PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SetEndingSelect", RpcTarget.All, 1);
                GameManager.Instance.goal += 1;
            }
            else if (other.gameObject.CompareTag("Player") && !other.GetComponent<PhotonView>().IsMine && PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("SetEndingSelect", RpcTarget.All, 2);
                GameManager.Instance.goal += 1;
            }
        }
    }


}
