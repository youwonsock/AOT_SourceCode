using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
/// <summary>
/// 각 인칭에서 시야각을 조절하는 UI 스크립트
/// </summary>
/// <remarks>
/// 플레이어의 카메라 컴포넌트를 가져와 1인칭에서와 3인칭에서의 시야각을
/// 슬라이더를 통해 조절할 수 있게 했습니다. 슬라이더 옆에는 숫자를 표시하여
/// 플레이어가 좀 더 세밀하게 값을 조절할 수 있도록 돕습니다.
/// </remarks>
/// @date last change 2023/05/30
/// @author MW
/// @class UI

public class UI_POV : MonoBehaviour
{

    private CinemachineVirtualCamera FPVcam;
    private CinemachineVirtualCamera TPVcam;

    [SerializeField] public Slider fpvSlider;
    [SerializeField] public Slider tpvSlider;

    [SerializeField] public GameObject fpvBox;
    [SerializeField] public GameObject tpvBox;
    private Text fpvtxt;
    private Text tpvtxt;

    private PlayerCamera pc;

    
    private void Awake()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCamera>();

        FPVcam = pc.FPVCamera;
        TPVcam = pc.TPVCamera;

        fpvSlider.value = FPVcam.m_Lens.FieldOfView;
        tpvSlider.value = TPVcam.m_Lens.FieldOfView;

        fpvtxt = fpvBox.GetComponent<Text>();
        tpvtxt = tpvBox.GetComponent<Text>();

    }

    public void SetFpv()
    {
        if (FPVcam != null)
            FPVcam.m_Lens.FieldOfView = fpvSlider.value;
        if (fpvtxt != null ) 
            fpvtxt.text = fpvSlider.value.ToString();
    }
    public void SetTpv()
    {
        if (TPVcam != null)
            TPVcam.m_Lens.FieldOfView = tpvSlider.value;
        if (tpvtxt != null) 
            tpvtxt.text = tpvSlider.value.ToString();
    }
}
