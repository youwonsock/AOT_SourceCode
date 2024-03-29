using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Ending 씬의 오브젝트를 관리하는 클래스입니다.
/// 
/// </summary>
/// @date last change 2023/05/21
/// @author MW
/// 
public class EndingManager : MonoBehaviour
{
    #region Field
    private static EndingManager instance;
    EndingInfo ei;

    /**
     * @param ending_des EndingInfo의 des0를 받아와 갇히는 위치 설정
     * @param ending_num EndingInfo의 end_select를 받아와 갇히는 플레이어 설정
     * @param cutscenecam 시네마틱 카메라의 재생을 위한 오브젝트
     * @param changer Credit 씬을 불러오기 위한 오브젝트
     * @param player1set 1p가 이겼을 경우 비활성화
     * @param player2ser 2p가 이겼을 경우 비활성화
     * @param vcam1 승자에 따라 달라지는 시네마틱 카메라의 위치
     * @param winner 승자 오브젝트, 복장 변경을 위해 사용
     * @param p1,p2 패자 오브젝트, 복장 변경을 위해 사용
     */
    private bool ending_des;
    private int ending_num;
    [SerializeField] GameObject cutscenecam;
    [SerializeField] GameObject changer;
    [SerializeField] GameObject player1set;
    [SerializeField] GameObject player2set;
    [SerializeField] GameObject vcam1;
    [SerializeField] GameObject winner;
    [SerializeField] GameObject p1;
    [SerializeField] GameObject p2;
    #endregion

    #region Property
    public static EndingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EndingManager>();
            }
            return instance;
        }
    }

    #endregion

    /**
     * @brief 승자 정보를 받아와서, 승자 쪽의 오브젝트는 비활성화합니다.
     * @details 먼저 들어온 방향의 철창과 플레이어 프리펩을 비활성화, 나중에 들어온 방향으로 Virtual Cinematic Camera 배치.
     */
    #region Methods
    public void EndingSelect(bool des)
    {
        switch (des)
        {
            case true:
                player1set.SetActive(false);
                vcam1.transform.localPosition = new Vector3(-298.82f, 13.93f, 28.16f);
                vcam1.transform.localRotation = Quaternion.Euler(-1.2f, 82.88f, 0.004f);
                break;
            case false:
                player2set.SetActive(false);
                vcam1.transform.localPosition = new Vector3(-301.30f, 13.13f, 89.75f);
                vcam1.transform.localRotation = Quaternion.Euler(-3.61f, 101.96f, 0.005f);
                break;

        }
    }
    public void LoadCredit()
    {
        SceneManager.LoadScene("Credit");
    }

    /**
     * @param i ending_num을 받아와 승자를 기준으로 플레이어 프리펩의 의상 설정
     * @brief 승자를 기준으로 각 플레이어의 의상 설정
     */

    public void SetCharClothing(int j)
    {
        if(j == 1)
        {
            winner.GetComponent<EndingClothes>().ChangeCloth(j);
            if (player1set.activeSelf)
                p1.GetComponent<EndingClothes>().ChangeCloth(j + 1);
            else if(player2set.activeSelf)
                p2.GetComponent<EndingClothes>().ChangeCloth(j + 1);
            

        }
        else if (j == 2)
        {
            winner.GetComponent<EndingClothes>().ChangeCloth(j);
            if (player1set.activeSelf)
                p1.GetComponent<EndingClothes>().ChangeCloth(j - 1);
            else if (player2set.activeSelf)
                p2.GetComponent<EndingClothes>().ChangeCloth(j - 1);
        }
    }
  

    #endregion

    #region Unity Events

    private void Update()
    {
        if(changer.activeSelf)
        {
            LoadCredit();            
        }
    }
    private void Awake()
    {
        ei = GameObject.FindObjectOfType<EndingInfo>();
        ending_num = ei.end_select;
        ending_des = ei.des0;
        EndingSelect(ending_des);
        SetCharClothing(ending_num);
        cutscenecam.SetActive(true);
    }

    
    #endregion
}
