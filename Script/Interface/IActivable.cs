using UnityEngine;

/// /// <summary>
/// 함정등에 상속시켜 외부에서 interface참조를 통한 활성화를 가능하게 해주는 interface입니다.
/// </summary>
/// 
/// @author yws
/// @date last change 2022/11/05
public interface IActivable
{
    void ActivateObject(Vector3 triggerPos);
}
