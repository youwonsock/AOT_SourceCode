using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

/// <summary>
/// Player 복장 변경 클래스
/// </summary>
/// <remarks>
/// 
/// </remarks>
///
/// @date last change 2023/05/19
/// @author YWS
/// @class PlayerClothes
public class PlayerClothes : MonoBehaviourPunCallbacks
{

    #region Fields

    //- Private -
    [SerializeField] SkinnedMeshRenderer body;
    [SerializeField] SkinnedMeshRenderer head;

    [SerializeField] Mesh[] masterBodyMeshes = new Mesh[2];
    [SerializeField] Mesh[] masterHeadMeshes = new Mesh[2];
    [SerializeField] Mesh[] clientBodyMeshes = new Mesh[2];
    [SerializeField] Mesh[] clientHeadMeshes = new Mesh[2];
    //- Public -

    #endregion


    #region Methods
    //- Private -

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Stage_1":
                if(PhotonNetwork.IsMasterClient)
                {
                    if (photonView.IsMine)
                    {
                        body.sharedMesh = masterBodyMeshes[0];
                        head.sharedMesh = masterHeadMeshes[0];
                    }
                    else
                    {
                        body.sharedMesh = clientBodyMeshes[0];
                        head.sharedMesh = clientHeadMeshes[0];
                    }    
                }
                else
                {
                    if (photonView.IsMine)
                    {
                        body.sharedMesh = clientBodyMeshes[0];
                        head.sharedMesh = clientHeadMeshes[0];
                    }
                    else
                    {
                        body.sharedMesh = masterBodyMeshes[0];
                        head.sharedMesh = masterHeadMeshes[0];
                    }
                }
                break;
            case "Stage_2":
                if (PhotonNetwork.IsMasterClient)
                {
                    if (photonView.IsMine)
                    {
                        body.sharedMesh = masterBodyMeshes[1];
                        head.sharedMesh = masterHeadMeshes[1];
                    }
                    else
                    {
                        body.sharedMesh = clientBodyMeshes[1];
                        head.sharedMesh = clientHeadMeshes[1];
                    }
                }
                else
                {
                    if (photonView.IsMine)
                    {
                        body.sharedMesh = clientBodyMeshes[1];
                        head.sharedMesh = clientHeadMeshes[1];
                    }
                    else
                    {
                        body.sharedMesh = masterBodyMeshes[1];
                        head.sharedMesh = masterHeadMeshes[1];
                    }
                }
                if (body.sharedMesh.name == "Miner-Stickman" && body.materials[0].name == "Flyx_Color (Instance)")
                {
                    var arr = body.materials;

                    var t = arr[0];
                    arr[0] = arr[1];
                    arr[1] = t;

                    body.materials = arr;
                }
                break;
        }  
    }

    private void SetHeadLayerFPV()
    {
        head.gameObject.layer = 15;
    }

    private void SetHeadLayerTPV()
    {
        head.gameObject.layer = 0;
    }

    #endregion



    #region UnityEvent

    private void Start()
    {
        GameManager.Instance.AddOnSceneLoaded(OnSceneLoaded);

        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnEnable()
    {
        base.OnEnable();   

        if(photonView.IsMine) 
        {
            GameManager.Instance.AddChangePVEvent(SetHeadLayerFPV, 1);
            GameManager.Instance.AddChangePVEvent(SetHeadLayerTPV, 3);
        }
    }

    private void OnDisable()
    {
        base.OnEnable();

        if(photonView.IsMine)
        {
            GameManager.Instance.RemoveChangePVEvent(SetHeadLayerFPV, 1);
            GameManager.Instance.RemoveChangePVEvent(SetHeadLayerTPV, 3);
        }
    }

    #endregion
}
