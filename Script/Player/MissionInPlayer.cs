using Photon.Pun;
using UnityEngine;

/// <summary>
/// 플레이어에 붙이는 미션오브젝트 컴포넌트
/// </summary>
/// <remarks>
/// 미션오브젝트 관련 입력 동작과 트리거 충돌 감지를 수행한다.
/// </remarks>
/// @memo 이 스크립트는 플레이어에 붙여 사용합니다.
/// @author 이은수
/// @date last change 2023/04/03
public class MissionInPlayer : MonoBehaviourPunCallbacks
{
    #region fields

    P_Input input;
    IMission mission;

    bool buttonPerformed;   // input mission
    bool holdPerformed;     // hand mission

    Animator anim;

    #endregion

    #region properties

    public bool ButtonPerformed { get { return buttonPerformed; } set { buttonPerformed = value; } }  // input mission
    public bool HoldPerformed { get { return holdPerformed; } set { holdPerformed = value; } }        // hand mission

    #endregion

    #region unity events

    private void Awake()
    {
        input = GetComponentInParent<P_Input>();
        anim = GetComponentInParent<Animator>();
    }
    /// <summary>
    /// 키의 입력 여부에 따라 변수 조절 
    /// </summary>
    /// <remarks>
    /// 외부에서 입력 여부를 참조하여 미션동작을 수행시킨다. 
    /// 레버와 버튼, 핸드 오브젝트의 수행을 위한 입력으로 사용된다. 
    /// 키를 누르고 있을 때 true, 손을 떼면 false로 전환된다. 
    /// </remarks>
    private void UpdateWork()
    {
        // lever or button input
        if (input.ButtonStarted)
        {
            ButtonPerformed = true;

            // interaction anim
            anim.SetTrigger(GameManager.animIDInteraction);
        }
        else if (input.ButtonCanceled)
        {
            ButtonPerformed = false; 
        }

        // hand(key) object input
        if (input.HoldStarted)
        {
            HoldPerformed = true;
        }
        else if (input.HoldCanceled)
        {
            HoldPerformed = false;
        }
    }

    /// <summary>
    /// 플레이어에 충돌한 미션오브젝트의 종류를 판별하여 실제 미션수행 클래스의 함수를 호출한다.
    /// </summary>
    /// <remarks>
    /// 실제 미션수행 클래스는 IMission 인터페이스를 상속받은 클래스로 ActivatePuzzle()이 각각의 요구사항에 맞게 구현되어 있다.
    /// 충돌한 미션오브젝트의 종류에 따라 달리 구현된 ActivatePuzzle()을 호출한다.
    /// player의 정보와 trigger 진입 종류를 인자로 넘긴다.
    /// </remarks>
    /// @memo ActivatePuzzleTogether()도 동일한 방식
    /// <param name="other">충돌한 미션오브젝트</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IMission>(out mission))
        {
            mission.ActivatePuzzleTogether(this, true);

            if (!photonView.IsMine) return;

            mission.ActivatePuzzle(this, (int)TriggerType.Enter);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IMission>(out mission))
        {
            mission.ActivatePuzzleTogether(this, true);

            if (!photonView.IsMine) return;

            mission.ActivatePuzzle(this, (int)TriggerType.Stay);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IMission>(out mission))
        {
            mission.ActivatePuzzleTogether(this, false);

            if (!photonView.IsMine) return;

            mission.ActivatePuzzle(this, (int)TriggerType.Exit);
        }
    }
    public override void OnEnable()
    {
        if (!photonView.IsMine) return;
        UpdateManager.SubscribeToUpdate(UpdateWork);
    }
    public override void OnDisable()
    {
        UpdateManager.UnsubscribeFromUpdate(UpdateWork);
    }

    #endregion
}
