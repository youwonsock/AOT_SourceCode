using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// 승자의 정보를 읽어서 다음 오브젝트에 전해주기 위한 스크립트
/// </summary>
/// /// @date last change 2023/05/19
/// @author MW
/// 
public class Stage2Info : MonoBehaviourPunCallbacks
{
    /**
     * @param endinginfo DontDestoryOnLoad 오브젝트로, Ending씬에 정보를 넘겨주기 위한 오브젝트
     * @param part0,part1 플레이어의 엔딩씬 진입을 막는 partition 오브젝트
     */
    [SerializeField] GameObject endinginfo;
    [SerializeField] GameObject part0;
    [SerializeField] GameObject part1;

    EndingInfo eifo;
    // Start is called before the first frame update
    void Start()
    {
        eifo = endinginfo.GetComponent<EndingInfo>();
    }


    /**
     * @brief RPC 함수 호출로 양쪽 플레이어 변수값 동기화
     */
    [PunRPC]
    public void SetEndingRPC(int s)
    {
        if(s==1)
        {
            eifo.Des0 = true;
        }

        else if(s == 0)
        {
            eifo.Des1 = true;
        }
    }
    
    private void Update()
    {
        if (part0.transform.position.y >= 200)
        {
            photonView.RPC("SetEndingRPC", RpcTarget.All,0);
        }
        else if (part1.transform.position.y >= 200)
        {
            photonView.RPC("SetEndingRPC", RpcTarget.All, 1);
        }
    }
}
