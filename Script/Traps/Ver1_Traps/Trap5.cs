using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 실제 함정이 움직이는 부분
/// </summary>
/// <remarks>
/// 문제가 적힌 벽이 플레이어 근처에 배치된 target 오브젝트를 향해 다가오고
/// target 오브젝트와 만날 경우 벽이 destroy됩니다.
/// </remarks>
/// @auther 이민우
/// @date last date 22/10/10

public class Trap5 : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
    private void FixedUpdate()
    { 
        Vector3 targetPosition = target.transform.position;
        if(gameObject.activeSelf)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.0075f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "target")
        {
            Destroy(gameObject);
        }
    }
}
