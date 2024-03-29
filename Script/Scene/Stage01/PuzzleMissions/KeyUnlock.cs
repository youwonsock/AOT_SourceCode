using UnityEngine;
using Photon.Pun;

/// <summary>
/// 잠금장치 해제
/// </summary>
/// <remarks>
/// 잠금장치가 열쇠를 감지하면, 잠금문이 열린다. 
/// </remarks>
/// @memo 이 스크립트는 잠금장치 오브젝트에 붙여 사용합니다.
/// @author 이은수
/// @date last change 2023/05/17
public class KeyUnlock : MonoBehaviourPunCallbacks
{
    #region fields
    [SerializeField] Collider colliderObj; // next step의 collider을 활성화하는 목적
    [Tooltip("잠금문"), SerializeField] KeyDoor DoorObject;
    [Tooltip("열쇠 오브젝트"), SerializeField] GameObject KeyObject;
    [Tooltip("열쇠 위치설정값"), SerializeField] Vector3 offset;
    HandMission Key;
    [SerializeField] GameObject Stage2_Mission;

    [SerializeField] GameObject TutorialObj;
    [SerializeField] int doorNum = 0; // 1=튜토리얼 석상, 1스테이지 석상, 2= 검, 3=열쇠

    #endregion

    #region unity events
    private void Awake()
    {
        KeyObject.TryGetComponent<HandMission>(out Key);
        this.gameObject.layer = 14;
    }

    /// <summary>
    /// 잠금장치가 열쇠를 감지하면 열쇠의 위치를 고정시키고, 잠겨있던 문을 연다.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == KeyObject)
        {
            photonView.RPC(nameof(ConnectKey), RpcTarget.All, null);

            
        }
    }
    #endregion

    #region method
    /// <summary>
    /// 열쇠의 위치를 잠금장치 위에 고정시키고, 잠겨있던 문을 연 다음, 재감지를 방지하기 위해 컴포넌트를 비활성화하는 함수
    /// </summary>
    [PunRPC]
    public void ConnectKey()
    {
        // 1. notify 'unlock' to HandMission Script(=Key)
        if (Key != null)
            Key.Unlock = true;

        // 2. connection work
        KeyObject.transform.position = transform.position + offset; // fix key position

        AudioManager.Instance.AddSfxSoundData(SFXClip.KeyDown, false, transform.position);

        Light lightObj = GetComponentInChildren<Light>();
        if (lightObj != null)
            lightObj.enabled = true;

        // 3. open door
        if (DoorObject != null) 
            DoorObject.DoorAction();

        // play sfx sound
        if (doorNum == 1)
            AudioManager.Instance.AddSfxSoundData(SFXClip.Wheel, false, transform.position);
        else if (doorNum == 2)
            AudioManager.Instance.AddSfxSoundData(SFXClip.Together_UP, false, transform.position);
        else if (doorNum == 3)
            AudioManager.Instance.AddSfxSoundData(SFXClip.STAGE1_OpenDoor, false, transform.position);

        // 4. go to the next step
        if (colliderObj != null)
            colliderObj.enabled = true;

        if (TutorialObj != null)
            TutorialObj.SetActive(true);


        TryGetComponent<Collider>(out var co);
        co.enabled = false;
        this.enabled = false;

        if (Stage2_Mission != null)
        {
            PhotonNetwork.Destroy(Stage2_Mission);
        }
    }
    #endregion
}