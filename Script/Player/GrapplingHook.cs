using Cinemachine.Utility;
using Cysharp.Threading.Tasks;
using Kinemation.FPSFramework.Runtime.Core;
using Photon.Pun;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 플레이어의 GrapplingHook 클래스
/// </summary>
/// <remarks>
/// 플레이어 Prefab의 하위 객체로 부착되어 기능을 수행합니다.
/// </remarks>
/// 
/// @author yws
/// @date last change 2023/05/20
[RequireComponent(typeof(LineRenderer))]
public partial class GrapplingHook : MonoBehaviourPunCallbacks
{

    #region Fields

    //- Private -

    [Header("Gun Setting")]
    [SerializeField] private Transform leftArm;
    [SerializeField] private Transform rightArm;
    
    [SerializeField] private Transform firePos;
    [SerializeField] private float grapplingDistance = 100;
    [SerializeField] private float tutorialDistance;
    [SerializeField] private float stage1Distance;
    [SerializeField] private float stage2Distance;

    private IChangeView playerCamera;
    private SpringJoint springJoint;
    private CancellationTokenSource _source;

    private bool onGrappling;
    [SerializeField] private bool hookActive;
    private Vector3 destination;

    //- Public -

    public WeaponAnimData gunData;

    #endregion



    #region Property

    //- Public -

    public bool OnGrappling { get { return onGrappling; } }
    public bool HookActive { get { return hookActive; } }

    #endregion



    #region Methods
    //- Private -

    /// <summary>
    /// GrapplingHook의 목표지점 설정 메서드
    /// </summary>
    /// <returns></returns>
    private bool SetGrapplingPoint()
    {
        // camera ray
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, grapplingDistance, LayerMask.GetMask("AimPoint")))
        {
            destination = hit.point;
            
            // player ray
            if (Physics.Raycast(firePos.position, destination - firePos.position, grapplingDistance))
            {
                onGrappling = true;

                AudioManager.Instance.AddSfxSoundData(SFXClip.GrapplingHook, false, transform.position);

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Swing 메서드
    /// </summary>
    private void Swing()
    {
        if(SetGrapplingPoint())
        { 
            springJoint = transform.root.gameObject.AddComponent<SpringJoint>();
            springJoint.autoConfigureConnectedAnchor = false;
            springJoint.connectedAnchor = destination;

            float dis = Vector3.Distance(firePos.position, destination);

            springJoint.maxDistance = dis * 0.6f;
            springJoint.minDistance = dis * 0.25f;
            springJoint.spring = 4.5f;
            springJoint.damper = 7;
            springJoint.massScale = 4.5f;

            // sync by rpc
            photonView.RPC(nameof(SyncState), RpcTarget.OthersBuffered, onGrappling, destination);
        }
    }

    /// <summary>
    /// 로프를 발사한 위치로 점프하는 메서드
    /// </summary>
    /// <param name="rigidbody"> Player RigidBody </param>
    /// <returns></returns>
    private async UniTaskVoid Hook(Rigidbody rigidbody)
    {
        if (SetGrapplingPoint())
        {
            // sync by rpc
            photonView.RPC(nameof(SyncState), RpcTarget.OthersBuffered, onGrappling, destination);

            Vector3 lowestPoint = transform.position;

            float yPosGap = destination.y - lowestPoint.y;
            float highestPoint = yPosGap + 2;

            if (yPosGap < 0)
                highestPoint = 2;

            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            rigidbody.velocity = Vector3.zero;
            Vector3 velocity = CalculateJumpVelocity(transform.root.position, destination, highestPoint);

            if (!velocity.IsNaN())
                rigidbody.velocity = velocity;
            else
            {
                EndGrappling();
                return;
            }

            float startTime = Time.time;
            await UniTask.WaitUntil(() => CheckLeftHookDistance(startTime));

            rigidbody.velocity /= 3;

            EndGrappling();
        }
    }

    /// <summary>
    /// 남은 거리 계산 메서드
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private bool CheckLeftHookDistance(float time)
    {
        if (Vector3.Distance(transform.position, destination) < 4 || Time.time -time > 2.5f)
            return true;

        return false;
    }

    /// <summary>
    /// 점프 벡터 계산 메서드
    /// </summary>
    /// <param name="startPoint"> 시작 위치 </param>
    /// <param name="endPoint"> 도착 위치 </param>
    /// <param name="trajectoryHeight"> 최대 높이 </param>
    /// <returns></returns>
    private Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));
        
        return (velocityXZ + velocityY) * 1f;
    }

    /// <summary>
    /// Grappling 종료 메서드
    /// </summary>
    private void EndGrappling()
    {
        onGrappling = false;

        if (springJoint != null)
        {
            Destroy(springJoint);
            springJoint= null;
        }

        transform.localEulerAngles = new Vector3(-85, 90, 0);

        //sync by rpc
        photonView.RPC(nameof(SyncState), RpcTarget.OthersBuffered, onGrappling, destination);
    }

    [PunRPC]
    private void SyncState(bool onGrappling, Vector3 destination) 
    {
        this.onGrappling = onGrappling;
        this.destination = destination;
        this.transform.localEulerAngles = new Vector3(-85, 90, 0);
    }


    /// <summary>
    /// 씬 전환시 실행되는 메서드
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Tutorial":
                grapplingDistance = tutorialDistance;
                break;
            case "Stage_1":
                grapplingDistance = stage1Distance;
                break;
            case "Stage_2":
                grapplingDistance = stage2Distance;
                break;
        }
    }


    //- Public -

    /// <summary>
    /// Grappling 메서드
    /// </summary>
    /// <param name="input"> Player Input </param>
    /// <param name="rigid"> Player RigidBody </param>
    public void Grappling(P_Input input, Rigidbody rigid, bool isGround)
    {
        if (!hookActive)
            return;

        if (isGround)
        {
            if (input.MouseLeft && !onGrappling)
                Swing();
            else if (input.MouseRight && !onGrappling)
                Hook(rigid).Forget();
        }

        if (springJoint != null)
            if (!input.MouseLeft)
                EndGrappling();
    }

    /// <summary>
    /// GrapplingHook 활성화 메서드
    /// </summary>
    /// <returns></returns>
    [SerializeField]private bool delay;
    public async UniTaskVoid ChangeHookState()
    {
        if (delay)
            return;

        delay = true;
        if (hookActive)
        {
            if (playerCamera == null)
            {
                var t = transform.root.GetComponentsInChildren<Player>();
                playerCamera = t[0].GetComponent<PhotonView>().IsMine ? t[0].transform.GetComponent<IChangeView>() : t[1].transform.GetComponent<IChangeView>();
            }

            EndGrappling();
            this.gameObject.SetActive(false);
            hookActive = false;
            playerCamera.ChangeView(3);
        }
        else
        {
            if (playerCamera == null)
            {
                var t = transform.root.GetComponentsInChildren<Player>();
                playerCamera = t[0].GetComponent<PhotonView>().IsMine ? t[0].transform.GetComponent<IChangeView>() : t[1].transform.GetComponent<IChangeView>();
            }


            hookActive = true;

            playerCamera.ChangeView(1);

            this.gameObject.SetActive(true);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(1));
        delay = false;
    }

    #endregion



    #region UnityEvent

    private void Awake()
    {
        TryGetComponent<LineRenderer>(out lineRenderer);

        // Rope
        spring = new Spring();
        spring.SetTarget(0);

        if(photonView.IsMine)   
            transform.root.TryGetComponent<IChangeView>(out playerCamera);

        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        GameManager.Instance.AddOnSceneLoaded(OnSceneLoaded);
    }

    public override void OnEnable()
    {
        base.OnEnable();

        if (_source != null)
            _source.Dispose();

        _source = new CancellationTokenSource();

        transform.localEulerAngles = new Vector3(-85,90,0);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (_source != null)
            _source.Cancel();
    }

    private void OnDestroy()
    {
        if (_source != null)
        {
            _source.Cancel();
            _source.Dispose();
        }

        GameManager.Instance.RemoveOnSceneLoaded(OnSceneLoaded);
    }
    #endregion
}

public partial class GrapplingHook
{
    private LineRenderer lineRenderer;
    private Spring spring;
    private Vector3 currentGrapplePos;

    [Header("Rope Settings")]
    [SerializeField] private int quality = 200; // how many segments the rope will be split up in
    [SerializeField] private float damper = 14; // this slows the simulation down, so that not the entire rope is affected the same
    [SerializeField] private float strength = 800; // how hard the simulation tries to get to the target point
    [SerializeField] private float velocity = 15; // velocity of the animation
    [SerializeField] private float waveCount = 3; // how many waves are being simulated
    [SerializeField] private float waveHeight = 7;
    [SerializeField] private AnimationCurve affectCurve;
    
    private void DrawRope()
    {
        if (!onGrappling)
        {
            currentGrapplePos = firePos.position;

            spring.Reset();
            
            if(lineRenderer.positionCount > 0)
                lineRenderer.positionCount = 0;

            return;
        }

        if(lineRenderer.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lineRenderer.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        Vector3 up = Quaternion.LookRotation((destination - firePos.position).normalized) * Vector3.up;

        currentGrapplePos = Vector3.Lerp(currentGrapplePos, destination, Time.deltaTime*8);
       
        for (int i = 0; i < quality + 1; i++)
        {
            float delta = i / (float)quality;
            Vector3 offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta);

             lineRenderer.SetPosition(i, Vector3.Lerp(firePos.position, currentGrapplePos, delta) + offset);
        }

        if(photonView.IsMine)
            transform.rotation = Quaternion.LookRotation(destination - firePos.position);
    }

    private void Update()
    {
        DrawRope();
    }
}

public class Spring
{
    // values explained in the GrapplingRope_MLab script
    private float strength;
    private float damper;
    private float target;
    private float velocity;
    private float value;

    public void Update(float deltaTime)
    { 
        // calculate the animation values using some formulas I don't understand :D
        var direction = target - value >= 0 ? 1f : -1f;
        var force = Mathf.Abs(target - value) * strength;
        velocity += (force * direction - velocity * damper) * deltaTime;
        value += velocity * deltaTime;
    }

    public void Reset()
    {
        // reset values
        velocity = 0f;
        value = 0f;
    }

    /// here you'll find all functions used to set the variables of the simulation
    #region Setters

    public void SetValue(float value)
    {
        this.value = value;
    }

    public void SetTarget(float target)
    {
        this.target = target;
    }

    public void SetDamper(float damper)
    {
        this.damper = damper;
    }

    public void SetStrength(float strength)
    {
        this.strength = strength;
    }

    public void SetVelocity(float velocity)
    {
        this.velocity = velocity;
    }

    public float Value => value;

    #endregion
}
