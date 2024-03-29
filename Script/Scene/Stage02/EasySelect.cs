using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// 이지 스테이지 선택시 활성화되는 스크립트
/// </summary>
/// 
/// @date last change 2023/05/27
/// @author LSM
/// @class EasySelect
public class EasySelect : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject Block;
    [SerializeField] GameObject EasyTrigger;
    [SerializeField] GameObject HardStage;
    [SerializeField] GameObject G;

    private void OnTriggerEnter(Collider other)
    {
        photonView.RPC("easy", RpcTarget.MasterClient, other.CompareTag("Player"));
        if (other.GetComponent<PhotonView>().IsMine)
        {
            HardStage.SetActive(false);
            G.SetActive(true);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Destroy(gameObject);
    }

    [PunRPC]
    void easy(bool a)
    {
        if (a)
        {
            if (Block != null)
                PhotonNetwork.Destroy(Block);
            PhotonNetwork.Destroy(EasyTrigger);
        }
    }
}
