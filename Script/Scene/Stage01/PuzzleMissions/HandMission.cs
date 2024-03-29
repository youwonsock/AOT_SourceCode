using Photon.Pun;
using UnityEngine;
using System.Collections;

/// <summary>
/// 오른쪽 마우스 버튼을 누르고 있으면 핸드오브젝트를 들고 다닐 수 있게 하는 클래스
/// </summary>
/// <remarks>
/// IMission 인터페이스를 상속받아 ActivatePuzzleTogether의 실제 동작을 구현
/// </remarks>
/// @memo 이 스크립트는 핸드오브젝트에 붙여 사용합니다.
/// @author 이은수
/// @date last change 2023/05/17
public class HandMission : MonoBehaviourPunCallbacks, IMission, IPunObservable
{
    #region fields
    Transform hand;
    MissionInPlayer missionPlayer;
    Animator animObj;
    Vector3 initPos; // initial position

    bool unlock;
    bool canInit; // [can initiate key position?]
    bool performed;
    int actorNr = 0; // PhotonView Control Actor Number
    Collider thisCollider;
    #endregion

    #region property
    /// <summary>
    /// performed 값이 true가 되면 핸드오브젝트 이동함수가 호출된다.
    /// </summary>
    /// <remarks>
    /// 입력이 들어오는 동안 Following 함수를 호출하고, 핸드미션오브젝트의 레이어를 변경합니다.
    /// 레이어가 변경된 미션오브젝트는 오브젝트를 들고 있는 플레이어, 잠금해제장치와만 상호작용합니다.(다른 플레이어 접근 x)
    /// 미션오브젝트를 놓치는 경우 오브젝트의 포지션이 초기화됩니다. 
    /// </remarks>
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
            {
                StartCoroutine(Following());
                
                if (missionPlayer != null)
                {
                    missionPlayer.gameObject.layer = 14;
                    this.gameObject.layer = 14;
                }
                canInit = true; 
            }
            else
            {
                if (canInit && !unlock) // 초기화 가능 상태가 아니거나, 미션이 해결된 경우 아래 구문을 실행하지 않는다
                {
                    photonView.RPC(nameof(InitPosition), RpcTarget.All, null);
                    canInit = false;
                }
            }
            if (animObj != null)
                animObj.SetBool("Collision", !value);
        }
    }
    /// <summary>
    /// 모든 수행이 끝났을 때 핸드오브젝트의 재사용을 방지한다.
    /// </summary>
    public bool Unlock
    {
        set
        {
            unlock = value;
            if (unlock)
            {
                if (missionPlayer != null)
                {
                    missionPlayer.gameObject.layer = 1;
                    this.gameObject.layer = 1;
                }
                if (animObj != null)
                {
                    animObj.SetBool("Collision", false); 
                    animObj.enabled = false;
                }
                thisCollider.enabled = false;
                this.enabled = false;
            }
        }
    }
    #endregion

    #region methods
    /// <summary>
    /// 플레이어가 미션오브젝트의 트리거에 진입했을 때, MissionInPlayer 컴포넌트를 참조하여 
    /// 키입력 여부를 LateUpdateWork()에서 참조할 수 있게 한다. 또한, hand의 위치를 받아 저장한다.   
    /// </summary>
    /// <remarks>
    /// actorNr는 이 포톤뷰를 제어하는 액터의 번호를 의미하며
    /// 0이면 트리거에 들어 온 액터가 없고, 
    /// 0이 아니면 어떤 플레이어가 들어왔다는 뜻이며 다른 플레이어는 미션오브젝트에 접근할 수 없다. 
    /// </remarks>
    /// <param name="player">플레이어의 미션오브젝트 컴포넌트</param>
    /// <param name="type">트리거 종류(enter/stay/exit)</param>
    void IMission.ActivatePuzzleTogether(MissionInPlayer player, bool enter)
    {
        if (!enter) // [trigger exit?]
        {
            player.gameObject.layer = 1;
            this.gameObject.layer = 1;
            this.missionPlayer = null;

            if (player.gameObject.CompareTag("PlayerHand"))
            {
                if (actorNr == player.photonView.ControllerActorNr) // [Now, do I control the photon-view ?]
                    actorNr = 0; // initialize

                if (animObj != null && photonView.IsMine)
                    animObj.SetBool("Collision", false);
            }
        }
        else if (player != null) // [trigger enter & stay?]
        {
            this.missionPlayer = player;

            if (player.gameObject.CompareTag("PlayerHand"))
            {
                if (actorNr != photonView.ControllerActorNr) // [initial state] OR [if the another player exit the trigger]
                {
                    photonView.ControllerActorNr = player.photonView.ControllerActorNr; // change photon-view control to me
                    actorNr = photonView.ControllerActorNr;

                    if (animObj != null && photonView.IsMine)
                        animObj.SetBool("Collision", true);

                    hand = player.gameObject.transform; // set hand pos
                }
            }
        }
    }
    /// <summary>
    /// 미션오브젝트가 플레이어를 따라다니게 하는 함수
    /// </summary>
    IEnumerator Following()
    {
        if (hand != null)
            transform.position = Vector3.Lerp(this.transform.position, hand.position, Time.deltaTime * 10);
        yield return null;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Performed);
            stream.SendNext(actorNr);
        }
        else
        {
            this.Performed = (bool)stream.ReceiveNext();
            this.actorNr = (int)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// 미션오브젝트의 참조정보와 게임 내에서 오브젝트의 위치를 초기화하는 함수
    /// </summary>
    [PunRPC]
    void InitPosition()
    {
        if (photonView.IsMine)
            AudioManager.Instance.AddSfxSoundData(SFXClip.KeyDrop, false, transform.position);

        // initialize
        if (missionPlayer != null)
        {
            missionPlayer.gameObject.layer = 1;
            actorNr = 0;
            hand = null;
            missionPlayer = null;
        }
        this.gameObject.layer = 1;
        this.transform.position = initPos;
    }
    /// <summary>
    /// 플레이어가 데드존에 닿아 이 함수가 외부에서 호출되면, 미션오브젝트 위치 초기화 함수를 실행시킨다.
    /// </summary>
    public void OnDeadzone()
    {
        if (canInit && !unlock) // 초기화 가능 상태가 아니거나, 미션이 해결된 경우 아래 구문을 실행하지 않는다
        {
            photonView.RPC(nameof(InitPosition), RpcTarget.All, null);
            canInit = false;
        }
    }
    #endregion

    #region unity events
    private void Awake()
    {
        animObj = GetComponentInChildren<Animator>();
        TryGetComponent<Collider>(out thisCollider);

        initPos = this.transform.position;
    }
    /// <summary>
    /// trigger enter 시, 키입력 여부에 따라 Performed 프로퍼티를 통해 핸드오브젝트의 이동을 관리한다.
    /// </summary>
    private void LateUpdateWork()
    {
        if (missionPlayer == null || !photonView.IsMine) // [if exit trigger]
            return;    

        if (actorNr == missionPlayer.photonView.ControllerActorNr)  // [Now, do I control the photon-view?]
            this.Performed = missionPlayer.HoldPerformed;           // enable key-input 
    }

    public override void OnEnable()
    {
        UpdateManager.SubscribeToFixedUpdate(LateUpdateWork);
    }

    public override void OnDisable()
    {
        UpdateManager.UnsubscribeFromFixedUpdate(LateUpdateWork);
    }
    #endregion

    void IMission.ActivatePuzzle(MissionInPlayer player, int type)
    {
        return;
    }
}

