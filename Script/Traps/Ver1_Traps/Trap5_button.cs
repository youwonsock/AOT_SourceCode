using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어가 함정 시작되는 지역에 들어왔음을 판정하는 스크립트
/// </summary>
/// <remarks>
/// 플레이어가 밟으면, 함정이 시작되어 플레이어에게 다가옵니다.
/// </remarks>
/// @author 이민우
/// @date last date 22/10/10
public class Trap5_button : MonoBehaviour
{
    [SerializeField] private GameObject trap;

    void Awake()
    {
        trap.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            trap.SetActive(true);
    }
}
