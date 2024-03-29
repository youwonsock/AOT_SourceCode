using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ESC input을 케이스별로 관리하는 스크립트입니다.
/// </summary>
/// 
/// @date last change 2023/05/10
/// @author MW
/// @class UI
public class Call_Pause : MonoBehaviour
{
    private GameObject pause_ui;
    private GameObject setting_ui;
    private void Awake()
    {
        pause_ui = UIManager.Instance.PauseUI;
        setting_ui = UIManager.Instance.SettingUI;
    }
    private void Update()
    {
        /**
         * @brief 일시정지 UI를 키고 끄는 것을 관리
         * @detail 일시정지 UI가 켜져 있을 때는 Setting이 켜져 있을 경우, Setting을 먼저 닫고@n
         * Setting이 꺼져 있을 경우 일시정지 UI를 닫는다.@n
         * 일시정지 UI가 꺼져 있다면, 일시정지 UI를 호출한다.
         */
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //일시정지 중일때
            if (pause_ui.activeSelf)
            {
                if (setting_ui.activeSelf)
                {
                    SettingManager.Instance.CloseSettingUI();
                }
                else
                {
                    UIManager.Instance.ExitUI();
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            //일시정지가 해제됐을 때
            else
            {
                UIManager.Instance.PauseUICall();
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
    }
}
