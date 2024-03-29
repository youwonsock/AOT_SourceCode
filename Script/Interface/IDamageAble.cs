using UnityEngine;

/// <summary>
/// 피해를 입을 수 있는 객체들이 상속받는 인터페이스
/// </summary>
/// <remark>
/// 어떤 방식으로든 피해를 받을 수 있는 객체에 상속하여 
/// 피해를 입은 경우 객체가 취해야하는 행동을 정의하기위한 메서드 TakeDamage를 재정의하여
/// 직접 객체 컴포넌트를 참조하는 것이 아니라 인터페이스를 통해 참조할 수 있습니다.
/// </remark>
/// 
/// @author yws
/// @date last change 2022/11/09
public interface IDamageAble
{
    public void TakeDamage(Vector3 hitPoint, float damage, float knockbackForce);
}
