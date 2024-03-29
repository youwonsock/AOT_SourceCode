using Photon.Pun;
using UnityEngine;
/// <summary>
/// 키입력 미션 수행 클래스
/// </summary>
/// <remarks>
/// IMission 인터페이스를 상속받아 ActivatePuzzle의 실제 동작을 구현
/// </remarks>
/// @author 이은수
/// @date last change 2023/04/09
public class InputMission : MonoBehaviourPunCallbacks, IMission, IPunObservable
{
    #region fields
    [SerializeField] Collider colliderObj; // next step의 collider
    [SerializeField] bool isOneWay;
    [SerializeField] int doorNum = 0; // 1=wood

    Animator animObj;   // Animation
    Light lightObj;     // Light Effect
    MissionInPlayer missionPlayer = null;

    bool performed;
    bool previous;
    #endregion

    #region properties
    /// <summary>
    /// performed 값이 true이면, 입력이벤트의 액션함수를 수행한다.
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
            if (previous == performed) // 지속입력방지
                return;

            if (performed)
                this.Action(true);
            else if (!isOneWay)
                this.Action(false);
            
            previous = performed; // 지속입력방지
        }
    }
    #endregion

    #region methods
    /// <summary>
    /// 플레이어가 미션오브젝트의 트리거에 진입했을 때, MissionInPlayer 컴포넌트를 참조하여 
    /// 키입력 여부를 LateUpdateWork()에서 참조할 수 있게 한다.
    /// </summary>
    /// <param name="player">플레이어의 미션오브젝트 컴포넌트</param>
    /// <param name="type">트리거 종류(enter/stay/exit)</param>
    void IMission.ActivatePuzzle(MissionInPlayer player, int type)
    {
        if (type == (int)TriggerType.Exit)  // [trigger exit?]
        {
            this.missionPlayer = null;
            if (animObj != null)
                animObj.SetBool("Collision", false);
        }
        else if (type == (int)TriggerType.Enter && player != null) // [trigger enter & stay?]
        {
            this.missionPlayer = player;
            if (animObj != null && photonView.IsMine)
                animObj.SetBool("Collision", true);

            photonView.ControllerActorNr = player.photonView.ControllerActorNr; // change photon-view control to me
        }
    }

    /// <summary>
    /// 애니메이션 이벤트 함수로, 레버가 완전히 당겨졌을 때 호출된다.
    /// </summary>
    void LeverComplete()
    {
        if (colliderObj != null)
            colliderObj.enabled = true;

        if (animObj != null)
            animObj.SetLayerWeight(1, 0); // text anim. layer off

        if (doorNum == 1)
            AudioManager.Instance.AddSfxSoundData(SFXClip.STAGE1_OpenWood, false, transform.position);

        TryGetComponent<Collider>(out var co);
        co.enabled = false;
        this.enabled = false;
    }

    /// <summary>
    /// 키입력 수행 시 호출되는 액션 함수
    /// </summary>
    void Action(bool val)
    {
        if (lightObj != null)
            lightObj.enabled = val;

        if (animObj != null)
            animObj.SetBool("LeverUp", val);

        if (val)
            AudioManager.Instance.AddSfxSoundData(SFXClip.Button, false, transform.position);
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
            stream.SendNext(Performed);
        else
            this.Performed = (bool)stream.ReceiveNext();
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
    private void UpdateWork()
    {
        if (missionPlayer == null || !photonView.IsMine) // [if exit trigger]
            return; 

        this.Performed = missionPlayer.ButtonPerformed;        // enable key-input 
    }
    public override void OnEnable()
    {
        UpdateManager.SubscribeToUpdate(UpdateWork);
    }
    public override void OnDisable()
    {
        UpdateManager.UnsubscribeFromUpdate(UpdateWork);
    }
    void IMission.ActivatePuzzleTogether(MissionInPlayer player, bool enter)
    {
        return;
    }
    #endregion
}
