using UnityEngine;

/// <summary>
/// 세이브 포인트 아이템
/// </summary>
/// 
/// @author yws
/// @date last change 2023/02/26
public class SaveItem : Item
{
    #region Fields

    //- Private -
    [SerializeField] GameObject savePointPrefab;

    #endregion

    #region Methods

    //- Public -
    public override void UseItem()
    {
        Instantiate(savePointPrefab, owner.transform.position + owner.transform.forward, Quaternion.identity);

        AudioManager.Instance.AddSfxSoundData(SFXClip.SaveItem, false, owner.transform.position);
    }

    #endregion

}