using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 자동차 체력 관리 클래스
/// </summary>
/// <remarks>
/// 
/// </remarks>
///
/// @date last change 2023/05/19
/// @author YWS
/// @class Car_Health
public class Car_Health : MonoBehaviourPunCallbacks
{

    #region Fields

    //- Private -
    [SerializeField] private float carHealth = 100;
    [SerializeField] private int carMaxHealth = 100;
    private bool isHitable = true;

    //- Public -

    #endregion



    #region UnityEvent

    private void Awake()
    {
        carHealth = carMaxHealth;
    }

    private void SetHitable()
    {
        isHitable = true;
    }

    [PunRPC]
    private void SyncCarHealth()
    {
        isHitable = false;
        carHealth -= 20;
        AudioManager.Instance.AddSfxSoundData(SFXClip.CarCrash, false, transform.position);

        if (carHealth <= 0)
        {
            carHealth = carMaxHealth;

            this.transform.position = GameManager.Instance.RespawnPoint;
        }

        Invoke(nameof(SetHitable), 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && isHitable && photonView.IsMine)
        {
            photonView.RPC(nameof(SyncCarHealth), RpcTarget.All);

        }
    }

    #endregion
}
