using UnityEngine;
using DG.Tweening;
using Photon.Pun;

/// <summary>
/// 버튼으로 징검다리 이동미션을 수행하는 클래스 
/// </summary>
/// <remarks>
/// DOTween API를 이용하여 모션 구현,
/// IMission 인터페이스를 상속받아 ActivatePuzzle의 실제 동작을 구현
/// </remarks>
/// @author 이은수
/// @date last change 2023/04/09
public class SteppingButton : MonoBehaviourPunCallbacks, IMission, IPunObservable
{
    #region fields
    [SerializeField] Transform obj1; // moving object
    [SerializeField] Transform obj2;
    [SerializeField] Transform way1; // waypoint
    [SerializeField] Transform way2;
    [SerializeField] float duration = 1.0f;
    [SerializeField] Ease easeType = Ease.OutQuad;
    Vector3 orgPos1;  // origin position
    Vector3 orgPos2;
    Vector3 wayPos1;
    Vector3 wayPos2;
    Sequence seq1; // motion sequence
    Sequence seq2;
    Animator animObj; // button animation
    MissionInPlayer missionPlayer;

    bool performed;
    bool previous;
    #endregion

    #region property
    /// <summary>
    /// performed 값이 true가 되면 플랫폼 이동함수가 호출된다.
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
            if (previous == performed)
                return;

            if (performed)
                AudioManager.Instance.AddSfxSoundData(SFXClip.Button, false, transform.position);

            MoveStepping(performed);
            previous = performed;
        } 
    }

    #endregion

    #region methods
    /// <summary>
    /// 플레이어가 미션오브젝트의 트리거에 진입했을 때, MissionInPlayer 컴포넌트를 참조하여 
    /// 키입력 여부를 UpdateWork()에서 참조할 수 있게 한다.
    /// </summary>
    /// <param name="player">플레이어의 미션오브젝트 컴포넌트</param>
    /// <param name="type">트리거 종류(enter/stay/exit)</param>
    void IMission.ActivatePuzzle(MissionInPlayer player, int type)
    {
        if (type == (int)TriggerType.Exit) // [trigger exit?]
        {
            this.missionPlayer = null;
        }
        else if (type == (int)TriggerType.Enter && player != null) // [trigger enter?]
        {
            this.missionPlayer = player;
            photonView.ControllerActorNr = player.photonView.ControllerActorNr; // change photon-view control to me
        }
    }
    /// <summary>
    /// 버튼을 누르고 있는 동안에만 플랫폼이 경유지로 이동하게 하며
    /// 손을 떼면 플랫폼을 다시 원위치시킨다.
    /// </summary>
    void MoveStepping(bool performed)
    {
        if (animObj != null)
            animObj.SetBool("Collision", performed); // button anim.

        if (obj1 == null || obj2 == null) 
            return;

        seq1 = DOTween.Sequence();
        seq2 = DOTween.Sequence();
        if (performed)
        {
            seq1.Append(obj1.DOMove(wayPos1, duration).SetEase(easeType)); // odd-numbered objects
            seq2.Append(obj2.DOMove(wayPos2, duration).SetEase(easeType)); // even-numbered objects
        }
        else
        {
            seq1.Append(obj1.DOMove(orgPos1, duration).SetEase(easeType));
            seq2.Append(obj2.DOMove(orgPos2, duration).SetEase(easeType));
        }
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
        TryGetComponent<Animator>(out animObj);
    }
    private void Start()
    {
        orgPos1 = obj1.position; // save origin position
        orgPos2 = obj2.position;
        wayPos1 = way1.position;
        wayPos2 = way2.position;
    }
    private void UpdateWork()
    {
        if (missionPlayer == null || !photonView.IsMine) 
            return;
        this.Performed = missionPlayer.ButtonPerformed; // enable key-input 
    }
    public override void OnEnable()
    {
        UpdateManager.SubscribeToUpdate(UpdateWork);
    }
    public override void OnDisable()
    {
        UpdateManager.UnsubscribeFromUpdate(UpdateWork);
    }
    #endregion

    void IMission.ActivatePuzzleTogether(MissionInPlayer player, bool enter)
    {
        return;
    }
}