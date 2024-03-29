using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HardSelect : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject Block;
    [SerializeField] GameObject HardTrigger;
    [SerializeField] GameObject EasyStage;
    [SerializeField] GameObject G;

    private void OnTriggerEnter(Collider other)
    {
        photonView.RPC("hard", RpcTarget.MasterClient, other.CompareTag("Player"));
        if (other.GetComponent<PhotonView>().IsMine)
        {
            G.SetActive(true);
            EasyStage.SetActive(false);
        }
            
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Destroy(gameObject);
    }

    [PunRPC]
    void hard(bool a)
    {
        if (a)
        {
            if (Block != null)
                PhotonNetwork.Destroy(Block);
            PhotonNetwork.Destroy(HardTrigger);
        }
    }
}
