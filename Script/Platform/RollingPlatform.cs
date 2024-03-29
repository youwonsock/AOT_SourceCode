using UnityEngine;
using DG.Tweening;

/// <summary>
/// 회전하는 플랫폼 컴포넌트
/// </summary>
/// <remarks>
/// 회전하며 이동하는 모션을 구현할 수 있습니다.
/// </remarks>
/// @author 이은수
/// @date last change 2022/11/15
public class RollingPlatform : MonoBehaviour
{
    #region fields

    [SerializeField] int loops = -1;
    [SerializeField] float moveDuration = 3;
    [SerializeField] float rotateDuration = 1;
    [SerializeField] Vector3 moveVec = new(0, 0, 0);
    [SerializeField] Vector3 rotateVec = new(0, 0, 0);

    #endregion

    #region unity event

    private void Start()
    {
        Sequence roll = DOTween.Sequence();
        roll.Append(transform.DOMove(moveVec, moveDuration).SetRelative())      // 이동모션
            .Join(transform.DORotate(rotateVec, rotateDuration).SetRelative()); // 회전모션
        roll.SetLoops(loops, LoopType.Restart);
    }

    #endregion
}
