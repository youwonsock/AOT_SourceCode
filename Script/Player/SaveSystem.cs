using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/// <summary>
/// 세이브 시스템에 대한 스크립트
/// </summary>

/// <remarks>
/// 세이브 발판을 밟을 경우 리스폰 포인트를 바뀌고
/// 세이브 발판의 색깔이 빨간색에서 초록색으로 바뀌도록 구현하였습니다.
/// </remarks>
public class SaveSystem : MonoBehaviour
{
    bool isSave;

    [SerializeField]
    private Material Save, Non_Save;

    Collider co;
    MeshRenderer mesh;

    private void Awake()
    {
        TryGetComponent<Collider>(out co);
        
    }
    // Start is called before the first frame update
    void Start()
    {
        isSave = false;
        mesh = GetComponent<MeshRenderer>();
            
        if(mesh!= null ) 
            mesh.material = Non_Save;
    }

    private void ResetPoint()
    {
        isSave = false;
        mesh.material = Non_Save;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && mesh != null)
        {
            if (!isSave)
            {
                if (other.gameObject.GetPhotonView().IsMine)
                {
                    //GameManager.Instance.RespawnPoint = transform.position;
                    GameManager.Instance.RespawnPoint = co.bounds.center;

                    other.gameObject.GetComponent<Player>().onDeath += ResetPoint;
                    
                    isSave = true;
                    mesh.material = Save;

                    AudioManager.Instance.AddSfxSoundData(SFXClip.Save, false, transform.position);
                }
            }
        }

        if(other.CompareTag("Car"))
        {
            GameManager.Instance.RespawnPoint = transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            GameManager.Instance.RespawnPoint = transform.position;
        }
    }
}
