using UnityEngine;

/// <summary>
/// 트리거에 의해 게임오브젝트를 활성화시킴
/// </summary>
/// <remarks>
/// 3d text modular와 함께 사용하는 목적으로 만들어짐. 
/// </remarks>
public class TriggerText : MonoBehaviour
{
    #region fields

    [SerializeField] GameObject textObj;
    [SerializeField] bool useOnOff = true; // [use trigger exit?]

    #endregion

    #region unity events

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            textObj.SetActive(true);
            AudioManager.Instance.AddSfxSoundData(SFXClip.Text, false, transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (useOnOff && other.CompareTag("PlayerHand"))
        {
            textObj.SetActive(false);
        }
    }

    #endregion
}
