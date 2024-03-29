using UnityEngine;
/// <summary>
/// 설명 텍스트와 가이드를 띄우는 컴포넌트
/// </summary>
/// <remarks>
/// 플레이어가 접근하면, 지정된 애니메이션을 실행한다
/// </remarks>
/// @author 이은수
/// @date last change 2023/03/22
public class FloatingText : MonoBehaviour
{
    #region fields

    Animator anim;
    [SerializeField, Tooltip("게임시작과 동시실행")] bool playOnStart = false;
    [SerializeField, Tooltip("자체 트리거를 사용합니까?")] bool useTrigger = true;

    #endregion

    #region unity events
    void Awake()
    {
        TryGetComponent<Animator>(out anim);
    }
    private void Start()
    {
        if(playOnStart) 
            Floating(); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if(useTrigger && other.CompareTag("PlayerHand")) 
            Floating();
    }
    private void OnTriggerExit(Collider other)
    {
        if (useTrigger && other.CompareTag("PlayerHand")) 
            Remove();
    }

    #endregion

    #region methods

    public void Floating()
    {
        if (anim != null) 
            anim.SetBool("Collision", true); 
    }
    public void Remove()
    {
        if (anim != null)  
            anim.SetBool("Collision", false); 
    }

    #endregion
}
