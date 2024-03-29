using Photon.Pun;
using UnityEngine;

/// <summary>
/// 플레이어가 사용하는 아이템의 추상 클래스입니다.
/// </summary>
/// 
/// <remarks>
/// Item Class는 플레이어가 사용하는 아이템 클래스들의 부모 클래스로 Item에 필요한 메서드 및 변수를 명시해둔 클래스입니다.
/// </remarks>
/// 
/// @author yws
/// @date last change 2023/02/26
public abstract class Item : MonoBehaviourPunCallbacks
{

    #region Fields

    //- Private -
    [SerializeField] Sprite itemImage;
    protected Player owner;

    //- Public -

    #endregion



    #region Property
    //- Private -

    //- Public -
    public Sprite ItemImage { get { return itemImage; } }

    #endregion



    #region Methods
    //- Private -

    /// <summary>
    /// 오브젝트 활성상태 설정을 위한 RPC 메서드
    /// </summary>
    /// <param name="value"> 설정값 </param>
    [PunRPC]
    protected void SetObjectActive(bool value)
    {
        gameObject.SetActive(value);
    }

    //- Public -

    /// <summary>
    /// 아이템 사용 메서드
    /// </summary>
    public abstract void UseItem();

    #endregion



    #region Coroutine
    //- Private -

    //- Public -

    #endregion



    #region UnityEvent

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetPhotonView().IsMine)
            {
                Inventory inventory;
                other.gameObject.TryGetComponent<Inventory>(out inventory);

                other.TryGetComponent<Player>(out owner);

                if (inventory != null)
                {
                    inventory.AddItemToInventory(this);

                    photonView.RPC(nameof(SetObjectActive), RpcTarget.All, false);
                }
            }
        }
    }

    #endregion
}