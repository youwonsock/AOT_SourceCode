using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 정답인 문을 골라서 넘어갈 경우 함정이 사라짐
/// </summary>
/// <remarks>
/// 정답인 문과 플레이어가 부딪힐 경우 함정 오브젝트가 파괴됩니다.
/// </remarks>
/// @author 이민우
/// @date last date 22/12/06
public class Correct_answer : MonoBehaviour
{
 
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(transform.parent.gameObject);    
        }
    }
    
}
