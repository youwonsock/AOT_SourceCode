using UnityEngine;
/// <summary>
/// 애니메이션을 실행하는 트리거 미션이지만, 
/// 상대 플레이어와 동기화하지 않게 하는 컴포넌트
/// </summary>
/// <remarks>
/// IMission 인터페이스를 상속받아 ActivatePuzzle의 실제 동작을 구현
/// </remarks>
public class AsyncAnimMission : MonoBehaviour, IMission
{
    #region fields

    Animator animObj;   // Animation
    bool performed;
    [SerializeField] bool isOneWay; 

    #endregion

    #region properties

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
    void IMission.ActivatePuzzle(MissionInPlayer player, int type)
    {
        if (type == (int)TriggerType.Exit && !isOneWay) // [trigger exit?]
            this.Performed = false;
        else if (type == (int)TriggerType.Enter) // [trigger enter?]
            this.Performed = true;
    }
    /// <summary>
    /// 트리거 액션 함수
    /// </summary>
    void Action(bool val)
    {
        if (animObj != null)
            animObj.SetBool("Collision", val); // play animation
    }

    #endregion

    #region unity events

    private void Awake()
    {
        TryGetComponent<Animator>(out animObj);
    }
    void IMission.ActivatePuzzleTogether(MissionInPlayer player, bool enter)
    {
        return;
    }

    #endregion
}
