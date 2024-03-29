using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

/// <summary>
/// 하트의 갯수를 표시해주는 UI
/// </summary>
/// <remarks>
/// 
/// </remarks>
/// @author 이민우
/// @date last date 23/04/10
/// 

/**
* @param ph 플레이어의 체력 컴포넌트 가져옵니다
* @param HeartSprites 체력에 따라 표시할 하트 이미지를 모아둔 배열
* @param HeartUI 하트 이미지를 표시할 오브젝트
*/
public class UI_Heart : MonoBehaviourPunCallbacks
{
    
    Player ph;

    public Sprite[] HeartSprites;
    public GameObject Heart;
    public Image HeartUI;
    

    void Start()
    {
        if (photonView.IsMine)
        {
            TryGetComponent<Player>(out ph);
            Heart = GameObject.FindGameObjectWithTag("Health");
            Heart.TryGetComponent<Image>(out HeartUI);
        }
    }
    /**
     * @details 체력에 따라 이미지를 바꿔줍니다.
     * @param c_health 플레이어의 현재 체력
     * @param idx 플레이어의 체력에 따라 배열에서 해당하는 이미지 선택
     */
     
    public void ChangeHeartImage()
    {
        if(ph == null)
            GameObject.FindGameObjectWithTag("Player").TryGetComponent<Player>(out ph);

        float c_health = ph.Health;
        if (c_health < 0)
        {
            HeartUI.sprite = HeartSprites[0];
        }
        else
        {
            double idx = c_health / 12.5;
            HeartUI.sprite = HeartSprites[(int)idx];
        }
        
    }
}
