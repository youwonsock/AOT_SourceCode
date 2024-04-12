using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/// <summary>
/// 게임의 관리를 위한 기능들을 가지고있는 Class
/// </summary>
/// <remarks>
/// GameManager는 Singleton 클래스로 게임에서 사용되는 전역 변수등을 관리할 예정입니다.
/// </remarks>
/// 
/// @author yws
/// @date last change 2023/05/19
public class GameManager : Singleton<GameManager>
{
    protected GameManager() { }

    #region Fields
    public int goal;
    public int levercnt = 0;
    public bool master_first;
    public bool client_first;
    
    // animation hash values
    public static int animIDSpeed = Animator.StringToHash("Speed");
    public static int animIDGrounded = Animator.StringToHash("Grounded");
    public static int animIDJump = Animator.StringToHash("Jump");
    public static int animIDFreeFall = Animator.StringToHash("FreeFall");
    public static int animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    public static int animIDFp = Animator.StringToHash("FP");
    public static int animIDInteraction = Animator.StringToHash("Interaction");

    private Action activeFPV;
    private Action activeTPV;
    
    private Vector3 respawnPoint = Vector3.zero;
    #endregion


    #region Property

    public Vector3 RespawnPoint { get { return respawnPoint; } set { respawnPoint = value; } }

    public bool ShaderChange { get; set; }
    #endregion



    #region Methods

    /// <summary>
    /// 리스폰 포인트 초기화 메서드
    /// </summary>
    public void InitRespawnPoint()
    {
        var startPointObj = GameObject.FindGameObjectWithTag("StartPoint");

        if (startPointObj != null)
            respawnPoint = startPointObj.transform.position;
    }

    /// <summary>
    /// OnSceneLoaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitRespawnPoint();

        if(scene.name == "Tutorial" || scene.name == "Stage_1"|| scene.name == "Stage_2" || scene.name == "Ending")
        {
            Cursor.lockState= CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    /// <summary>
    /// SceneLoaded Event 등록 메서드
    /// </summary>
    /// <param name="action"> 등록할 Action</param>
    public void AddOnSceneLoaded(UnityAction<Scene, LoadSceneMode> action)
    {
        SceneManager.sceneLoaded += action;
    }

    /// <summary>
    /// SceneLoaded Event 등록 헤제 메서드
    /// </summary>
    /// <param name="action">등록 해제할 Action</param>
    public void RemoveOnSceneLoaded(UnityAction<Scene, LoadSceneMode> action)
    {
        SceneManager.sceneLoaded -= action;
    }

    /// <summary>
    /// 시점 변환 시 실행되는 메서드
    /// </summary>
    /// <param name="n"></param>
    public void OnChangePV(int n)
    {
        if(n == 1)
            activeFPV?.Invoke();
        else if(n == 3)
            activeTPV?.Invoke();
    }

    /// <summary>
    /// 시점 변환 이벤트 등록 메서드
    /// </summary>
    /// <param name="action"></param>
    public void AddChangePVEvent(Action action, int n)
    {
        if (n == 1)
            activeFPV += action;
        else if (n == 3)
            activeTPV += action;
    }

    /// <summary>
    /// 시점 변환 이벤트 제거 메서드
    /// </summary>
    /// <param name="action"></param>
    public void RemoveChangePVEvent(Action action, int n)
    {
        if (n == 1)
            activeFPV -= action;
        else if (n == 3)
            activeTPV -= action;
    }

    #endregion



    #region Unity Event

    /// <summary>
    /// Awake에서 실행할 작업을 구현하는 메서드
    /// </summary>
    protected override void OnAwakeWork()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        //Application.targetFrameRate = 30;
    }

    /// <summary>
    /// OnDestroyed에서 실행할 작업을 구현하는 메서드
    /// </summary>
    protected override void OnDestroyedWork()
    {
        base.OnDestroyedWork();
    }

    // temp !!!!
    private void Start()
    {
        PlayerPrefs.GetFloat("TutorialClear", 0);
        PlayerPrefs.SetFloat("Stage1Clear", 0);
        PlayerPrefs.SetFloat("Stage2Clear", 0);

        ShaderChange = true;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    #endregion
}
