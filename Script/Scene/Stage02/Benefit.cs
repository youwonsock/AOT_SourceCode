using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// 스테이지 선택권을 부여하기 위해 플레이어를 분리시키는 스크립트
/// </summary>
/// 
/// @date last change 2023/05/27
/// @author LSM
/// @class Benefit
public class Benefit : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject Block;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine && GameManager.Instance.master_first && PhotonNetwork.IsMasterClient)
        {
            Block.SetActive(false);
        }
        else if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine && GameManager.Instance.client_first && !PhotonNetwork.IsMasterClient)
        {
            Block.SetActive(false);
        }
    }
}
