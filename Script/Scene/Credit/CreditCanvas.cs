using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Experimental.GlobalIllumination;
using Cysharp.Threading.Tasks.Triggers;
/// <summary>
/// 크레딧을 재생하는 스크립트
/// </summary>
/// <remarks>
/// 크레딧을 재생하고, 상호작용 가능한 병을 떨어트립니다.
/// </remarks>
/// @date last change 2023/05/16
/// @author MW
/// 
public class CreditCanvas : MonoBehaviour
{
    #region Field
    [SerializeField] GameObject title;
    [SerializeField] GameObject team;
    [SerializeField] GameObject teamate;    
    [SerializeField] GameObject spthx;
    [SerializeField] GameObject beta;
    [SerializeField] GameObject you;
    [SerializeField] GameObject farewell;
    [SerializeField] GameObject bottle;
    #endregion
    private void Start()
    {
        CreditPlay();
    }

    public void CreditPlay()
    {
        title.transform.DOScale(new Vector3(1f, 1f, 1f), 2.5f);
        title.transform.DOLocalMoveX(-1200, 3f).SetEase(Ease.InQuart).SetDelay(5f);

        team.transform.DOLocalMoveX(0, 2.5f).SetEase(Ease.OutQuart).SetDelay(7.5f);
        teamate.transform.DOLocalMoveX(0, 2.5f).SetEase(Ease.OutQuart).SetDelay(8f);

        team.transform.DOLocalMoveX(-1000, 2.5f).SetEase(Ease.InQuart).SetDelay(11f);
        teamate.transform.DOLocalMoveX(-1200, 2.5f).SetEase(Ease.InQuart).SetDelay(11.5f);

        spthx.transform.DOLocalMoveX(0, 2.5f).SetEase(Ease.OutQuart).SetDelay(13.5f);
        beta.transform.DOLocalMoveX(0, 2.5f).SetEase(Ease.OutQuart).SetDelay(14f);
        beta.transform.DOLocalMoveX(-1000, 2.5f).SetEase(Ease.InQuart).SetDelay(17.5f);

        you.transform.DOLocalMoveX(0, 2.5f).SetEase(Ease.OutQuart).SetDelay(20f);
        spthx.transform.DOLocalMoveX(-1000, 2.5f).SetEase(Ease.InQuart).SetDelay(22.5f);
        you.transform.DOLocalMoveX(-1000, 2.5f).SetEase(Ease.InQuart).SetDelay(23f);

        farewell.transform.DOLocalMoveX(0, 2.5f).SetEase(Ease.OutQuart).SetDelay(25.5f);
        farewell.transform.DOLocalMoveX(-1000, 2.5f).SetEase(Ease.InQuart).SetDelay(28f);

        Invoke("EasterEgg",30.5f);
    }

    public void EasterEgg()
    {
        Rigidbody rb = bottle.GetComponent<Rigidbody>();
        rb.useGravity = true ;
    }
}