using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputSystem을 통해 플레이어 입력을 받는 스크립트
/// </summary>
/// <remarks>
/// 입력을 받아서 플레이어에게 전달하는 역할입니다.
/// </remarks>
/// @date last change 2023/03/02
/// @author yws
/// @class PlayerInput
public class P_Input : MonoBehaviour
{
    #region Fields

    [SerializeField] private Vector2 move;
    [SerializeField] private Vector2 look;
    [SerializeField] private bool jump;
    [SerializeField] private bool sprint;
    [SerializeField] private bool use;
    [SerializeField] private bool mouseLeft;
    [SerializeField] private bool mouseRight;
    [SerializeField] private bool hook;
    [SerializeField] private bool holdStarted;
    [SerializeField] private bool holdCanceled;
    [SerializeField] private bool buttonStarted;
    [SerializeField] private bool buttonCanceled;

    private bool previousJump;
    private bool previousUse;
    private bool previousHook;

    private bool PBS; // previous button started
    private bool PBC; // previous button canceled
    private bool PHS; // previous hold started
    private bool PHC; // previous hold canceled

    #endregion

    #region Property

    public Vector2 Move { get { return move; } }
    public Vector2 Look { get { return look; } }
    public bool Jump
    {
        get
        {
            jump = previousJump == jump ? false : true;
            previousJump = jump;

            return jump;
        }
    }
    public bool Sprint { get { return sprint; } }
    public bool Use
    {
        get
        {
            use = previousUse == use ? false : true;
            previousUse = use;

            return use;
        }
    }
    public bool MouseLeft { get { return mouseLeft; } }
    public bool MouseRight { get { return mouseRight; } }
    public bool Hook
    {
        get
        {
            hook = previousHook == hook ? false : true;
            previousHook = hook;

            return hook;
        }
    }
    public bool ButtonStarted {
        get
        {
            buttonStarted = PBS == buttonStarted? false : true;
            PBS = buttonStarted;
            return buttonStarted;
        }
    }
    public bool ButtonCanceled {
        get
        {
            buttonCanceled = PBC == buttonCanceled ? false : true;
            PBC = buttonCanceled;
            return buttonCanceled;
        }
    }
    public bool HoldStarted
    {
        get
        {
            holdStarted = PHS == holdStarted ? false : true;
            PHS = holdStarted;
            return holdStarted;
        }
    }
    public bool HoldCanceled
    {
        get
        {
            holdCanceled = PHC == holdCanceled ? false : true;
            PHC = holdCanceled;
            return holdCanceled;
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// 마우스 좌클릭 입력을 받는 함수
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseLeft(InputAction.CallbackContext context)
    {
        mouseLeft = context.performed;
    }

    /// <summary>
    /// 마우스 우클릭 입력을 받는 함수
    /// </summary>
    /// <param name="context"></param>
    public void OnMouseRight(InputAction.CallbackContext context) 
    {
        mouseRight = context.started;
    }

    /// <summary>
    /// 키보드로 상하좌우 움직임을 받는 함수입니다
    /// </summary>
    /// <param name="value"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// shift키를 누르는 여부에 따라 점프 여부를 받는 함수입니다
    /// </summary>
    /// <param name="value"></param>
    public void OnJump(InputAction.CallbackContext context)
    {
        jump = context.started;
    }

    /// <summary>
    /// 마우스 움직임에 따라 입력을 받는 함수입니다
    /// </summary>
    /// <param name="value"></param>
    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 스페이스바를 누르는 여부에 따라 달리기 여부를 받는 함수입니다.
    /// </summary>
    /// <param name="value"></param>
    public void OnSprint(InputAction.CallbackContext context)
    {
        sprint = context.performed;
    }

    public void OnUse(InputAction.CallbackContext context)
    {
        use = context.started;
    }

    /// <summary>
    /// F키를 누르면 호출되는 함수로, 미션을 수행할 수 있습니다.
    /// </summary>
    /// <remarks>
    /// Action Type = Button, Interactions = Press, Trigger Behavior = Release Only
    /// </remarks>
    public void OnButton(InputAction.CallbackContext context)
    {
        //button = context.performed;
        buttonStarted = context.started;    // F키를 누를 동안 계속 호출
        buttonCanceled = context.canceled;  // F키를 떼고 나서 계속 호출
    }

    /// <summary>
    /// 마우스 오른쪽 키를 누르고 있을 때 호출되며, 이 함수를 통해 열쇠 오브젝트를 들고 내릴 수 있습니다.
    /// </summary>
    /// <remarks>
    /// Action Type = Button, Interactions = Press, Trigger Behavior = Release Only
    /// </remarks>
    public void OnHold(InputAction.CallbackContext context)
    {
        holdStarted = context.started;
        holdCanceled = context.canceled;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void OnHook(InputAction.CallbackContext context)
    {
        hook = context.started;
    }

    #endregion
}