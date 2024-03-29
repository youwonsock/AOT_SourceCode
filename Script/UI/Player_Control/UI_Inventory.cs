using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 인벤토리에서 정보를 받아와 인벤토리 UI에 표시
/// </summary>
/// <remarks>
/// </remarks>
/// @author 이민우
/// @date last date 22/12/06
/// 

/**
* @param ivt 인벤토리
* @param items 아이템을 표시할 UI 이미지 박스의 배열
* @param item0,1 해당 인덱스의 아이템이 갖고 있는 이미지 정보
* @param images 인벤토리의 정보를 가져와 이미지를 표시할 칸을 정하는 리스트
* @param transparent 인벤토리가 비어있을 경우에 채우기 위한 투명 이미지
*/
public class UI_Inventory : MonoBehaviour
{
    private Inventory ivt;
    private Image item0;
    private Image item1;
    public List<Image> images;

    [SerializeField] private GameObject[] inventory;
    [SerializeField] public Sprite transparent;

    
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").TryGetComponent<Inventory>(out ivt);

        inventory[0].TryGetComponent<Image>(out item0);
        inventory[1].TryGetComponent<Image>(out item1);

    }
    /**
     * @details 인벤토리 UI의 이미지를 변경합니다.
     * 인벤토리의 리스트의 원소 갯수를 읽어옵니다.
     * 빈 곳은 투명 이미지를 올리고,
     * 아이템이 들어있다면 images에 추가하고 PrintInventory()를 호출합니다. 
     */

    public void ChangeInventory()
    {
        List<Item> inv = ivt.ItemInventory;
        Debug.Log(inv.Count);
        switch (inv.Count)
        {
            case 0:
                images.Clear();
                item0.sprite = transparent;
                item1.sprite = transparent;
                PrintInventory();
                break;
            case 1:
                images.Clear();
                images.Add(item0);
                item1.sprite = transparent;
                PrintInventory();
                break;
            case 2:
                images.Clear();
                images.Add(item0);
                images.Add(item1);
                PrintInventory();
                break;
        }

    }
    
    /**
     * @details 인벤토리 내 아이템의 이미지 정보를 UI에 출력합니다.
     * inven에서 인벤토리 정보를 가져옵니다.
     * image[i]의 sprite에 대해 
     * inven[i]가 null일 경우 투명 이미지를 할당하고,
     * 아닐 경우 inven[i]가 가진 이미지 정보를 할당합니다.
     */
    public void PrintInventory()
    {
        List<Item> inven = ivt.ItemInventory;

        for (int i = 0; i < inven.Count; i++)
        {
            images[i].sprite = inven[i] != null ? inven[i].ItemImage : transparent;
        }
    }
}
