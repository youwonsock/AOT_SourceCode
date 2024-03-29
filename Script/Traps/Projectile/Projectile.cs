using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 투사체 클래스들의 기본기능을 구현해둔 베이스 클래스
/// </summary>
/// <remarks>
/// Projectile_yws는 투사체의 기본 기능을 구현한 베이스 클래스로 컴포넌트로 붙여 바로 사용할수도 있으며 \n
/// 추가적인 기능이 필요한 경우 자식 클래스를 만들어 사용하시면 됩니다.
/// </remarks>
/// 
/// @author yws
/// @date last change 2022/11/22
public class Projectile : MonoBehaviour
{
    #region Fields

    [SerializeField] private Projectile_SO projectileData;

    Rigidbody rigid;

    #endregion



    #region Methods

    /// <summary>
    /// 투사체 세팅 메서드
    /// </summary>
    /// <param name="shootDirection">발사하는 방향</param>
    /// <param name="shootPos">발사하는 위치</param>
    public void SetProjectile(Vector3 shootDirection, Vector3 shootPos)
    {
        transform.position = shootPos;
        transform.Rotate(shootDirection);

        rigid.velocity = shootDirection.normalized * projectileData.ProjectileSpeed;
    }

    private void OnFixedUpdateWork()
    {
        // 아직 test projectile prefab만 사용해서 transform.up을 사용합니다.
        // 나중에 transform.forword로 변경할 예정입니다.
        transform.up = rigid.velocity;
    }



    #endregion

    #region Unity Event

    private void Awake()
    {
        TryGetComponent<Rigidbody>(out rigid);

        if (projectileData.IsStraight)
            rigid.useGravity = false;
        else
            rigid.useGravity = true;
    }

    private void OnEnable()
    {
        if(!projectileData.IsStraight)    
            UpdateManager.SubscribeToFixedUpdate(OnFixedUpdateWork);
    }

    private void OnDisable()
    {
        if(!projectileData.IsStraight)
            UpdateManager.UnsubscribeFromFixedUpdate(OnFixedUpdateWork);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageAble damageAble = other.GetComponent<IDamageAble>();

        if (damageAble != null)
        {
            damageAble.TakeDamage(transform.position, projectileData.Damage, projectileData.KnockbackForce);
        }

        ObjectPool.Instance.ReturnProjectile(this, projectileData.ProjectileType);
    }


    #endregion
    
    
    
    
    #region Obsolete

    ///// <summary>
    ///// 직진 투사체 진행 코루틴
    ///// </summary>
    ///// <param name="shootDirection">발사 방향</param>
    ///// <returns></returns>
    //[Obsolete]
    //IEnumerator TransferProjectile(Vector3 shootDirection)
    //{
    //    var wfs = new WaitForSecondsRealtime(0.02f);
    //    float time = 0;


    //    while (time < projectileData.LifeTime)
    //    {
    //        transform.position += shootDirection * projectileData.ProjectileSpeed * 0.02f;
    //        time += 0.02f;

    //        yield return wfs;
    //    }

    //    yield return null;
    //}

    ///// <summary>
    ///// 포물선 투사체 진행 코루틴
    ///// </summary>
    ///// <param name="shootDirection">총알 진행 방향</param>
    ///// <returns></returns>
    //[Obsolete]
    //IEnumerator TransferFallingProjectile(Vector3 shootDirection)
    //{
    //    var wfs = new WaitForSecondsRealtime(0.02f);
    //    float time = 0;
    //    float gravity = 0;
    //    Vector3 pos;

    //    while (time < projectileData.LifeTime)
    //    {
    //        pos = shootDirection * projectileData.ProjectileSpeed * 0.02f;
    //        gravity += Physics.gravity.y / (projectileData.ProjectileSpeed * projectileData.GravityForce);
    //        pos.y += gravity * 0.02f;

    //        transform.position += pos;

    //        time += 0.02f;
    //        yield return wfs;
    //    }

    //    yield return null;
    //}

    #endregion 
}
