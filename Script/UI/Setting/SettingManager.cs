using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Setting UI 패널 관리 클래스
/// </summary>
/// <remarks>
/// 기존의 흩어져있던 Setting UI 관련 스크립트 (CloseSetting, UI_SettingBtn)을 합쳤습니다.
/// </remarks>
/// @date last change 2023/05/08
/// @author MW
/// @class UI
public class SettingManager : MonoBehaviour
{

    #region Field
    public static SettingManager instance;

    private GameObject setting_ui;

    [SerializeField] GameObject vpanel;
    [SerializeField] GameObject cpanel;
    [SerializeField] GameObject chpanel;
    #endregion


    #region Property
    public static SettingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SettingManager>();
            }
            return instance;
        }
    }
    
    #endregion

    #region Methods
    public void CloseSettingUI()
    {
        setting_ui.SetActive(false);
    }

    public void ActiveVolume()
    {
        vpanel.SetActive(true);
        cpanel.SetActive(false);
        chpanel.SetActive(false);
    }

    public void ActiveCamera()
    {
        vpanel.SetActive(false);
        cpanel.SetActive(true);
        chpanel.SetActive(false);
    }
    public void ActiveCrosshair()
    {
        vpanel.SetActive(false);
        cpanel.SetActive(false);
        chpanel.SetActive(true);
    }

    #endregion

    #region Unity Event
    private void Start()
    {
        setting_ui = UIManager.Instance.SettingUI;
    }
    #endregion


}
