using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 캔버스가 유저의 카메라를 보도록 하는 스크립트
/// </summary>
/// <remarks>
/// 캔버스의 트랜스폼을 메인 카메라의 위치를 받아와
/// 마우스를 움직이더라도 체력바가 정상적으로 플레이어를 바라보도록
/// 하는 스크립트입니다.
/// </remarks>
///
/// @date last change 2023/01/16
/// @author MW
/// @class UI

public class UI_HealthCanvas : MonoBehaviour
{
    private Transform tr;
    private Transform mainCamtr;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        mainCamtr = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        tr.LookAt(mainCamtr);
    }
}
