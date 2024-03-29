using Photon.Pun;
using UnityEngine;

/// <summary>
/// 외부에서 문을 여는 함수가 호출되면, 리지드바디와 힌지조인트를 사용하여 잠금문을 연다.
/// </summary>
/// 
/// @author 이은수
/// @date last change 2023/02/14
public class KeyDoor : MonoBehaviourPunCallbacks
{
    #region fields
    Rigidbody rigid;
    new HingeJoint hingeJoint;
    JointLimits JointLimits;
    float currentLimits;

    Animator anim;

    [SerializeField] bool useAnim = false;
    #endregion

    #region property

    #endregion

    #region unity events
    private void Awake()
    {
        TryGetComponent<Rigidbody>(out rigid);
        TryGetComponent<HingeJoint>(out hingeJoint);
        TryGetComponent<Animator>(out anim);
    }

    #endregion

    #region method
    /// <summary>
    /// 지정한 애니메이션을 실행하거나, 물리 엔진을 이용하여 지정한 방향으로 힘을 주어 문을 밀어냄
    /// </summary>
    public void DoorAction()
    {
        if (useAnim && anim != null)
        {
            anim.SetBool("Action", true);
        }
        else if (rigid != null && hingeJoint != null) 
        {
            currentLimits = 85f;
            JointLimits.max = currentLimits;    // upper limit
            JointLimits.min = -currentLimits;   // lower limit
            hingeJoint.limits = JointLimits;    // Set limit
            rigid.AddRelativeForce(new Vector3(0, -1, 0) * 20);
        }
    }
    #endregion

}
