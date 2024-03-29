using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// Stage2에서 Ending 씬으로 넘어올 때, 승자의 정보를 받아서 함께 넘어오는 오브젝트입니다.
/// </summary>
/// 
/// @date last change 2023/05/18
/// @author MW
/// @class UI
public class EndingInfo : MonoBehaviourPunCallbacks
{
    /**
     * @param des0 왼쪽(Easy Stage)에 플레이어가 먼저 들어왔을 경우 true
     * @param des1 오른쪽(Hard Stage)에 플레이어가 먼저 들어왔을 경우 true
     * @param end_select 플레이어 1,2 중 먼저 들어온 플레이어의 번호(1 xor 2)가 할당
     */
    public bool des0 = false;
    public bool des1 = false;

    public int end_select = 0;

    #region Property
    public bool Des0 { get { return des0;  } set { des0 = value; } }
    public bool Des1 { get { return des1;  } set { des1 = value; } }

    public int End_select { get { return end_select; } set { end_select = value; } }
    #endregion

    private void Awake()
    {
        var obj = FindObjectsOfType<EndingInfo>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
