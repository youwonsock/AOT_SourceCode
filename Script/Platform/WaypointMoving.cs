using UnityEngine;
using System.Collections;
using DG.Tweening;
using Photon.Pun;
using System;

/// <summary>
/// 설정한 경유지를 따라 무빙플랫폼을 이동시키는 컴포넌트
/// </summary>
/// @author 이은수
/// @date last change 2023/03/29
public class WaypointMoving : MonoBehaviourPunCallbacks, IPunObservable
{
	#region fields

	[Tooltip("이동할 오브젝트"), SerializeField] Transform target;
	[Tooltip("동시시작 설정"), SerializeField] bool playOnStart = true;
	[Tooltip("편도이동 설정"), SerializeField] bool isOneWay = false;
	[Tooltip("작동시간"), SerializeField] float duration = 2;
	[Tooltip("대기시간"), SerializeField] float interval = 1;
	[Tooltip("지연시간"), SerializeField] float delay = 0;
	[Tooltip("반복횟수"), SerializeField] int loops = 1;

	[Tooltip("반복유형 설정"), SerializeField] LoopType loopType = LoopType.Restart;
	[SerializeField] Ease easeType = Ease.InOutQuad;
	[SerializeField] SetWaypointType setWaypointType;

	[Tooltip("경유위치를 가진 오브젝트"), SerializeField] Transform[] waypointToObject = { };
	[Tooltip("경유위치 벡터"), SerializeField]

    Vector3[] waypointToVector = new[]
	{
		new Vector3(0,0,0),
		new Vector3(0,0,0)
	};
    [Tooltip("트리거 미션으로 사용하기"), SerializeField] bool TriggerMission = false; 

    bool activePlatform;	// 상태 동기화를 위한 플래그
	bool movable = true;    // 모션의 중복실행 방지 플래그
	int wayCount = 0;		// 편도 순서 카운트 변수
	Sequence moveSequence;  // 모션들의 순서
	Vector3 init;			// 타겟의 초기 위치

    #endregion

    #region property

    /// <summary>
    /// set; Active Platform이 true로 활성화될 때마다 Move() 코루틴 수행. 
    /// </summary>
    public bool ActivePlatform
    {
		get
		{
			return activePlatform;
		}
		set 
		{
            activePlatform = value;
			if(activePlatform)
                StartCoroutine(Move());
		}
	}

    #endregion

    #region methods

    /// <summary>
    /// 무빙플랫폼의 이동로직
    /// </summary>
    /// <remarks>
    /// 편도 혹은 왕복으로 이동시킬 수 있다.
	/// 오브젝트 포지션 혹은 벡터로 위치를 설정할 수 있다.
    /// </remarks>
    IEnumerator Move()
    {
		if (movable && target != null)
        {
			var wfs = new WaitForSecondsRealtime(delay);
			yield return wfs; 

			movable = false;  // 모션을 더 이상 추가하지 못하는 상태로 전환
			moveSequence = DOTween.Sequence();

			if (isOneWay) // 편도이동
			{
				switch (setWaypointType)
				{
					case SetWaypointType.ToObject: // object 경유 
                        if (wayCount != waypointToObject.Length)
                            moveSequence.Append(target.DOMove(waypointToObject[wayCount].position, duration) // 이동 및 파라미터 설정
							.SetEase(easeType)
							.SetDelay(interval));
						else
                            moveSequence.Append(target.DOMove(init, duration) // 마지막 카운트엔 초기 위치로 돌아간다.
                           .SetEase(easeType)
                           .SetDelay(interval));
                        wayCount = (wayCount + 1) % (waypointToObject.Length + 1); // 호출 시 카운트 
						break;

					case SetWaypointType.ToVector: // vector 경유
						moveSequence.Append(target.DOMove(waypointToVector[wayCount], duration)
									.SetRelative()
									.SetEase(easeType)
									.SetDelay(interval)
									);
						wayCount = (wayCount + 1) % waypointToVector.Length;
						break;
                }
			}
			else // 왕복이동
			{
				switch (setWaypointType)			
				{
					case SetWaypointType.ToObject:
                        for (int i = 0; i < waypointToObject.Length; i++)	
						{
							moveSequence.Append(target.DOMove(waypointToObject[i].position, duration)
								.SetEase(easeType)
								.SetDelay(interval));
						}
						break;

					case SetWaypointType.ToVector: 
						for (int i = 0; i < waypointToVector.Length; i++) 
						{
							moveSequence.Append(target.DOMove(waypointToVector[i], duration)
								.SetRelative()
								.SetEase(easeType)
								.SetDelay(interval));
						}
						break;
				}
				moveSequence.SetLoops(loops, loopType); // loops번 반복
			}
			yield return moveSequence.WaitForKill();	// 모션이 끝날 때까지 대기
			activePlatform = false;						// 모션이 끝나면 false로 초기화
			movable = true;								// 모션을 추가할 수 있는 상태로 전환
        }
	}

	/// <summary>
	/// 플랫폼 어태치 기능 함수
	/// </summary>
    [PunRPC]
    private void SetTransform(int photonViewNum)
    {
        var target = PhotonView.Find(photonViewNum).transform;

        if (target.parent == null)
            target.SetParent(this.transform);
        else
            target.SetParent(null);
    }

    /// <summary>
    /// activePlatform 변수 동기화
    /// </summary>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
		if(stream.IsWriting)
			stream.SendNext(activePlatform);
		else
			this.ActivePlatform = (bool)stream.ReceiveNext();
    }

    #endregion

    #region unity events

    /// <summary>
    /// waypointToObject 초기 위치 설정
    /// </summary>
	/// <remarks>
	/// 인스펙터에서 상호배타적인 기능을 중복 체크했을 때를 고려하여 playOnStart를 미리 비활성화
	/// 왕복인 경우, 배열의 크기를 늘려 타겟을 마지막 요소로 삽입한다.
	/// 편도인 경우, 타겟의 초기 위치를 따로 저장한다.
	/// </remarks>
    private void Awake()
    {
        if (TriggerMission || isOneWay) 
			playOnStart = false; 
		
		if (!isOneWay)
		{
            Array.Resize(ref waypointToObject, waypointToObject.Length + 1);    // 배열의 크기를 한 칸 늘리고 
            waypointToObject[waypointToObject.Length - 1] = target;             // 초기 위치를 마지막 도착지로 저장한다.
		}
		else
		{
            init = target.position;
        }
	}
    /// <summary>
    /// 동시시작 플랫폼 관리 
    /// </summary>
    private void Start()
	{
		if (target == null)
			target = transform;

		if (playOnStart && PhotonNetwork.IsMasterClient)
		{
			loops = -1;				// 무한루프
			StartCoroutine(Move()); 
		}
	}
    /// <summary>
    /// 비동시시작 플랫폼 관리와 플랫폼어태치 활성화
    /// </summary>
    private void OnTriggerEnter(Collider other) 
    {
		if (!other.CompareTag("Player"))
			return;

		if (!playOnStart)
		{
			ActivePlatform = true;
			if (other.gameObject.GetPhotonView().IsMine)
				AudioManager.Instance.AddSfxSoundData(SFXClip.MovingPlatform, false, transform.position);
        }

        if (!TriggerMission && other.gameObject.GetPhotonView().IsMine)
            photonView.RPC(nameof(SetTransform), RpcTarget.All, other.gameObject.GetPhotonView().ViewID);
    }
    /// <summary>
    /// 무빙플랫폼에 타고 있는 경우
    /// </summary>
    /// <remarks>
	/// 비동시시작이면서 왕복이동 플랫폼인 경우
	/// activePlatform을 true로 지속한다.
    /// </remarks>
    private void OnTriggerStay(Collider other)
    {
		if (!other.CompareTag("Player") || playOnStart || isOneWay || TriggerMission) 
			return;

        if (!ActivePlatform && photonView.IsMine) 
			ActivePlatform = true;
    }
    /// <summary>
    /// 플랫폼어태치 비활성화
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !TriggerMission && other.gameObject.GetPhotonView().IsMine)
            photonView.RPC(nameof(SetTransform), RpcTarget.All, other.gameObject.GetPhotonView().ViewID);
    }

    #endregion
}

/// <summary>
/// 경유지 설정 방법
/// </summary>
public enum SetWaypointType
{
    ToObject,   // 위치오브젝트를 경유지로
    ToVector,   // 타겟의 상대좌표벡터를 경유지로
}