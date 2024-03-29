using System;
using UnityEngine;


/// <summary>
/// Projectile Dispenser의 Scriptable Object Class
/// </summary>
/// 
/// @author yws
/// @date last change 2023/01/01
[CreateAssetMenu(fileName = "Projectile Dispenser Data", menuName = "Scriptable Object/Projectile Dispenser Data")]
public class ProjectileDispenser_SO : ScriptableObject
{
    #region Fields

    [SerializeField] string projectileName;
    [SerializeField] bool fireOnStart;
    [SerializeField] bool infiniteRepeating;
    [SerializeField] int repeatingTime;
    [SerializeField] float fireInterval;
    private int projectileType = -1;

    #endregion



    #region Property

    public bool FireOnStart { get { return fireOnStart; } }

    public bool InfiniteRepeating { get { return infiniteRepeating; } }

    public int RepeatingTime { get { return repeatingTime; } }

    public float FireInterval { get { return fireInterval; } }

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
