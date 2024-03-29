using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// 미로의 위치 바꾸기 시스템
/// </summary>
/// 
/// 
/// @date last change 2023/05/27
/// @author LSM
/// @class Position_Switch
public class Position_Switch : MonoBehaviourPunCallbacks
{
    GameObject[] index;
    GameObject player1;
    GameObject player2;
    
    private void Start()
    {
        Invoke("playerFind", 2f);
    }

    void playerFind()
    {
        index = GameObject.FindGameObjectsWithTag("Player");
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("playerIndex", RpcTarget.All, index[0].GetPhotonView().ViewID, index[1].GetPhotonView().ViewID);
        }
    }
    [PunRPC]
    void playerIndex(int view1, int view2)
    {
        player1 = PhotonView.Find(view1).gameObject;
        player2 = PhotonView.Find(view2).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetPhotonView().IsMine)
        {
            AudioManager.Instance.AddSfxSoundData(SFXClip.PosChangeItem, false, transform.position);
            Vector3 pos1 = player1.transform.position;
            Vector3 pos2 = player2.transform.position;
            photonView.RPC("SwitchPlayerPositions", RpcTarget.AllViaServer, pos1, pos2);
        }
    }

    [PunRPC]
    void SwitchPlayerPositions(Vector3 pos1, Vector3 pos2)
    {
        player1.transform.position = pos2;
        player2.transform.position = pos1;

        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.Destroy(gameObject);
    }

}
