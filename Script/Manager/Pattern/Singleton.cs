using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton패턴 구현을 위한 Class
/// </summary>
/// <remarks>
/// Singleton.cs는 Singleton패턴을 적용할 클래스의 부모 클래스로\n
/// 자식 오브젝트는 protected ChildClass() { }를 선언하여 자신의 instancd를 미리 초기화 해줍니다.
/// </remarks>
/// <typeparam name="T">생성할 Singleton Type</typeparam>
/// 
/// @author yws
/// @date last change 2023/04/05
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    #region Fields
    private static bool isShutDown = false;
    private static object m_Lock = new object();
    private static T instance;

    protected event Action OnAwakeEvent;
    protected event Action OnDestroyedEvent;
    #endregion



    #region Funtions
    /// <summary>
    /// instance 반환 Property
    /// </summary>
    public static T Instance
    {
        get
        {
            //// play 버튼 클릭시 OnApplicationQuit이벤트가 호출되는 버그가 있음!
            //if (isShutDown)
            //{
            //    Debug.LogWarning(" allication is quit");
            //    return null;
            //}

            lock (m_Lock)
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null)
                    {
                        GameObject gameObject = new GameObject(""+typeof(T));
                        instance = gameObject.AddComponent<T>();
                    }
                }

                return instance;
            }
        }
    }

    /// <summary>
    /// 자식 클래스에서 재정의하여 Awake에서 실행할 작업을 구현하는 메서드
    /// </summary>
    protected virtual void OnAwakeWork() { }

    /// <summary>
    /// 자식 클래스에서 재정의하여 OnDestroyed에서 실행할 작업을 구현하는 메서드
    /// </summary>
    protected virtual void OnDestroyedWork() { }

    #endregion

    #region Unity Event

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance != null && instance.gameObject != this.gameObject)
        {
            Destroy(this.gameObject);
            return;
        }

        if (OnAwakeEvent != null)
            OnAwakeEvent.Invoke();

        OnAwakeWork();
    }

    private void OnApplicationQuit()
    {
        isShutDown = true;
    }

    private void OnDestroy()
    {
        if (instance == this)
            isShutDown = true;

        if(OnDestroyedEvent != null) 
            OnDestroyedEvent.Invoke();

        OnDestroyedWork();
    }

    #endregion
}
