using Photon.Pun;
using UnityEngine;
using Cinemachine;
using Kinemation.FPSFramework.Runtime.Layers;

public enum ViewMode
{
    FPV,
    TPV
}

/// <summary>
/// 플레이어 카메라 제어 클래스
/// </summary>
/// 
/// @author yws
/// @date last change 2023/05/20
public class PlayerCamera : MonoBehaviourPun, IChangeView
{

    #region Fields

    //- Private -
    [Tooltip("올려다 볼 수 있는 카메라 각도 한계치")][SerializeField] private float TopClamp = 60.0f;
    [Tooltip("내려다 볼 수 있는 카메라 각도 한계치")][SerializeField] private float BottomClamp = -60.0f;
    [SerializeField] private Transform TPcinemachineCameraTarget;
    [SerializeField] private Transform FPcinemachineCameraTarget;
    [Range(1f, 10f)][SerializeField] private float sensitivitys;

    private float _cinemachineTargetPitch;
    private float _cinemachineTargetYaw;
    [SerializeField] public CinemachineVirtualCamera TPVCamera;
    [SerializeField] public CinemachineVirtualCamera FPVCamera;

    //- Public -

    #endregion

    public float Sensitivitys { get { return sensitivitys; } set { sensitivitys = value; } }

    #region Methods
    //- Private -

    public void SetFOV(int newFOV, ViewMode mode)
    {
        if (mode == ViewMode.FPV)
        {
            FPVCamera.m_Lens.FieldOfView = newFOV;
        }
        else
        {
            TPVCamera.m_Lens.FieldOfView = newFOV;
        }

    }


    /// <summary>
    /// 카메라 회전 로직 함수
    /// </summary>
    public void CameraRotation(P_Input input)
    {
        _cinemachineTargetYaw += input.Look.x * sensitivitys ;
        _cinemachineTargetPitch += input.Look.y * sensitivitys ;

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

       
        if (FPVCamera.isActiveAndEnabled)
        {
            transform.rotation = Quaternion.Euler(0, _cinemachineTargetYaw, 0.0f); 
            FPcinemachineCameraTarget.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
        }
        else
            TPcinemachineCameraTarget.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);

    }

    /// <summary>
    /// 시점 전환 메서드 
    /// </summary>
    /// <param name="n"> 전환할 시점</param>
    public void ChangeView(int n)
    {
        // 3 -> 1
        if (n == 1)
        {
            TPVCamera.gameObject.SetActive(false);
            FPVCamera.gameObject.SetActive(true);

            GameManager.Instance.OnChangePV(1);
        }
        // 1 -> 3
        else
        {
            TPVCamera.gameObject.SetActive(true);
            FPVCamera.gameObject.SetActive(false);

            GameManager.Instance.OnChangePV(3);
        }
    }

    /// <summary>
    /// 카메라 회전 범위 제한 함수
    /// </summary>
    /// <param name="lfAngle"></param>
    /// <param name="lfMin"></param>
    /// <param name="lfMax"></param>
    /// <returns></returns>
    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    /// <summary>
    /// 초기화 메서드
    /// </summary>
    private void Init()
    {
        if (!photonView.IsMine)
        {
            TPVCamera.enabled = false;
            FPVCamera.enabled = false;
            return;
        }
            

        if (TPVCamera == null || FPVCamera == null)
        {
            transform.Find("TPVCamera").TryGetComponent<CinemachineVirtualCamera>(out TPVCamera);
            transform.Find("FPVCamera").TryGetComponent<CinemachineVirtualCamera>(out FPVCamera);

            Debug.Log("TPVCamera, FPVCamera NULL!!!");
        }

        TPVCamera.Follow = TPcinemachineCameraTarget;
        FPVCamera.Follow = FPcinemachineCameraTarget;

        TPVCamera.gameObject.SetActive(true);
        FPVCamera.gameObject.SetActive(false);

    }

    #endregion



    #region UnityEvent

    private void Start()
    {
        Init();
    }

    #endregion
}