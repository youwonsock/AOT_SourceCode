using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 열쇠를 얻으면 문이 열립니다.
/// </summary>
/// remarks>
/// 플레이어가 열쇠를 획득했는지 검사하고, 획득했다면 문을 움직입니다.
/// </remarks>
/// @author 이민우
/// @date last date 22/10/26
public class DoorOpen_key : MonoBehaviour
{
    [SerializeField] private GameObject left_door;
    [SerializeField] private GameObject right_door;
    [SerializeField] public bool isGotKey;

    private void Awake()
    {
        isGotKey = false;
    }
    void FixedUpdate()
    {
        if (isGotKey)
        {
            Vector3 velo = Vector3.zero;

            left_door.transform.position = Vector3.MoveTowards(left_door.transform.position, new Vector3(0.45f, 1.25f, 11), Time.deltaTime);
            right_door.transform.position = Vector3.MoveTowards(right_door.transform.position, new Vector3(3.55f, 1.25f, 11), Time.deltaTime);
        }

    }

}
