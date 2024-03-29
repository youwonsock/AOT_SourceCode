using UnityEngine;

/// <summary>
/// input class의 의존성 주입을 위한 interface
/// </summary>
/// 
/// @author yws
/// @date last change 2023/05/20
public interface IInput
{
    public Vector3 GetMoveDir();
    public bool IsJump();
    public Vector3 GetLookDir();
    public bool IsSprint();
}
