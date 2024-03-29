using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 떨어지는 발판입니다.
/// </summary>
/// <remarks>
/// 부딪힌 오브젝트를 검사하여, 태그가 플레이어일 경우 발판이 중력을 가지고 낙하합니다.
/// </remarks>
/// @author 이민우
/// @date last date 22/10/06
public class Trap1 : MonoBehaviour
{
    Rigidbody body;
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            body.useGravity = true;
    }
}
