using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

/// <summary>
/// 투사체 dispenser 클래스
/// </summary>
/// <remarks>
/// ProjectileDispenser는 투사체 함정에서 사용되는 투사체 dispenser를 구현하는 클래스입니다.
/// 투사체를 발사할 오브젝트에 컴포넌트로 부착하여 값을 세팅해주면 오브젝트에서 투사체가 발사됩니다.
/// 
/// 함정의 트리거인 발판과 기타 기능은 아직 미완성이며
/// interface를 통해 다른 컴포넌트에서 참조해 사용가능하도록 구현 예정입니다.
/// 
/// </remarks>
/// 
/// @author yws
/// @date last change 2023/01/05
public class ProjectileDispenser : MonoBehaviour, IActivable
{

    #region Fields

    [SerializeField] private ProjectileDispenser_SO projectileDispenserData;
    private CancellationTokenSource _source;

    // 작동 여부 판단용 flag
    private bool isActive;

    #endregion



    #region Methods

    private async UniTaskVoid FireProjectile(Vector3 fireDir)
    {
        isActive = true;
        int i = projectileDispenserData.RepeatingTime;
        var t = TimeSpan.FromSeconds(projectileDispenserData.FireInterval);

        while (--i > 0 || projectileDispenserData.InfiniteRepeating)
        {
            await UniTask.Delay(t);

            ObjectPool.Instance.GetProjectile(projectileDispenserData.ProjectileType)
                .SetProjectile(fireDir, transform.position);
        }
    }

    /// <summary>
    /// 외부 트리거에 의한 활성화 시 호출되는 메서드
    /// </summary>
    /// <param name="triggerPos">트리거의 위치</param>
    public void ActivateObject(Vector3 triggerPos)
    {
        if (!isActive)
            FireProjectile(triggerPos - transform.position).Forget(); // trigger 방향으로 투사체 발사
    }

    #endregion



    #region Unity Event

    private void OnEnable()
    {
        if (_source != null)
            _source.Dispose();

        _source = new CancellationTokenSource();

        if (projectileDispenserData.FireOnStart)
            FireProjectile(transform.forward).Forget(); // trigger 방향으로 투사체 발사
    }

    private void OnDisable()
    {
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
    }


    #endregion
}