using UnityEngine;
using Photon.Pun;
using System.Linq.Expressions;

/// <summary>
/// 플레이어 이동과 관련된 스크립트
/// </summary>
/// <remarks>
/// 입력에 따른 이동을 담당하는 Move 함수, 
/// 입력에 따른 점프를 담당하는 Jump 함수,
/// 지면에 닿았는지 체크하는 GroundedCheck 함수 이렇게 총 4개의 함수로 구성되어 있습니다.
/// </remarks>
///
/// @date last change 2023/05/20
/// @author yws
/// @class Movement
public class Movement : MonoBehaviourPunCallbacks
{
    #region Fields

    //- Private -
    [SerializeField] private float walkSpeed = 40;
    [SerializeField] private float sprintSpeed = 50;
    [SerializeField] private float jumpForce = 7;
    [SerializeField] private float gravity = 2f;
    [SerializeField] private int maxJumpCount = 2;
    [Tooltip("이동 속도 변화율")]
    [SerializeField] private float speedChangeRate = 10f;
    [Tooltip("회전 속도")]
    [SerializeField] private float _rotationVelocity;
    [Tooltip("방향 전환 시간")]
    [Range(0.0f, 0.3f)][SerializeField]private float RotationSmoothTime = 0.12f;
    [SerializeField]private int remainJump = 0;

    private float currentSpeed = 0;
    private float maxSpeed = 0;
    private float _animationBlend;
    [SerializeField]private float moveSpeed;
    private float inputMagnitude;
    private float rotation;

    private Rigidbody rigid;
    private Animator animator;


    //- Public -

    #endregion

    


    #region Property
    //- Public -
    public bool Grounded { get; private set; }

    public bool OnGrappling { get; set; }

    #endregion



    #region Methods
    //- Private -

    /// <summary>
    /// 걷기와 달리기를 기능 메서드
    /// </summary>
    private void WalkAndSprint(P_Input input, bool TPV = true)
    {
        float lastFrameSec = Time.deltaTime;

        currentSpeed = new Vector3(rigid.velocity.x, 0, rigid.velocity.z).magnitude;
        inputMagnitude = input.Move.magnitude;

        maxSpeed = input.Sprint ? sprintSpeed : walkSpeed;
        maxSpeed = input.Move == Vector2.zero ? 0 : maxSpeed;

        _animationBlend = Mathf.Lerp(_animationBlend, maxSpeed, lastFrameSec * speedChangeRate);
        _animationBlend = _animationBlend < 0.01f ? 0 : _animationBlend;

        // 이동 속도 설정
        if (currentSpeed < maxSpeed - 0.1f || currentSpeed > maxSpeed + 0.1f)
        {
            // creates curved result rather than a linear one giving a more organic speed change
            // note T in Lerp is clamped, so we don't need to clamp our speed
            moveSpeed = Mathf.Lerp(currentSpeed, maxSpeed * inputMagnitude,
                lastFrameSec*speedChangeRate);

            // round speed to 3 decimal places
            moveSpeed = Mathf.Round(moveSpeed * 1000f) / 1000f;
        }
        else
            moveSpeed = maxSpeed;

        // 회전 설정
        rotation = Mathf.Atan2(input.Move.x, input.Move.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        if(TPV && input.Move != Vector2.zero)
            rigid.rotation = Quaternion.Euler(0.0f,Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref _rotationVelocity, RotationSmoothTime), 0.0f);

        rigid.MovePosition(transform.position + (Quaternion.Euler(0.0f, rotation, 0.0f) * Vector3.forward).normalized * (moveSpeed * lastFrameSec));

        animator.SetFloat(GameManager.animIDSpeed, _animationBlend);
        animator.SetFloat(GameManager.animIDMotionSpeed, inputMagnitude);
    }

    /// <summary>
    /// 점프 기능 메서드
    /// </summary>
    private void Jump(P_Input input)
    {
        if(input.Jump && remainJump > 0)
        {
            animator.SetBool(GameManager.animIDJump, true);
            animator.SetBool(GameManager.animIDGrounded, false);
            rigid.velocity = new Vector3(rigid.velocity.x, 0f, rigid.velocity.z);
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            --remainJump;
        }
        else if (!Grounded)
        {
            animator.SetBool(GameManager.animIDFreeFall, true);
            rigid.AddForce(new Vector3(0f, -1f, 0f) * gravity, ForceMode.Force);
        }

    }

    //- Public -

    /// <summary>
    /// 플레이어 이동 메서드
    /// </summary>
    /// <param name="input">PlayerInput Component</param>
    /// <param name="animator">Animator Component</param>
    public void Move(P_Input input, bool TPV = true)
    {
        WalkAndSprint(input, TPV);
        Jump(input);
    }

    #endregion



    #region UnityEvent

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3 && photonView.IsMine)
        {
            Grounded = true;
            remainJump = maxJumpCount;

            animator.SetBool(GameManager.animIDGrounded, true);
            animator.SetBool(GameManager.animIDJump, false);
            animator.SetBool(GameManager.animIDFreeFall, false);

            if(!OnGrappling)
                rigid.velocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 3 && photonView.IsMine)
        {
            Grounded = true;

            Vector3 t = rigid.velocity;
            t.x = 0;
            t.z = 0;
            
            if(!OnGrappling)
                rigid.velocity = t;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3 && photonView.IsMine)
        {
            Grounded = false;

            animator.SetBool(GameManager.animIDGrounded, false);
        }
    }

    private void Awake()
    {
        if (!photonView.IsMine)
            return;

        TryGetComponent<Rigidbody>(out rigid);
        TryGetComponent<Animator>(out animator);
        remainJump = 2;
    }

    #endregion

}
