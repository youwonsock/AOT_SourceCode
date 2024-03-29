using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 마우스 조작 민감도를 조절하는 UI 스크립트
/// </summary>
/// <remarks>
/// 플레이어의 카메라 컴포넌트를 가져와 슬라이더를 통해 마우스 민감도를 조절합니다.
/// 슬라이더 바의 옆에 숫자를 표시해 더 세밀하게 조절할 수 있도록 했습니다.
/// </remarks>
/// @date last change 2023/05/30
/// @author MW
/// @class UI

public class UI_Sensitivity : MonoBehaviour
{
    private PlayerCamera pc;

    [SerializeField] public Slider slider;
    [SerializeField] public GameObject txtBox;
    
    private void Awake()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCamera>();
        if (pc != null)
                slider.value = pc.Sensitivitys;

    }

    public void SetSens()
    {
        if (pc != null) 
            pc.Sensitivitys = slider.value;
        txtBox.GetComponent<Text>().text = (10*slider.value).ToString("F0");

    }

}
