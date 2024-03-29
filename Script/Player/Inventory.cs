using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

/// <summary>
/// 플레이어의 inventory class입니다.
/// </summary>
/// 
/// <remarks>
/// inventory는 List 가지고 있어 지정한 사이즈만큼의 아이템을 List에 저장할 수 있으며
/// 아이템을 얻은 순으로 아이템을 사용할 수 있습니다.
/// </remarks>
/// 
/// @author yws
/// @date last change 2023/02/26
public class Inventory : MonoBehaviour
{
    #region Fields

    [SerializeField] public List<Item> itemInventory = new List<Item>();
    [SerializeField] private int inventorySize = 2;
    UI_Inventory ivt;

    #endregion



    #region Property

    public List<Item> ItemInventory { get { return itemInventory; } }

    #endregion



    #region Methods
    private void Start()
    {
        FindObjectOfType<UI_Inventory>().TryGetComponent<UI_Inventory>(out ivt);
    }


    /// <summary>
    /// 인벤토리에 아이템을 추가하는 메서드
    /// </summary>
    /// <param name="item">추가할 아이템</param>
    public void AddItemToInventory(Item item)
    {
        AudioManager.Instance.AddSfxSoundData(SFXClip.Get, false, this.transform.position);

        if (itemInventory.Count >= inventorySize)
        {
            PhotonNetwork.Destroy(itemInventory[0].gameObject);
            itemInventory.RemoveAt(0);
        }

        itemInventory.Add(item);

        //ui
        ivt.ChangeInventory();
    }

    /// <summary>
    /// 아이템 사용 메서드
    /// </summary>
    public void UseItem()
    {
        if (itemInventory.Count > 0)
        {
            Item item = itemInventory[0];
            itemInventory.RemoveAt(0);

            item.UseItem();
            PhotonNetwork.Destroy(item.gameObject);

            // ui 
            ivt.ChangeInventory();
        }
        else
            Debug.Log("inventory is empty!");

    }

    #endregion

}
