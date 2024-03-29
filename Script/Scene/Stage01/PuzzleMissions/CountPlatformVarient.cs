using Photon.Pun;
using UnityEngine;

/// <summary>
/// 1스테이지 그래플링훅 구간에서 협력 플랫폼 미션을 수행하는 클래스
/// </summary>
/// <remarks>
/// 두 명이 모두 올라가야 계단이 올라가는 애니메이션이 수행된다.
/// </remarks>
/// @author 이은수
/// @date last change 2023/05/30
public class CountPlatformVarient : MonoBehaviourPunCallbacks
{
    #region fields

    [SerializeField] GameObject textObj1; // alone
    [SerializeField] GameObject textObj2; // or
    [SerializeField] GameObject textObj3; // together
    [SerializeField] GameObject textObj4; // stairs

    int cnt = 0;

    #endregion

    #region methods

    [PunRPC]
    void UpdateCount(int val)
    {
        cnt += val;

        if (cnt == 0)                       // [Idle]
        {
            textObj1.SetActive(false);
        }
        else if (cnt == 1)                  // [One]
        {
            textObj1.SetActive(true);       // alone

            AudioManager.Instance.AddSfxSoundData(SFXClip.Alone_UP, false, transform.position);

        }
        else if (cnt == 2)                  // [Two] = Mission Completed
        {
            TryGetComponent<Collider>(out var co);
            co.enabled = false;
            textObj2.SetActive(true);       // or

            AudioManager.Instance.AddSfxSoundData(SFXClip.Or_UP, false, transform.position);

            Invoke(nameof(DelayMove), 1.0f);
        }
    }

    [PunRPC]
    private void SyncGM(bool val)
    {
        if (val)
        {
            GameManager.Instance.master_first = true;
            GameManager.Instance.client_first = false;
        }
        else
        {
            GameManager.Instance.master_first = false;
            GameManager.Instance.client_first = true;
        }
    }

    void DelayMove()
    {
        textObj3.SetActive(true); // together
        textObj4.SetActive(true); // stairs

        AudioManager.Instance.AddSfxSoundData(SFXClip.Together_UP, false, transform.position);
    }

    #endregion

    #region unity events

    private void Awake()
    {
        textObj1.SetActive(false);
        textObj2.SetActive(false);
        textObj3.SetActive(false);
        textObj4.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetPhotonView().IsMine)
        {
            photonView.RPC(nameof(UpdateCount), RpcTarget.All, 1);

            if (PhotonNetwork.IsMasterClient && !GameManager.Instance.master_first && !GameManager.Instance.client_first)
                photonView.RPC(nameof(SyncGM), RpcTarget.All, true);
            else if (!PhotonNetwork.IsMasterClient && !GameManager.Instance.master_first && !GameManager.Instance.client_first)
                photonView.RPC(nameof(SyncGM), RpcTarget.All, false);
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
