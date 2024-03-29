using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 선을 건드릴 경우 큐브가 낙하하는 함정
/// </summary>
/// <remarks>
/// 선과 충돌한 오브젝트의 태그가 플레이어일 경우 연결된 큐브가 중력을 가지고 낙하합니다.
/// </remarks>
/// @author 이민우
/// @date last date 22/10/06
public class Trap2 : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rig = cube.GetComponent<Rigidbody>();
            rig.useGravity = true;
        }
    }
}
