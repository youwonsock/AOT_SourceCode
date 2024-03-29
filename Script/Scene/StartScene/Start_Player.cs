using UnityEngine;
using System;

/// <summary>
/// Game_Start씬에 있는 Player를 별도로 관리하기 위한 클래스
/// </summary>
/// <remarks>
/// 
/// </remarks>
///
/// @date last change 2023/04/19
/// @author LSM
/// @class Start_Player
public class Start_Player : MonoBehaviour
{
    #region Fields

    [SerializeField] private Movement movement;
    [SerializeField] private P_Input input;

    private Rigidbody rigid;
    private Animator animator;
    private Vector3 respawnPosition;
    private Vector3 startPosition;

    public Action onDeath;

    #endregion



    #region Property

    public Rigidbody Rigid { get { return rigid; } }

    public Vector3 RespawnPositon { get { return respawnPosition; } set { respawnPosition = value; } }

    #endregion

    #region UnityEvent

    private void Start()
    {
        this.gameObject.TryGetComponent<Rigidbody>(out rigid);

        startPosition = transform.position;
        respawnPosition = startPosition;

        TryGetComponent<Movement>(out movement);
        TryGetComponent<P_Input>(out input);
        TryGetComponent<Animator>(out animator);

        rigid.useGravity = true;
    }

    void OnEnable()
    {
        UpdateManager.SubscribeToFixedUpdate(FixedUpdateWork);
    }

    void OnDisable()
    {
        UpdateManager.UnsubscribeFromFixedUpdate(FixedUpdateWork);
    }

    private void FixedUpdateWork()
    {
        movement.Move(input, true);
    }

    #endregion
}
