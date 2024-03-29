

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Threading.Tasks.Sources;
/// <summary>
/// 플레이어 체력을 관리하는 Presenter 스크립트 입니다.
/// </summary>
/// <remarks>
/// 플레이어의 체력을 관리하는 스크립트로, Model인 Player스크립트와 View인 UI_PlayerView에
/// 대한 정보를 갖고 있습니다. 체력의 변동을 감지할 경우 Model로 그 정보를 넘겨 업데이트하고,
/// View의 체력 UI 변경 메소드를 호출합니다.
/// </remarks>
/// @date last change 2023/05/30
/// @author MW
/// @class UI

public class UI_Presenter : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private Player Model;
    [SerializeField] private UI_PlayerView View;
    private static UI_Presenter instance;

    public UI_Presenter(Player model, UI_PlayerView view)
    {
        Model = model;
        View = view;
    }

    public void connect_mvp(Player model, UI_PlayerView view)
    {
        Model = model;
        View = view;
    }


    public static UI_Presenter GetInstance(Player model, UI_PlayerView view)
    {
        if (instance == null)
        {
            instance = new UI_Presenter(model, view);
        }
        return instance;
    }

    [PunRPC]
    public void UpdateHealthRPC(float damage)
    {

        Model.Health -= damage;

        if (Model.Health <= 0)
        {
            Model.Health = Model.maxHealth;
            Model.Respawn();
        }

        View.UpdateHealthBar(Model.Health, Model.maxHealth);
    }

    public void UpdateHealth(float damage)
    {
        photonView.RPC("UpdateHealthRPC", RpcTarget.All, damage);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Model.Health);
        }
        else
        {
            Model.Health = (float)stream.ReceiveNext();
        }
    }
}
