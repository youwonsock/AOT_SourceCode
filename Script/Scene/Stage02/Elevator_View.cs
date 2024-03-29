using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 컨테이너 시점 변환 이벤트를 제공하는 스크립트
/// </summary>
/// 
/// @date last change 2023/05/27
/// @author LSM, YWS
/// @class Elevator_View
public class Elevator_View : MonoBehaviourPunCallbacks
{
    private IChangeView changeView;
    [SerializeField] private PhotonView elePhotonView;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") &&  elePhotonView == null &&  other.GetComponent<PhotonView>().IsMine)
        {
            other.TryGetComponent<IChangeView>(out changeView);
            other.TryGetComponent<PhotonView>(out elePhotonView);
        }
        else if (other.CompareTag("Player") && elePhotonView != null && elePhotonView.IsMine)
        {
            GameManager.Instance.ShaderChange = false;

            changeView.ChangeView(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && elePhotonView != null && elePhotonView.IsMine)
        {
            GameManager.Instance.ShaderChange = true;

            changeView.ChangeView(3);

            elePhotonView = null;
            changeView = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            AudioManager.Instance.AddSfxSoundData(SFXClip.ContainerFall, false, transform.position);
        }
    }
}
