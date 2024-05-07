using Photon.Pun;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Kinemation.FPSFramework.Runtime.Core;
using Kinemation.FPSFramework.Runtime.Layers;

/// <summary>
/// Player관리 클래스
/// </summary>
/// <remarks>
/// 
/// </remarks>
///
/// @date last change 2023/05/19
/// @author YWS
/// @class Player
public class Player : MonoBehaviourPunCallbacks, IDamageAble, IPunObservable
{
    #region Fields

    [SerializeField] private Movement movement;
    [SerializeField] private P_Input input;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Inventory inventory;
    [SerializeField] private GrapplingHook grapplingHook;

    [SerializeField] private float MaxHealth = 100f;

    private Rigidbody rigid;
    private Animator animator;
    private Vector3 networkPosition;
    private Vector3 networkLocalPosition;
    private Quaternion networkRotation;

    private float health;

    public Action onDeath;

    private UI_Heart heart;

    public UI_PlayerView _view;
    [SerializeField] private GameObject view;
    private UI_Presenter presenter;

    // FP Anim
    CharAnimData animData;
    CoreAnimComponent coreAnimComponent;

    #endregion



    #region Property

    public Rigidbody Rigid { get { return rigid; } }

    public float Health { get { return health; } set { health = value; } }

    public float maxHealth { get { return MaxHealth; } }


    #endregion



    #region Methods

    /// <summary>
    /// 투사체의 정보를 받아서 투사체의 데미지를 체력에 반영합니다.
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <param name="damage"></param>
    /// <param name="knockbackForce"></param>
    public void TakeDamage(Vector3 hitPoint, float damage, float knockbackForce)
    {
        if (photonView.IsMine)
        {
            AudioManager.Instance.AddSfxSoundData(SFXClip.PlayerDamage, false, transform.position);

            presenter.UpdateHealth(damage);

            rigid.AddForce(new Vector3(0, knockbackForce));

            heart.ChangeHeartImage();
        }
    }

    /// <summary>
    /// 플레이어를 리스폰 포인트로 옮겨주는 함수입니다
    /// </summary>
    public void Respawn()
    {
        photonView.RPC(nameof(RespawnAll), RpcTarget.OthersBuffered);
    }

    /// <summary>
    /// Photon 동기화 함수
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.position);

            stream.SendNext(transform.rotation);
        }
        else
        {
            networkLocalPosition = (Vector3)stream.ReceiveNext();
            networkPosition = (Vector3)stream.ReceiveNext();

            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }


    //---------------------------------------------- Private ------------------------------------------------------

    /// <summary>
    /// 방의 권한을 갖고 있는 호스트가 다른 플레이어와 함께 다음 씬으로 넘어갑니다.
    /// </summary>
    private void Next()
    {
        GameManager.Instance.goal = 0;
        if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name == "Tutorial")
        {
            PhotonNetwork.LoadLevel("Stage_1");
            PlayerPrefs.SetFloat("TutorialClear", 1);
        }
        else if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name == "Stage_1")
        {
            PhotonNetwork.LoadLevel("Stage_2");
            PlayerPrefs.SetFloat("Stage1Clear", 1);
        }
        else if(SceneManager.GetActiveScene().name == "Stage_2")
        {
            photonView.RPC("End", RpcTarget.All, true);
            PlayerPrefs.SetFloat("Stage2Clear", 1);
            PhotonNetwork.LeaveRoom();
        }
    }

    [PunRPC]
    void End(bool go)
    {
        if(go)
            PhotonNetwork.LoadLevel("Ending");
    }

    /// <summary>
    /// OnSceneLoaded
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        onDeath = null;
    }

    /// <summary>
    /// 체력 동기화를 위한 RPC 함수입니다.
    /// </summary>
    /// <param name="newHealth">체력</param>
    [PunRPC]
    private void SyncHealth(float newHealth)
    {
        health = newHealth;
    }

    /// <summary>
    /// Hook 동기화 메서드
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    private void SyncHook(bool value)
    {
        grapplingHook.gameObject.SetActive(value);
    }

    [PunRPC]
    private void RespawnAll()
    {
        var players = FindObjectsOfType<Player>();

        GameManager.Instance.InitRespawnPoint();

        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].photonView.IsMine)
            {
                players[i]._view.UpdateHealthBar(Health, maxHealth);
                continue;
            }

            if (players[i].onDeath != null)
                players[i].onDeath();

            players[i].transform.position = GameManager.Instance.RespawnPoint;
            players[i].Health = MaxHealth;

            players[i].heart.ChangeHeartImage();
        }
    }

    #endregion



    #region UnityEvent

    private void Start()
    {
        // ------------------ui----------------------
        health = MaxHealth;

        //view = GameObject.FindGameObjectWithTag("Heart");
        _view = view.GetComponent<UI_PlayerView>();

        TryGetComponent<UI_Presenter>(out presenter);
        presenter.connect_mvp(_view);
        // --------------ui----------------------------


        this.gameObject.TryGetComponent<Rigidbody>(out rigid);
        TryGetComponent<CoreAnimComponent>(out coreAnimComponent);

        if (!photonView.IsMine)
        {
            Destroy(coreAnimComponent);

            Destroy(GetComponent<LookLayer>());
            Destroy(GetComponent<LeftHandIKLayer>());
            Destroy(GetComponent<SwayLayer>());
            rigid.isKinematic = true;

            return;
        }

        TryGetComponent<Movement>(out movement);
        TryGetComponent<P_Input>(out input);
        TryGetComponent<PlayerCamera>(out playerCamera);
        TryGetComponent<Inventory>(out inventory);
        TryGetComponent<Animator>(out animator);

        coreAnimComponent.OnGunEquipped(grapplingHook.gunData);
        coreAnimComponent.SetLeftHandIKWeight(0, 0);

        GameManager.Instance.AddOnSceneLoaded(OnSceneLoaded);

        rigid.useGravity = true;
        TryGetComponent<UI_Heart>(out heart);
    }

    public override void OnEnable()
    {
        base.OnEnable();

        UpdateManager.SubscribeToFixedUpdate(FixedUpdateWork);

        if (!photonView.IsMine)
            return;

        UpdateManager.SubscribeToUpdate(UpdateWork);
        UpdateManager.SubscribeToFixedUpdate(LateUpdateWork);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        UpdateManager.UnsubscribeFromFixedUpdate(FixedUpdateWork);

        if (!photonView.IsMine)
            return;

        UpdateManager.UnsubscribeFromUpdate(UpdateWork);
        UpdateManager.UnsubscribeFromFixedUpdate(LateUpdateWork);
    }

    private void UpdateWork()
    {
        // item
        if (input.Use && !grapplingHook.OnGrappling)
            inventory.UseItem();
        
        //grappling hook
        if (input.Hook && MathF.Abs(rigid.velocity.y) < 1 && GameManager.Instance.ShaderChange)
        {
            grapplingHook.ChangeHookState().Forget();
            photonView.RPC(nameof(SyncHook), RpcTarget.OthersBuffered, grapplingHook.HookActive);
            animator.SetBool(GameManager.animIDFp, grapplingHook.HookActive);

            if (grapplingHook.HookActive)
                coreAnimComponent.SetLeftHandIKWeight(1, 0);
            else
                coreAnimComponent.SetLeftHandIKWeight(0, 0);
        }
        
        // camera
        playerCamera.CameraRotation(input);

        //Fp anim
        animData.moveInput = input.Move;
        animData.AddAimInput(input.Look * playerCamera.Sensitivitys);
        coreAnimComponent.SetCharData(animData);

        // next point
        if (GameManager.Instance.goal == 2)
            Next();
        else if(GameManager.Instance.goal == 1 && SceneManager.GetActiveScene().name == "Stage_2")
            Next();
    }

    private void FixedUpdateWork()
    {
        // sync position
        if (!photonView.IsMine)
        {
            if (transform.parent != null)
                transform.localPosition = Vector3.Lerp(transform.localPosition, networkLocalPosition, Time.deltaTime * 10);
            else
                transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * 10);

            if (Vector3.Distance(transform.position, networkPosition) > 10)
                transform.position = networkPosition;

            transform.rotation = Quaternion.RotateTowards(transform.rotation, networkRotation, Time.deltaTime * 180.0f);
        }
        else
        {
            // move
            if (!grapplingHook.OnGrappling)
                movement.Move(input, !grapplingHook.HookActive);

            // grappling hook
            grapplingHook.Grappling(input, rigid, movement.Grounded);
            movement.OnGrappling = grapplingHook.OnGrappling;
        }
    }

    private void LateUpdateWork()
    {
    }

    #endregion
}
