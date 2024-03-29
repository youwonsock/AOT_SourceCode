using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
/// <summary>
/// 플레이어 스폰 컴포넌트
/// </summary>
/// <remarks>
/// 이 컴포넌트가 붙은 오브젝트의 포지션을 받아서, 스폰 위치를 설정한다.
/// </remarks>
/// @author 이은수 
/// @date last change 2023/02/22
public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    #region fields

    [SerializeField] GameObject playerPrefabs;
    [SerializeField] float offset = 5f;
    [SerializeField] GameObject ginfo;

    float randomX;
    float randomY;
    float randomZ;

    #endregion

    #region methods

    /// <summary>
    /// 스크립트 컴포넌트가 할당된 게임 오브젝트의 포지션을 기준으로 offset 만큼 범위를 설정해서 랜덤한 위치에 플레이어를 스폰합니다.
    /// </summary>
    /// @memo 플레이어의 높이는 그대로 유지합니다.
    private void Spawn()
    {
        if (SceneManager.GetActiveScene().name == "Stage_2")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Vector3 position1 = new Vector3(26.4f, 2.15f, -112.660004f);
                //Vector3 position1 = new Vector3(143.279999f, 79, -168.399994f); // 컨테이너
                //Vector3 position1 = new Vector3(333, 3, -222); // 자동차 테스트
                //Vector3 position1 = new Vector3(467.700012f, 4.73999977f, -149.889999f); // 스테이지 선택 테스트
                //Vector3 position1 = new Vector3(848, 188, -203); //for ending test
                PhotonNetwork.Instantiate(playerPrefabs.name, position1, Quaternion.identity);
            }
            else
            {
                Vector3 position2 = new Vector3(143.279999f, 79, -168.399994f);
                //Vector3 position2 = new Vector3(333, 3, -222);
                //Vector3 position2 = new Vector3(688.3f, 134.7f, -45.42f); //for ending test
                PhotonNetwork.Instantiate(playerPrefabs.name, position2, Quaternion.identity);
            }
        }
        else
        {
            // set position range
            randomX = Random.Range(transform.position.x - offset, transform.position.x + offset);
            randomY = Random.Range(transform.position.y, transform.position.y);
            randomZ = Random.Range(transform.position.z - offset, transform.position.z + offset);

            // positioning
            Vector3 randomPosition = new Vector3(randomX, randomY, randomZ);

            // spawn
            PhotonNetwork.Instantiate(playerPrefabs.name, randomPosition, Quaternion.identity);
        }
        if(PlayerPrefs.GetFloat("TutorialClear") == 1)
        {
            ginfo.SetActive(true);
        }
    }

    #endregion

    #region unity event

    void Awake()
    {
        Spawn();
    }

    #endregion

}