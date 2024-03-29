using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

/// <summary>
/// 다음 스테이지로 가는 처리를 맡은 스크립트
/// </summary>
/// <remarks>
/// 세이브 포인트가 갖고 있으며 세이브 포인트에 2명의 플레이어가 같이 존재하면 씬 전환되는 역할입니다
/// </remarks>
/// @author LSM
/// @date last change 2023/04/23
public class Next_Stage : MonoBehaviourPunCallbacks
{
    void Start()
    {
        GameManager.Instance.goal = 0;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.goal -= 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.goal += 1;
        }
    }
    
}