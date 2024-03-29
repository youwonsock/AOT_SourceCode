using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어와 상호작용을 위한 스크립트
/// </summary>
/// <remarks>
/// 플레이어가 부딪힐 경우, 조명을 켜고 숨겨진 문을 드러냅니다.
/// </remarks>
/// @date last change 2023/05/16
/// @author MW
/// 
public class bottle : MonoBehaviour
{
    [SerializeField] GameObject ceiling;
    [SerializeField] GameObject spotlgt;


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            spotlgt.SetActive(true);
            ceiling.SetActive(false);
            CreditManager.Instance.DoorActive();
        }
    }
}
