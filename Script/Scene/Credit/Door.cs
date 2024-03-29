using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 문과의 상호작용을 담당하는 스크립트 
/// </summary>
/// <remarks>
/// 플레이어가 문에 닿을 경우, 페이드 효과를 주도록 이미지 크기를 변경하고, 플레이어를 포톤 연결 해제 후
/// 플레이어를 Start씬으로 옮깁니다.
/// </remarks>
/// /// @date last change 2023/05/16
/// @author MW
/// 

public class Door : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] GameObject fade;
    public Image fimage;

    // Start is called before the first frame update
    void Start()
    {
        fimage = fade.GetComponent<Image>();
    }

    private void OnTriggerEnter(Collider other)
    {
        fade.transform.DOScale(new Vector2(20000, 20000), 1.5f).SetEase(Ease.InOutExpo);
        Invoke("LoadStartScene", 2f);
    }

    public void LoadStartScene()
    {
        CreditManager.Instance.Load();
    }
}
