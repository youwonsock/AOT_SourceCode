using DG.Tweening;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// 플레이어가 미션 존에 진입하면, 미션을 수행하는 클래스
/// </summary>
/// <remarks>
/// IMission 인터페이스를 상속받아 ActivatePuzzle의 실제 동작을 구현
/// </remarks>
/// @author 이은수
/// @date last change 2023/04/09
public class TriggerMission : MonoBehaviourPunCallbacks, IMission, IPunObservable
{
    #region fields
    Animator animObj;   // Animation
    Light lightObj;     // Light Effect
    [SerializeField] Collider colliderObj; // next step의 collider
    [SerializeField] bool barrier = false;

    [SerializeField] bool isOneWay;
    [SerializeField] Transform target;  // Moving Transform 
    [SerializeField] Transform waypoint;
    [SerializeField] float duration;
    [SerializeField] Ease easyType = Ease.InOutQuad;
    Sequence seq;
    Vector3 originPos;
    Vector3 waypointPos;

    bool performed;
    #endregion

    #region properties
    /// <summary>
    /// performed 값이 true가 되면 트리거이벤트의 액션함수가 호출된다.
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
            if (performed)
                this.Action(true);
            else if (!isOneWay)
                this.Action(false);
        }
    }
    #endregion

    #region methods
    /// <summary>
    /// 미션오브젝트의 트리거와 충돌하면 Performed를 통해 액션을 수행할 수 있게 한다.
    /// </summary>
    /// <param name="type">트리거 종류(enter/stay/exit)</param>
    void IMission.ActivatePuzzle(MissionInPlayer player, int type)
    {
        if (type == (int)TriggerType.Exit && !isOneWay) // [trigger exit?]
        {
            this.Performed = false;
        }
        else if (type == (int)TriggerType.Enter) // [trigger enter?]
        {
            photonView.ControllerActorNr = player.photonView.ControllerActorNr; // change photon-view control to me
            this.Performed = true;
        }
    }
    /// <summary>
    /// 트리거 액션 함수
    /// </summary>
    /// <param name="val">트리거 여부</param>
    void Action(bool val)
    {
        if (colliderObj != null)
            colliderObj.enabled = true;

        if (animObj != null)
            animObj.SetBool("Collision", val);

        if (lightObj != null)
            lightObj.enabled = val;

        if (val && target != null && waypoint != null)
        {
            MoveTarget();
            if (barrier)
                AudioManager.Instance.AddSfxSoundData(SFXClip.Wheel, false, transform.position);
        }
    }

    /// <summary>
    /// 타겟을 이동시키는 함수
    /// </summary>
    /// <param name="origin">타겟의 처음위치</param>
    /// <param name="waypoint">타겟의 경유지</param>
    void MoveTarget()
    {
        seq = DOTween.Sequence();
        seq.Append(target.DOMove(waypointPos, duration).SetEase(easyType));   // move to waypoint

        if (!isOneWay)
            seq.Append(target.DOMove(originPos, duration).SetEase(easyType)); // return to origin 
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

        if (target == null || waypoint == null)
            return;

        originPos = target.position;
        waypointPos = waypoint.position;
    }
    void IMission.ActivatePuzzleTogether(MissionInPlayer player, bool enter)
    {
        return;
    }
    #endregion
}