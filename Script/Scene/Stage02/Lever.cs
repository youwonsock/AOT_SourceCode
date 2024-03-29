using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/// <summary>
/// 컨테이너를 떨어트리는 스크립트
/// </summary>
/// 
/// @date last change 2023/05/27
/// @author LSM
/// @class Lever
public class Lever : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject container;
    [SerializeField] GameObject entrance;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            photonView.RPC("AddRigidbody", RpcTarget.AllViaServer, Input.GetKey(KeyCode.F));
        }
    }
    
    [PunRPC]
    void AddRigidbody(bool check)
    {
        if (check && !container.GetComponent<Rigidbody>())
        {
            container.AddComponent<Rigidbody>();
            container.GetComponent<Rigidbody>().freezeRotation = true;
            entrance.SetActive(false);
        }
    }
}
