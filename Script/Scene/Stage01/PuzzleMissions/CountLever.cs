using UnityEngine;
using Photon.Pun;

/// <summary>
/// 협력레버 혹은 버튼 미션을 수행하는 클래스
/// </summary>
/// <remarks>
/// IMission 인터페이스를 상속받아 ActivatePuzzle의 실제 동작을 구현
/// </remarks>
/// @author 이은수
/// @date last change 2023/04/09
public class CountLever : MonoBehaviourPunCallbacks, IMission
{
    #region fields

    [SerializeField] bool isLakeLever = false;
    Animator animObj;
    Light lightObj;
    MissionInPlayer missionPlayer;

    int cnt = 0;

    bool performed;
    bool previous;          // 지속입력방지
    bool canInput = true;   // 연타입력방지

    #endregion

    #region properties

    /// <summary>
    /// performed 값이 true이면, 입력 가능한 상태일 때 UpdateCount 함수를 RPC로 호출한다.
    /// F키 입력/해제 시 카운트를 1만큼 증가/감소시킨다.
    /// </summary>
    public bool Performed
    { 
        get
        {
            return performed;
        }
        set 
        {
            performed = value;
            if (previous == performed) // 지속입력 방지
                return;

            if (performed)
            {
                if (!canInput)
                    return;
                canInput = false; // 연타 방지
                photonView.RPC(nameof(UpdateCount), RpcTarget.All, 1, true);
            }
            else 
            {
                photonView.RPC(nameof(UpdateCount), RpcTarget.All, -1, true);
                canInput = true;
            }
            previous = performed; // 지속입력 방지
        }
    }

    #endregion

    #region methods

    /// <summary>
    /// 협력레버 혹은 버튼의 트리거와 충돌/해제 시 카운트를 1만큼 증가/감소시킨다.
    /// </summary>
    /// <param name="player">플레이어의 미션오브젝트 컴포넌트</param>
    /// <param name="type">트리거 종류(enter/stay/exit)</param>
    void IMission.ActivatePuzzle(MissionInPlayer player, int type)
    {
        if (type == (int)TriggerType.Enter && player != null)
        {
            this.missionPlayer = player;
            photonView.RPC(nameof(UpdateCount), RpcTarget.All, 1, false);
        }
        else if (type == (int)TriggerType.Exit)
        {
            this.missionPlayer = null;
            photonView.RPC(nameof(UpdateCount), RpcTarget.All, -1, false);
        }
    }

    /// <summary>
    /// 협력레버의 카운트 값을 증감시키고, 그 값에 맞는 애니메이션을 실행하는 함수
    /// </summary>
    /// <remarks>
    /// 애니메이션 상태 순서: [Idle] <-> [Zero] <-> [One] -> [Two] -> [Completed]
    /// </remarks>
    /// <param name="val">카운트 증감값</param>
    /// <param name="inputEvent">입력이벤트에 의한 수행인지</param>
    [PunRPC]
    void UpdateCount(int val, bool inputEvent)  // [Below shows animation state]
    {
        cnt += val;

        if (cnt == 0)                           // [Idle]
        {
            animObj.SetBool("Zero", false);
        }
        else if (cnt == 1)                      // [Zero]
        {
            animObj.SetBool("Zero", true);
            animObj.SetBool("One", false);

            AudioManager.Instance.AddSfxSoundData(SFXClip.Text, false, transform.position);
        }
        else if (cnt == 2 && inputEvent) 
        {
            if (val > 0) // F키를 눌렀을 때
            {
                AudioManager.Instance.AddSfxSoundData(SFXClip.Button, false, transform.position);
                animObj.SetBool("One", true);   // [Zero] -> [One]
            }
            else        // F키를 뗐을 때
            {
                animObj.SetBool("One", false);  // [One] -> [Zero]
            }
        }
        else if (cnt == 3)                      // [One]
        {
            AudioManager.Instance.AddSfxSoundData(SFXClip.Button, false, transform.position);
            animObj.SetBool("One", true);
        }
        else if (cnt == 4)                      // [Two] = Mission Completed
        {
            if (isLakeLever)
                AudioManager.Instance.AddSfxSoundData(SFXClip.LakeMove, false, transform.position);
            else
                AudioManager.Instance.AddSfxSoundData(SFXClip.TUTO_OpenDoor, false, transform.position);

            if (lightObj != null)
                lightObj.enabled = true;

            TryGetComponent<Collider>(out var co);
            co.enabled = false;
            cnt = 0; // initiate

            animObj.SetBool("Two", true);

            this.enabled = false;
        }
    }

    #endregion

    #region unity events

    private void Awake()
    {
        animObj = GetComponentInChildren<Animator>();
        lightObj = GetComponentInChildren<Light>();
    }

    /// <summary>
    /// trigger enter 시, 키입력 여부에 따라 Performed 프로퍼티를 통해 액션 수행을 관리한다.
    /// </summary>
    /// <remarks>
    /// missionPlayer == null의 의미는 trigger exit 된 시점을 의미한다.
    /// </remarks>
    void UpdateWork()
    {
        if (missionPlayer == null)
            return;

        this.Performed = missionPlayer.ButtonPerformed; 
    }
    
    public override void OnEnable()
    {
        UpdateManager.SubscribeToUpdate(UpdateWork);
    }
    public override void OnDisable()
    {
        UpdateManager.UnsubscribeFromUpdate(UpdateWork);
    }

    void IMission.ActivatePuzzleTogether(MissionInPlayer player, bool _)
    {
        return;
    }

    #endregion
}
