using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 크로스헤어의 모양과 색상을 설정하는 UI 스크립트
/// </summary>
/// @date last change 2023/05/30
/// @author MW
/// @class UI

public class UI_CHChange : MonoBehaviour
{
    [SerializeField] public Sprite[] imgs;
    [SerializeField] public Image image;
    public GameObject colorpicker;

    private int iter = 0;

    public void ChangeImage(int i)
    {
        image.sprite = imgs[i];
    }
    /**
     * @details 왼쪽 버튼을 누를 때, imgs[]의 iterator를 감소시킵니다. 
     * 또한 위의 이미지를 imgs[iter]로 설정합니다.
     */
    public void LeftbtnClick()
    {
        iter--;
        if (iter < 0) iter = 2;
        ChangeImage(iter);
    }
    /**
     * @details 오른쪽 버튼을 누를 때, imgs[]의 iterator를 증가시킵니다. 
     * 또한 위의 이미지를 imgs[iter]로 설정합니다.
     */
    public void RightbtnClick()
    {
        iter++;
        if (iter > 2) iter = 0;
        ChangeImage(iter);
    }
    /**
     * @brief 적용 버튼에 할당되는 메소드 입니다.
     * @details iter값과 Colorpicker의 색상 정보를 UI_CHmanager로 값을 전달하여
     * 설정한 모양과 색상으로 크로스 헤어를 변경합니다.
     */
    public void AdaptbtnClick()
    {
        Color c = colorpicker.GetComponent<ColorPicker>().Color;
        int i = iter;

        UI_CHmanager.Instance.OnImageSwitched(i, c);
    }
}
