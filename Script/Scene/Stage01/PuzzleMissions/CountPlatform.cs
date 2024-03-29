using Photon.Pun;
using UnityEngine;

/// <summary>
/// 협력 플랫폼 미션을 수행하는 클래스
/// </summary>
/// <remarks>
/// 두 명이 모두 올라가야 미션이 수행된다.
/// </remarks>
/// @author 이은수
/// @date last change 2023/04/10
public class CountPlatform : MonoBehaviourPunCallbacks
{
    #region fields

    Animator animObj;
    int cnt = 0;

    #endregion

    #region methods

    [PunRPC]
    void UpdateCount(int val)
    {
        cnt += val;

        if (cnt == 0)                       // [Idle]
        {
            animObj.SetBool("One", false);
        }
        else if (cnt == 1)                  // [One]
        {
            animObj.SetBool("One", true);
            AudioManager.Instance.AddSfxSoundData(SFXClip.Or_UP, false, transform.position);
        }
        else if (cnt == 2)                  // [Two] = Mission Completed
        {
            animObj.SetBool("Two", true);
            AudioManager.Instance.AddSfxSoundData(SFXClip.Together_UP, false, transform.position);
            this.enabled = false;
        }
    }

    #endregion

    #region unity events

    private void Awake()
    {
        animObj = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetPhotonView().IsMine)
        {
            photonView.RPC(nameof(UpdateCount), RpcTarget.All, 1);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetPhotonView().IsMine)
        {
            photonView.RPC(nameof(UpdateCount), RpcTarget.All, -1);
        }
    }

    #endregion
}
