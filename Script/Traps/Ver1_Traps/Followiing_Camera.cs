using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 플레이어를 따라오는 카메라 입니다.
/// </summary>
/// <remarks>
/// 플레이어보다 약간 뒤에서 플레이어를 비추는 카메라 입니다.
/// </remarks>
/// @author 이민우
/// @date last date 22/09/30
public class Followiing_Camera : MonoBehaviour
{
    public Transform target;
    private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        tr.position = new Vector3 (target.position.x - 0.52f, tr.position.y, target.position.z - 6.56f);
        tr.LookAt(target);
    }
}
