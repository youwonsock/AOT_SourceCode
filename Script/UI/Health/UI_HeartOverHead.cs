using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
/// <summary>
/// 플레이어의 머리 위의 체력바가 자신의 것은 보이지 않게 하는 스크립트입니다.
/// </summary>
/// @date last change 2023/01/16
/// @author MW
/// @class UI
public class UI_HeartOverHead : MonoBehaviourPunCallbacks
{

    [SerializeField] private Image HeartUI;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
           HeartUI.gameObject.SetActive(false);
        }
    }

}
