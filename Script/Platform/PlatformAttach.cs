using UnityEngine;
using Photon.Pun;

/// <summary>
/// WaypointMoving을 트리거미션으로 사용할 때 트리거의 타겟이 되는 무빙플랫폼을 위한 플랫폼어태치
/// </summary>
/// @author 유원석
/// @date last change 2023/03/29
public class PlatformAttach : MonoBehaviourPunCallbacks
{
    #region Fields
    #endregion

    #region Property
    #endregion

    #region Methods
    [PunRPC]
    private void SetTransform(int photonViewNum)
    {
        var target = PhotonView.Find(photonViewNum).transform;

        if (target.parent == null)
            target.SetParent(this.transform);
        else
        {
            Debug.Log("set null");
            target.SetParent(null);
        }
    }
    #endregion

    #region Unity Event
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetPhotonView().IsMine)
            photonView.RPC(nameof(SetTransform), RpcTarget.All, other.gameObject.GetPhotonView().ViewID);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetPhotonView().IsMine)
        {
            Debug.Log("exit");
            photonView.RPC(nameof(SetTransform), RpcTarget.All, other.gameObject.GetPhotonView().ViewID);
        }
    }
    #endregion
}
