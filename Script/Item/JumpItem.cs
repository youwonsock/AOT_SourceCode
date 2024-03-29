using UnityEngine;

/// <summary>
/// 점프 아이템
/// </summary>
/// 
/// @author yws
/// @date last change 2023/02/26
public class JumpItem : Item
{
    public override void UseItem()
    {
        owner.Rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);

        AudioManager.Instance.AddSfxSoundData(SFXClip.JumpItem, false, owner.transform.position);
    }
}