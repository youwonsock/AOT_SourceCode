using System;
using UnityEngine;
using static ObjectPool;

/// <summary>
/// Projectile의 Scriptable Object Class
/// </summary>
/// 
/// @author yws
/// @date last change 2023/01/01
[CreateAssetMenu(fileName = "Projectile Data", menuName = "Scriptable Object/Projectile Data")]
public class Projectile_SO : ScriptableObject
{
    #region Fields

    [SerializeField] private string projectileName;
    [SerializeField] private float damage;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float lifeTime;
    [SerializeField] private float knockbackForce;
    [SerializeField] private bool isStraight;
    private int projectileType = -1;

    #endregion


    // 만약 외부에서 변경이 필요하다면 Setter가 정의되어 있다면 Property로 변경가능
    #region Property

    public float Damage { get { return damage; } set { damage = value; } }

    public float ProjectileSpeed { get { return projectileSpeed; } set { projectileSpeed = value; } }

    public float LifeTime { get { return lifeTime; } }

    public float KnockbackForce { get { return knockbackForce; } }

    public bool IsStraight { get { return isStraight; } }

    public int ProjectileType 
    { 
        get 
        {
            if (projectileType < 0)
                projectileType = ObjectPool.Instance.GetProjectileType(projectileName);

            return projectileType; 
        } 
    }

    #endregion

}
