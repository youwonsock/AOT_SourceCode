using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Slider와 toggle button을 통해 볼륨을 조절하는 UI 스크립트
/// </summary>
/// <remarks>
/// 슬라이더를 조절하여 설정한 볼륨값을 AudioManager에게 전달해 볼륨을 조절합니다.
/// Mute 토글 버튼을 누르면 해당 값을 0으로 설정하고 슬라이더를 조절할 수 없도록 했습니다.
/// </remarks>
/// @date last change 2023/05/30
/// @author MW
/// @class UI

public class UI_Volume : MonoBehaviour
{
    [SerializeField] public Slider MasterSlider;
    [SerializeField] public Slider BgmSlider;
    [SerializeField] public Slider SfxSlider;
    [SerializeField] public Toggle MasterToggle;
    [SerializeField] public Toggle BgmToggle;
    [SerializeField] public Toggle SfxToggle;

    public AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        MasterSlider.value = audioManager.Master;
        BgmSlider.value = audioManager.Bgm;
        SfxSlider.value = audioManager.Sfx;

    }

    public void SetMaster()
    {
        if (audioManager != null)
            audioManager.Master = MasterSlider.value;
    }
    public void SetBgm()
    {
        if (audioManager != null)
            audioManager.Bgm = BgmSlider.value;
    }
    public void SetSfx()
    {
        if (audioManager != null)
            audioManager.Sfx = SfxSlider.value;
    }

    public void MuteMaster(Toggle MasterToggle)
    {
        if(MasterToggle.isOn)
        {

            audioManager.MuteAll();
            MasterSlider.interactable = false;
            BgmSlider.interactable = false;
            SfxSlider.interactable = false;
        }
        else
        {
            SetMaster();
            SetBgm();
            SetSfx();

            MasterSlider.interactable = true;
            BgmSlider.interactable = true;
            SfxSlider.interactable = true;

        }
    }

    public void MuteBgm(Toggle BgmToggle)
    {
        if (BgmToggle.isOn)
        {
            audioManager.MuteBgm();
            BgmSlider.interactable = false;

        }
        else
        {
            SetBgm();
            BgmSlider.interactable = true;
        }
    }
    public void MuteSfx(Toggle SfxToggle)
    {
        if (SfxToggle.isOn)
        {
            audioManager.MuteSfx();
            SfxSlider.interactable = false;

        }
        else
        {
            SetSfx();
            SfxSlider.interactable = true;
        }
    }


}
