using System;
using UnityEngine;

/// <summary>
/// Unity의 Update 이벤트를 관리하는 Class
/// </summary>
/// <remarks>
/// UpdateManager는 최적화를 위해서 Update 호출을 한 곳에서 실행하기 위해 만들어진 Class입니다.\n
/// 각 UpdateEvent에 대응되는 3개의 이벤트를 가지고 있으며, 컴포넌트의 OnEnable, OnDisable에서 구독과 구독 해제를 진행해줍니다.
/// </remarks>
/// 
/// @author yws
/// @date last change 2022/10/06
public class UpdateManager : MonoBehaviour {

    #region Fields

    private static event Action OnUpdate;
    private static event Action OnFixedUpdate;
    private static event Action OnLateUpdate;

    #endregion



    #region Funtions
    /// <summary>
    /// OnUpdate Event 구독 메서드
    /// </summary>
    /// <param name="callback">구독시킬 메서드</param>
    public static void SubscribeToUpdate(Action callback)
    {
        OnUpdate += callback;
    }

    /// <summary>
    /// OnFixedUpdate 구독 메서드
    /// </summary>
    /// <param name="callback">구독시킬 메서드</param>
    public static void SubscribeToFixedUpdate(Action callback)
    {
        OnFixedUpdate += callback;
    }

    /// <summary>
    /// OnLateUpdate 구독 메서드
    /// </summary>
    /// <param name="callback">구독시킬 메서드</param>
    public static void SubscribeToLateUpdate(Action callback)
    {
        OnLateUpdate += callback;
    }

    /// <summary>
    /// OnUpdate 구독 해제 메서드
    /// </summary>
    /// <param name="callback">구독 해제시킬 메서드</param>
    public static void UnsubscribeFromUpdate(Action callback)
    {
        OnUpdate -= callback;
    }

    /// <summary>
    /// OnFixedUpdate 구독 해제 메서드
    /// </summary>
    /// <param name="callback">구독 해제시킬 메서드</param>
    public static void UnsubscribeFromFixedUpdate(Action callback)
    {
        OnFixedUpdate -= callback;
    }

    /// <summary>
    /// OnLateUpdate 구독 해제 메서드
    /// </summary>
    /// <param name="callback">구독 해제시킬 메서드</param>
    public static void UnsubscribeFromLateUpdate(Action callback)
    {
        OnLateUpdate -= callback;
    }
    #endregion



    #region UnityEvent
    void Update()
    {
        if (OnUpdate != null)
            OnUpdate.Invoke();
    }

    private void FixedUpdate()
    {
        if (OnFixedUpdate != null)
            OnFixedUpdate.Invoke();
    }

    private void LateUpdate()
    {
        if (OnLateUpdate != null)
            OnLateUpdate.Invoke();
    }
    #endregion
}