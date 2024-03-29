using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 크로스 헤어의 변경을 관리하는 UI 스크립트
/// </summary>
/// @date last change 2023/05/30
/// @author MW
/// @class UI

public class UI_CHmanager : Singleton<UI_CHmanager>
{
    public Image ch;
    public Sprite[] imgs;

    /**
     * @brief 플레이어가 설정한 색상과 모양을 적용합니다
     * @param int i 플레이어가 설정한 크로스 헤어 모양
     * @param Color c 플레이어가 설정한 크로스 헤어 색상
     */
    public void OnImageSwitched(int i, Color c)
    {
        ch.sprite = imgs[i];
        Color ic = ch.GetComponent<Image>().color;
        ic = c;
        ch.GetComponent<Image>().color = ic;
    }
}
