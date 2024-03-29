using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Globalization;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

/// <summary>
/// 차의 탑승 및 하차 역할을 맡은 스크립트
/// </summary>
/// @author YWS
/// @date last change 2023/05/19
public class Car_Together : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    #region fields

    [SerializeField] GameObject Racing_Camera;
    [SerializeField] GameObject text;

    private int boardingNum = -1;
    private bool isBoarding;
    int[] playerIDArr = new int[2] { -1, -1 };

    [SerializeField] private ParticleSystem particle;

    private AudioSource audio;
    private Rigidbody rigid;

    #endregion

    #region Property

    public bool BoardingAll { get; private set; }

    public int BoardingNum { get { return boardingNum; } }

    #endregion

    #region Methods

    /// <summary>
    /// 플레이어 차량 탑승 메서드
    /// </summary>
    /// <param name="playerID"></param>
    [PunRPC]
    private void Boarding(int playerID)
    {
        var player = PhotonView.Find(playerID);

        Vector3 direction = player.transform.position - transform.position;
        bool isOwner = Vector3.Dot(transform.up,Vector3.Cross(direction, transform.forward)) > 0 ? true : false;

        if(CheckSheet(isOwner))
        {
            SyncArr(true, isOwner, playerID);

            if (isOwner)
                photonView.TransferOwnership(player.Owner);

            if (player.IsMine)
            {
                Racing_Camera.SetActive(true);
                boardingNum = isOwner ? 0 : 1;

                text.SetActive(false);

                SetBoarding(true).Forget();
            }

            player.gameObject.SetActive(false);

            if (boardingNum == 0)
                Racing_Camera.transform.localPosition = new Vector3(-0.5f, 1.8f, -0.2f);
            else if (boardingNum == 1)
                Racing_Camera.transform.localPosition = new Vector3(0.5f, 1.8f, -0.2f);

            AudioManager.Instance.AddSfxSoundData(SFXClip.RideCar, false, transform.position);
        }
    }

    /// <summary>
    /// 플레이어 차량 하차 메서드
    /// </summary>
    /// <param name="boardingNum"></param>
    [PunRPC]
    private void QuitCar(int boardingNum)
    {
        var player = PhotonView.Find(playerIDArr[boardingNum]);

        SyncArr(false, boardingNum == 0, playerIDArr[boardingNum]);

        if (player.IsMine)
        {
            player.transform.position = boardingNum == 0 ? this.transform.position + -transform.right * 2 : this.transform.position + transform.right * 2;

            Racing_Camera.SetActive(false);
            this.boardingNum =  -1;

            SetBoarding(false).Forget();
        }

        player.gameObject.SetActive(true);

        AudioManager.Instance.AddSfxSoundData(SFXClip.RideCar,false, transform.position);
    }

    /// <summary>
    /// 좌석 체크 메서드
    /// </summary>
    /// <param name="isOwner"> 운전석인지? </param>
    /// <returns></returns>
    private bool CheckSheet(bool isOwner)
    {
        if (isOwner)
        {
            if (playerIDArr[0] != -1)
                return false;
        }
        else
        {
            if (playerIDArr[1] != -1)
                return false;
        }

        return true;
    }


    /// <summary>
    /// 배열 동기화 메서드
    /// </summary>
    /// <param name="val"> 탑승, 하차 구분 </param>
    /// <param name="isOwner"> 운전석인지? </param>
    /// <param name="playerID"> playerID </param>
    private void SyncArr(bool val,bool isOwner, int playerID)
    {
        if (isOwner)
            playerIDArr[0] = val == true ? playerID : -1;
        else
            playerIDArr[1] = val == true ? playerID : -1;

        foreach (var t in playerIDArr)
        {
            if (t == -1)
            {
                BoardingAll = false;
                audio.enabled = false;
                return;
            }
        }

        audio.enabled = true;
        BoardingAll = true;
    }

    private async UniTaskVoid SetBoarding(bool val)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        isBoarding = val;
    }

    private async UniTaskVoid DestroyCar()
    {
        particle.Play();
        AudioManager.Instance.AddSfxSoundData(SFXClip.CarExplosion, false, transform.position);
        await UniTask.Delay(TimeSpan.FromSeconds(3));

        Destroy(this.gameObject);
    }

    public void OnOwnershipRequest(PhotonView targetView, Photon.Realtime.Player requestingPlayer)
    {

    }
    public void OnOwnershipTransfered(PhotonView targetView, Photon.Realtime.Player previousOwner)
    {

    }
    public void OnOwnershipTransferFailed(PhotonView targetView, Photon.Realtime.Player senderOfFailedRequest)
    {
        // 플레이어 재탑승 처리가 필요할듯
    }

    #endregion

    #region Unity Events

    private bool t;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Destination") && !t)
        {
            GameManager.Instance.RespawnPoint = other.transform.position;
            foreach (var t in playerIDArr)
            {
                var player = PhotonView.Find(t);

                player.transform.position = GameManager.Instance.RespawnPoint;
                player.gameObject.SetActive(true);
            }

            Racing_Camera.SetActive(false);
            t = true;
            DestroyCar().Forget();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetPhotonView().IsMine) // 탑승
        {
            text.SetActive(true);

            if (Input.GetKey(KeyCode.F) && !isBoarding)
            {
                photonView.RPC(nameof(Boarding), RpcTarget.All, other.gameObject.GetPhotonView().ViewID);
            }
        }
        else if(other.CompareTag("CheckPoint") && boardingNum != -1) // 하차
        {
            text.SetActive(true);

            if (rigid.velocity.magnitude < 1 && (Input.GetKey(KeyCode.F) && isBoarding))
            {
                photonView.RPC(nameof(QuitCar), RpcTarget.All, boardingNum);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CheckPoint") || other.CompareTag("Player"))
        {
            text.SetActive(false);
        }
    }

    private void Awake()
    {
        TryGetComponent<AudioSource>(out audio);
        TryGetComponent<Rigidbody>(out rigid);
    }

    private void Update()
    {
        if(BoardingAll)
            audio.volume = AudioManager.Instance.Sfx * AudioManager.Instance.Master;
    }

    #endregion

}