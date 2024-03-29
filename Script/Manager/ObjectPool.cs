using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 자주 사용되는 Object들을 관리하는 Class
/// </summary>
/// <remarks>
/// ObjectPool은 최적화를 위해 자주 사용되는 Object들을 미리 생성한뒤 Queue에 저장하여 Unity의 연산을 줄여줍니다.\n
/// </remarks>
/// 
/// @author yws
/// @date last change 2023/01/02
public class ObjectPool : Singleton<ObjectPool>
{
    protected ObjectPool() { }


    #region Fields

    [SerializeField] List<GameObject> projectileObjects = new List<GameObject>();

    private Dictionary<string, int> projectileType = new Dictionary<string, int>();
    private Dictionary<int, Queue<Projectile>> projectileDict = new Dictionary<int, Queue<Projectile>>();

    #endregion



    #region Funtion

    /// <summary>
    /// ObjectPool 초기화 메서드
    /// </summary>
    private void Init()
    {
        for (int i = 0; i < projectileObjects.Count; ++i)
        {
            projectileType.Add(projectileObjects[i].name, i);
            projectileDict.Add(i, new Queue<Projectile>());

            for (int j = 0; j < 10; ++j)
            {
                projectileDict[i].Enqueue(CreateNewProjectile(i).GetComponent<Projectile>());
                projectileDict[i].TryPeek(out var projectile);
            }
        }

    }

    /// <summary>
    /// Queue에 저장되는 Object 생성 메서드
    /// </summary>
    /// <param name="idx">GetProjectileType() 메서드를 통해 얻은 idx</param>
    /// <returns>new Projectile Object</returns>
    private GameObject CreateNewProjectile(int idx)
    {
        if (idx < 0 || idx >= projectileObjects.Count)
        {
            Debug.LogError("ObjectPool.CreateNewProjectile() is fall : index out of range");
            return null;
        }

        GameObject newObj = Instantiate<GameObject>(projectileObjects[idx]);

        newObj.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    /// <summary>
    /// ObjectPool에서 사용하는 ProjectileType을 얻는 메서드
    /// </summary>
    /// <param name="key">사용할 Projectile Prefab의 이름</param>
    /// <returns>int형의 key value</returns>
    public int GetProjectileType(string key)
    {
        if (projectileType.TryGetValue(key, out var type))
            return type;

        Debug.LogError("ObjectPool.GetProjectileType() is fall : key is not exist");

        return -1;
    }

    /// <summary>
    /// ObjectPool에서 사용할 Projectile을 가져오는 메서드
    /// </summary>
    /// <remarks>
    /// 만약 Queue에 저장된 Object가 부족하다면 새로 생성 후 반환합니다.
    /// </remarks>
    /// 
    /// <returns>사용할 Projectile</returns>
    public Projectile GetProjectile(int projectileType)
    {
        if (projectileDict.TryGetValue(projectileType, out var projectileQueue))
        {
            Projectile obj;

            if (projectileQueue.Count > 0)
                obj = projectileQueue.Dequeue();
            else
                obj = CreateNewProjectile(projectileType).GetComponent<Projectile>();

            obj.gameObject.SetActive(true);
            obj.transform.SetParent(null);

            return obj;
        }
        // TryGetValue 실패 시
        else
            Debug.LogError("ObjectPool.GetProjectile() is fall");

        return null;
    }

    /// <summary>
    /// 사용한 Projectile을 Queue에 반환하는 메서드
    /// </summary>
    /// <param name="projectile">반환한 Projectile</param>
    /// <param name="projectileType">반환하는 Projectile의 Type</param>
    public void ReturnProjectile(Projectile projectile, int projectileType)
    {
        if (projectileDict.TryGetValue(projectileType, out var projectileQueue))
        {
            projectile.gameObject.SetActive(false);
            projectile.transform.SetParent(transform);

            projectileQueue.Enqueue(projectile);
        }
        // TryGetValue 실패 시
        else
            Debug.LogError("ObjectPool.ReturnProjectile() is fall");
    }

    #endregion



    #region Unity Event

    /// <summary>
    /// Awake에서 실행할 작업을 구현하는 메서드
    /// </summary>
    protected override void OnAwakeWork()
    {
        Init();
    }


    #endregion
}