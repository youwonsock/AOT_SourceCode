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
/// @class EasyTrigger
public class EasyTrigger : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject EasyWall;
    [SerializeField] GameObject HardTrigger;
    [SerializeField] GameObject F;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                EasyWall.SetActive(false);
                HardTrigger.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            F.SetActive(true);
        }
    }
}
