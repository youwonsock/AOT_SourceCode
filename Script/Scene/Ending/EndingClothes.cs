using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Ending 씬에서의 player 오브젝트의 복장 변경을 담당하는 스크립트
/// </summary>
/// @date last change 2023/05/21
/// @author MW
/// 
public class EndingClothes : MonoBehaviour
{
    #region Field

    [SerializeField] SkinnedMeshRenderer body;
    [SerializeField] SkinnedMeshRenderer head;
    [SerializeField] Mesh[] BodyMeshes = new Mesh[2];
    [SerializeField] Mesh[] HeadMeshes = new Mesh[2];

    #endregion

    public void ChangeCloth(int w)
    {
        switch(w)
        {
            case 1:
                body.sharedMesh = BodyMeshes[0];
                head.sharedMesh = HeadMeshes[0];
                MinerClothing();
                break;
            case 2:
                body.sharedMesh = BodyMeshes[1];
                head.sharedMesh = HeadMeshes[1];
                break;
        }  
    }

    public void MinerClothing()
    {
        if (body.sharedMesh.name == "Miner-Stickman" && body.materials[0].name == "Flyx_Color (Instance)")
        {
            var arr = body.materials;

            var t = arr[0];
            arr[0] = arr[1];
            arr[1] = t;

            body.materials = arr;
        }
    }

    private void Start()
    {
        head.gameObject.layer = 0;
    }

}
