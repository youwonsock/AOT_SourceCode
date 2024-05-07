# Alone Or Together

## Developer Info
* 이름 : 유원석(You Won Sock)
* GitHub : https://github.com/youwonsock
* Mail : qazwsx233434@gmail.com

## Our Game
### Youtube

[![Alone Or Together](https://img.youtube.com/vi/gvBp5dKf-1s/0.jpg)](https://www.youtube.com/watch?v=gvBp5dKf-1s)

### Genres

3D Multiplayer platformer

<b><h2>Platforms</h2></b>

<p>
<img src="https://upload.wikimedia.org/wikipedia/commons/c/c7/Windows_logo_-_2012.png" height="30">
</p>

### Development kits

<p>
<img src="https://upload.wikimedia.org/wikipedia/commons/thumb/1/19/Unity_Technologies_logo.svg/1280px-Unity_Technologies_logo.svg.png" height="40">
</p>

<b><h2>Periods</h2></b>

* 2022-10 - 2023-05

## Contribution

### Item
  * JumpItem  
    ![jump-min](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/6385c7a1-e335-4143-bb00-8a507b7ad81c)  
    사용 시 플레이어를 점프시키는 아이템입니다.

  * SaveItem
    ![save-min](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/f220a099-570c-4564-9bd3-0ae0ff54cee7)  
    사용 시 세이브 포인트를 생성하는 아이템입니다.

### Manager 
  * GameManager   
    게임의 전체적인 흐름을 관리하는 매니저
    <details>
    <summary>GameManager Code</summary>
    <div markdown="1">

      ```c#
      using System;
      using UnityEngine;
      using UnityEngine.Events;
      using UnityEngine.SceneManagement;
      /// <summary>
      /// 게임의 관리를 위한 기능들을 가지고있는 Class
      /// </summary>
      /// <remarks>
      /// GameManager는 Singleton 클래스로 게임에서 사용되는 전역 변수등을 관리할 예정입니다.
      /// </remarks>
      /// 
      /// @author yws
      /// @date last change 2023/05/19
      public class GameManager : Singleton<GameManager>
      {
          protected GameManager() { }

          #region Fields
          public int goal;
          public int levercnt = 0;
          public bool master_first;
          public bool client_first;
          
          // animation hash values
          public static int animIDSpeed = Animator.StringToHash("Speed");
          public static int animIDGrounded = Animator.StringToHash("Grounded");
          public static int animIDJump = Animator.StringToHash("Jump");
          public static int animIDFreeFall = Animator.StringToHash("FreeFall");
          public static int animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
          public static int animIDFp = Animator.StringToHash("FP");
          public static int animIDInteraction = Animator.StringToHash("Interaction");

          private Action activeFPV;
          private Action activeTPV;
          
          private Vector3 respawnPoint = Vector3.zero;
          #endregion


          #region Property

          public Vector3 RespawnPoint { get { return respawnPoint; } set { respawnPoint = value; } }

          public bool ShaderChange { get; set; }
          #endregion



          #region Methods

          /// <summary>
          /// 리스폰 포인트 초기화 메서드
          /// </summary>
          public void InitRespawnPoint()
          {
              var startPointObj = GameObject.FindGameObjectWithTag("StartPoint");

              if (startPointObj != null)
                  respawnPoint = startPointObj.transform.position;
          }

          /// <summary>
          /// OnSceneLoaded
          /// </summary>
          /// <param name="scene"></param>
          /// <param name="mode"></param>
          private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
          {
              InitRespawnPoint();

              if(scene.name == "Tutorial" || scene.name == "Stage_1"|| scene.name == "Stage_2" || scene.name == "Ending")
              {
                  Cursor.lockState= CursorLockMode.Locked;
                  Cursor.visible = false;
              }
          }

          /// <summary>
          /// SceneLoaded Event 등록 메서드
          /// </summary>
          /// <param name="action"> 등록할 Action</param>
          public void AddOnSceneLoaded(UnityAction<Scene, LoadSceneMode> action)
          {
              SceneManager.sceneLoaded += action;
          }

          /// <summary>
          /// SceneLoaded Event 등록 헤제 메서드
          /// </summary>
          /// <param name="action">등록 해제할 Action</param>
          public void RemoveOnSceneLoaded(UnityAction<Scene, LoadSceneMode> action)
          {
              SceneManager.sceneLoaded -= action;
          }

          /// <summary>
          /// 시점 변환 시 실행되는 메서드
          /// </summary>
          /// <param name="n"></param>
          public void OnChangePV(int n)
          {
              if(n == 1)
                  activeFPV?.Invoke();
              else if(n == 3)
                  activeTPV?.Invoke();
          }

          /// <summary>
          /// 시점 변환 이벤트 등록 메서드
          /// </summary>
          /// <param name="action"></param>
          public void AddChangePVEvent(Action action, int n)
          {
              if (n == 1)
                  activeFPV += action;
              else if (n == 3)
                  activeTPV += action;
          }

          /// <summary>
          /// 시점 변환 이벤트 제거 메서드
          /// </summary>
          /// <param name="action"></param>
          public void RemoveChangePVEvent(Action action, int n)
          {
              if (n == 1)
                  activeFPV -= action;
              else if (n == 3)
                  activeTPV -= action;
          }

          #endregion



          #region Unity Event

          /// <summary>
          /// Awake에서 실행할 작업을 구현하는 메서드
          /// </summary>
          protected override void OnAwakeWork()
          {
              SceneManager.sceneLoaded += OnSceneLoaded;

              //Application.targetFrameRate = 30;
          }

          /// <summary>
          /// OnDestroyed에서 실행할 작업을 구현하는 메서드
          /// </summary>
          protected override void OnDestroyedWork()
          {
              base.OnDestroyedWork();
          }

          // temp !!!!
          private void Start()
          {
              PlayerPrefs.GetFloat("TutorialClear", 0);
              PlayerPrefs.SetFloat("Stage1Clear", 0);
              PlayerPrefs.SetFloat("Stage2Clear", 0);

              ShaderChange = true;

              //Cursor.lockState = CursorLockMode.Locked;
              //Cursor.visible = false;
          }

          #endregion
      }

      ```
    
    </div>
    </details>
  </br>

  * UpdateManager   
    최적화를 위해 각 컴포넌트에서 Update를 호출하는 것이 아닌 UpdateManager에 이벤트로 등록하여  
    UpdateManager에서 Update를 호출하는 방식으로 구현하였습니다.
    <details>
    <summary>UpdateManager Code</summary>
    <div markdown="1">

      ```c#
      using System;
      using UnityEngine;

      /// <summary>
      /// Unity의 Update 이벤트를 관리하는 Class
      /// </summary>
      /// <remarks>
      /// UpdateManager는 최적화를 위해서 Update 호출을 한 곳에서 실행하기 위해 만들어진 Class입니다.\n
      /// 각 UpdateEvent에 대응되는 3개의 이벤트를 가지고 있으며, 컴포넌트의 OnEnable, OnDisable에서 구독과 구독 해제를 진행해줍니다.
      /// </remarks>
      /// 
      /// @author yws
      /// @date last change 2022/10/06
      public class UpdateManager : MonoBehaviour {

          #region Fields

          private static event Action OnUpdate;
          private static event Action OnFixedUpdate;
          private static event Action OnLateUpdate;

          #endregion



          #region Funtions
          /// <summary>
          /// OnUpdate Event 구독 메서드
          /// </summary>
          /// <param name="callback">구독시킬 메서드</param>
          public static void SubscribeToUpdate(Action callback)
          {
              OnUpdate += callback;
          }

          /// <summary>
          /// OnFixedUpdate 구독 메서드
          /// </summary>
          /// <param name="callback">구독시킬 메서드</param>
          public static void SubscribeToFixedUpdate(Action callback)
          {
              OnFixedUpdate += callback;
          }

          /// <summary>
          /// OnLateUpdate 구독 메서드
          /// </summary>
          /// <param name="callback">구독시킬 메서드</param>
          public static void SubscribeToLateUpdate(Action callback)
          {
              OnLateUpdate += callback;
          }

          /// <summary>
          /// OnUpdate 구독 해제 메서드
          /// </summary>
          /// <param name="callback">구독 해제시킬 메서드</param>
          public static void UnsubscribeFromUpdate(Action callback)
          {
              OnUpdate -= callback;
          }

          /// <summary>
          /// OnFixedUpdate 구독 해제 메서드
          /// </summary>
          /// <param name="callback">구독 해제시킬 메서드</param>
          public static void UnsubscribeFromFixedUpdate(Action callback)
          {
              OnFixedUpdate -= callback;
          }

          /// <summary>
          /// OnLateUpdate 구독 해제 메서드
          /// </summary>
          /// <param name="callback">구독 해제시킬 메서드</param>
          public static void UnsubscribeFromLateUpdate(Action callback)
          {
              OnLateUpdate -= callback;
          }
          #endregion



          #region UnityEvent
          void Update()
          {
              if (OnUpdate != null)
                  OnUpdate.Invoke();
          }

          private void FixedUpdate()
          {
              if (OnFixedUpdate != null)
                  OnFixedUpdate.Invoke();
          }

          private void LateUpdate()
          {
              if (OnLateUpdate != null)
                  OnLateUpdate.Invoke();
          }
          #endregion
      }
      ```
    
    </div>
    </details>
  </br>

  * ObjectPool  
    * Projectile  
    
    오브젝트 풀링을 위한 매니저입니다.  
    오브젝트 풀에 오브젝트가 없는 경우 동적으로 생성하여 사용하고, 사용이 끝난 오브젝트는 오브젝트 풀에 반환합니다.  
    <details>
    <summary>ObjectPool Code</summary>
    <div markdown="1">

      ```c#
      using System;
      using System.Collections.Generic;
      using UnityEngine;

      /// <summary>
      /// 자주 사용되는 Object들을 관리하는 Class
      /// </summary>
      /// <remarks>
      /// ObjectPool은 최적화를 위해 자주 사용되는 Object들을 미리 생성한뒤 Queue에 저장하여 Unity의 연산을 줄여줍니다.\n
      /// </remarks>
      /// 
      /// @author yws
      /// @date last change 2023/01/02
      public class ObjectPool : Singleton<ObjectPool>
      {
          protected ObjectPool() { }


          #region Fields

          [SerializeField] List<GameObject> projectileObjects = new List<GameObject>();

          private Dictionary<string, int> projectileType = new Dictionary<string, int>();
          private Dictionary<int, Queue<Projectile>> projectileDict = new Dictionary<int, Queue<Projectile>>();

          #endregion



          #region Funtion

          /// <summary>
          /// ObjectPool 초기화 메서드
          /// </summary>
          private void Init()
          {
              for (int i = 0; i < projectileObjects.Count; ++i)
              {
                  projectileType.Add(projectileObjects[i].name, i);
                  projectileDict.Add(i, new Queue<Projectile>());

                  for (int j = 0; j < 10; ++j)
                  {
                      projectileDict[i].Enqueue(CreateNewProjectile(i).GetComponent<Projectile>());
                      projectileDict[i].TryPeek(out var projectile);
                  }
              }

          }

          /// <summary>
          /// Queue에 저장되는 Object 생성 메서드
          /// </summary>
          /// <param name="idx">GetProjectileType() 메서드를 통해 얻은 idx</param>
          /// <returns>new Projectile Object</returns>
          private GameObject CreateNewProjectile(int idx)
          {
              if (idx < 0 || idx >= projectileObjects.Count)
              {
                  Debug.LogError("ObjectPool.CreateNewProjectile() is fall : index out of range");
                  return null;
              }

              GameObject newObj = Instantiate<GameObject>(projectileObjects[idx]);

              newObj.SetActive(false);
              newObj.transform.SetParent(transform);
              return newObj;
          }

          /// <summary>
          /// ObjectPool에서 사용하는 ProjectileType을 얻는 메서드
          /// </summary>
          /// <param name="key">사용할 Projectile Prefab의 이름</param>
          /// <returns>int형의 key value</returns>
          public int GetProjectileType(string key)
          {
              if (projectileType.TryGetValue(key, out var type))
                  return type;

              Debug.LogError("ObjectPool.GetProjectileType() is fall : key is not exist");

              return -1;
          }

          /// <summary>
          /// ObjectPool에서 사용할 Projectile을 가져오는 메서드
          /// </summary>
          /// <remarks>
          /// 만약 Queue에 저장된 Object가 부족하다면 새로 생성 후 반환합니다.
          /// </remarks>
          /// 
          /// <returns>사용할 Projectile</returns>
          public Projectile GetProjectile(int projectileType)
          {
              if (projectileDict.TryGetValue(projectileType, out var projectileQueue))
              {
                  Projectile obj;

                  if (projectileQueue.Count > 0)
                      obj = projectileQueue.Dequeue();
                  else
                      obj = CreateNewProjectile(projectileType).GetComponent<Projectile>();

                  obj.gameObject.SetActive(true);
                  obj.transform.SetParent(null);

                  return obj;
              }
              // TryGetValue 실패 시
              else
                  Debug.LogError("ObjectPool.GetProjectile() is fall");

              return null;
          }

          /// <summary>
          /// 사용한 Projectile을 Queue에 반환하는 메서드
          /// </summary>
          /// <param name="projectile">반환한 Projectile</param>
          /// <param name="projectileType">반환하는 Projectile의 Type</param>
          public void ReturnProjectile(Projectile projectile, int projectileType)
          {
              if (projectileDict.TryGetValue(projectileType, out var projectileQueue))
              {
                  projectile.gameObject.SetActive(false);
                  projectile.transform.SetParent(transform);

                  projectileQueue.Enqueue(projectile);
              }
              // TryGetValue 실패 시
              else
                  Debug.LogError("ObjectPool.ReturnProjectile() is fall");
          }

          #endregion



          #region Unity Event

          /// <summary>
          /// Awake에서 실행할 작업을 구현하는 메서드
          /// </summary>
          protected override void OnAwakeWork()
          {
              Init();
          }


          #endregion
      }
      ```
    
    </div>
    </details>
  </br>

  * AudioManager  
    게임 내에서 사용되는 사운드를 관리하는 매니저입니다.
    <details>
    <summary>AudioManager Code</summary>
    <div markdown="1">

      ```c#
      using Cysharp.Threading.Tasks;
      using System;
      using System.Collections;
      using System.Collections.Generic;
      using System.Threading;
      using UnityEngine;
      using UnityEngine.SceneManagement;

      public enum SFXClip
      {
          Get = 0,            // 아이템 획득
          JumpItem,           // 점프 아이템 사용
          SaveItem,           // 세이브 아이템 사용
          PosChangeItem,      // 위치 변경 아이템 사용
          KeyDown,            // 소켓에 키 전달
          TUTO_OpenDoor,      // 튜토리얼 문 열림
          STAGE1_OpenWood,    // 1스테이지 나무 문 열림
          Alone_UP,           // Alone 상승
          Or_UP,              // Or 상승
          Together_UP,        // Together 상승
          STAGE1_OpenDoor,    // 1스테이지 문 열림
          RideCar,            // 차량 탑승
          CarExplosion,       // 차량 폭발
          PlayerDamage,       // 플레이어 데미지
          GrapplingHook,      // 훅 사용
          Button,             // 버튼 사용
          ContainerFall,      // 컨테이너 추락
          CarCrash,           // 차량 충돌
          KeyDrop,            // 열쇠 놓침
          Save,               // 세이브존 입장
          Text,               // 텍스트 오브젝트 활성화
          Wheel,              // 튜토리얼 장벽과 석상 상승, 1스테이지 석상 회전
          LakeMove,           // 1스테이지 호수 이동
          MovingPlatform,     // 착지 시 이동하는 플랫폼
          UIMenuClick,        // UI 메뉴 클릭
      }

      public enum BGMClip
      {
          Menu = 0,
          Tutorial,
          Stage1_1,
          Stage1_2,
          Stage2_1,
          Stage2_2,
          Credit
      }

      /// <summary>
      /// 게임내 사운드 출력을 위한 AudioSource를 관리하는 AudioManager 클래스
      /// </summary>
      /// 
      /// <remarks>
      /// AudioManager 클래스는 Singleton 클래스로 AudioSource를 관리합니다.
      /// 오디오를 재생할 오브젝트에서 재생할 오디오 클립과 loop 여부, 위치를 전달하면 AudioManager에서 재생해줍니다.
      /// </remarks>
      /// 
      /// @author yws
      /// @date last change 2023/05/20
      public class AudioManager : Singleton<AudioManager>
      {
          private class AudioData
          {
              AudioClip audioClip;
              bool loop;
              Vector3 pos;
              Transform parent;

              public AudioClip AudioClip { get { return audioClip; } }
              public bool Loop { get { return loop; } }
              public Vector3 Position { get { return pos; } }
              public Transform Parent { get { return parent; } }

              public AudioData(AudioClip clip, Vector3 pos, bool loop = false)
              {
                  audioClip = clip;
                  this.pos = pos;
                  this.loop = loop;
                  this.parent = null;
              }
              public AudioData(AudioClip clip, Transform parent, bool loop = false)
              {
                  audioClip = clip;
                  this.parent = parent;
                  this.loop = loop;
                  this.pos = Vector3.zero;
              }
          }

          protected AudioManager() { }

          #region Fields

          //- Private -
          private CancellationTokenSource _source;

          [SerializeField] [Range(0, 1)] private float master = 1;
          [SerializeField] [Range(0, 1)] private float sfx = 1;
          [SerializeField] [Range(0, 1)] private float bgm = 1;

          [SerializeField] private AudioClip[] sfxClips = new AudioClip[Enum.GetValues(typeof(SFXClip)).Length];
          [SerializeField] private AudioClip[] bgmClips = new AudioClip[Enum.GetValues(typeof(BGMClip)).Length];

          private Queue<AudioData> dateQueue = new Queue<AudioData>();

          private Queue<AudioSource> sfxQueue = new Queue<AudioSource>();

          private AudioSource bgmSource;

          //- Public -

          #endregion



          #region Property

          //- Public -
          public float Master
          {
              get { return master; }
              set
              {
                  master = value;
                  bgmSource.volume = master * bgm;
              }
          }
          public float Sfx { get { return sfx; } set { sfx = value; } }
          public float Bgm
          {
              get { return bgm; }
              set
              {
                  bgm = value;
                  bgmSource.volume = master * bgm;
              }
          }

          #endregion



          #region Methods
          //- Private -
          private void UpdateWork()
          {
              if (dateQueue.Count > 0)
                  PlaySfxSound(dateQueue.Dequeue()).Forget();
          }

          /// <summary>
          /// AudioData를 받아 재생해주는 메서드
          /// </summary>
          /// <param name="data">재생할 오디오 데이터</param>
          /// <returns></returns>
          private async UniTaskVoid PlaySfxSound(AudioData data)
          {
              AudioSource audio = sfxQueue.Count > 0 ? sfxQueue.Dequeue() : CreateAudioSource();

              audio.transform.position = data.Position;
              audio.clip = data.AudioClip;
              audio.loop = data.Loop;
              audio.volume = master * sfx;
              audio.transform.parent = data.Parent;

              audio.Play();

              await UniTask.Delay(TimeSpan.FromSeconds(audio.clip.length), ignoreTimeScale: false);

              if(!data.Loop)
                  audio.transform.parent = this.transform;

              sfxQueue.Enqueue(audio);
          }

          /// <summary>
          /// 초기화 메서드
          /// </summary>
          private void Init()
          {
              bgmSource = CreateAudioSource();

              for (int i = 0; i < 10; i++)
                  sfxQueue.Enqueue(CreateAudioSource());
          }

          /// <summary>
          /// 새로운 AudioSource 생성 메서드
          /// </summary>
          /// <returns></returns>
          private AudioSource CreateAudioSource()
          {
              var obj = new GameObject();
              obj.transform.parent = this.transform;
              var audio = obj.AddComponent<AudioSource>();
              audio.loop = false;

              return audio;
          }

          /// <summary>
          /// 씬 전환시 실행되는 메서드
          /// </summary>
          /// <param name="scene"></param>
          /// <param name="mode"></param>
          private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
          {
              switch(scene.name)
              {
                  case "Game_Start":
                  case "Game_Lobby":
                  case "Game_Waiting_Room":
                  case "Ending":
                      SetBGM(BGMClip.Menu);
                      break;
                  case "Tutorial":
                      SetBGM(BGMClip.Tutorial);
                      break;
                  case "Stage_1":
                      SetBGM(BGMClip.Stage1_1);
                      break;
                  case "Stage_2":
                      SetBGM(BGMClip.Stage2_1);
                      break;
                  case "Credit":
                      SetBGM(BGMClip.Credit);
                      break;
              }    
          }

          //- Public -

          /// <summary>
          /// BGM 설정 메서드
          /// </summary>
          /// <param name="data"></param>
          public void SetBGM(BGMClip clip, bool loop = true)
          {
              bgmSource.loop = loop;
              bgmSource.clip = bgmClips[(int)clip];

              bgmSource.Play();
          }

          /// <summary>
          /// SFX Sound 재생 메서드
          /// </summary>
          /// <param name="data"></param>
          public void AddSfxSoundData(SFXClip clip, bool loop, Vector3 pos)
          {
              if (sfxClips[(int)clip] != null)
                  dateQueue.Enqueue(new AudioData(sfxClips[(int)clip], pos, loop));
              else
                  Debug.Log("Audio Clip is Null");
          }

          /// <summary>
          /// 음원을 따라가는 SFX Sound 재생 메서드
          /// </summary>
          /// <param name="data"></param>
          public void AddSfxSoundData(SFXClip clip, bool loop, Transform transform)
          {
              if (sfxClips[(int)clip] != null)
                  dateQueue.Enqueue(new AudioData(sfxClips[(int)clip], transform, loop));
              else
                  Debug.Log("Audio Clip is Null");
          }

          /// <summary>
          /// 음소거
          /// </summary>
          public void MuteAll()
          {
              Master = 0;
              Bgm = 0;
              Sfx = 0;
          }

          public void MuteBgm()
          {
              Bgm = 0;
          }

          public void MuteSfx()
          {
              Sfx = 0;
          }

          #endregion



          #region UnityEvent

          private void OnEnable()
          {
              if (_source != null)
                  _source.Dispose();

              _source = new CancellationTokenSource();

              UpdateManager.SubscribeToUpdate(UpdateWork);

              GameManager.Instance.AddOnSceneLoaded(OnSceneLoaded);
          }

          private void OnDisable()
          {
              if (_source != null)
                  _source.Cancel();

              UpdateManager.UnsubscribeFromUpdate(UpdateWork);

              if(GameManager.Instance != null)
                  GameManager.Instance.RemoveOnSceneLoaded(OnSceneLoaded);
          }

          /// <summary>
          /// Awake에서 실행할 작업을 구현하는 메서드
          /// </summary>
          protected override void OnAwakeWork()
          {
              Init();
          }

          /// <summary>
          /// OnDestroyed에서 실행할 작업을 구현하는 메서드
          /// </summary>
          protected override void OnDestroyedWork()
          {
              base.OnDestroyedWork();

              if (_source != null)
              {
                  _source.Cancel();
                  _source.Dispose();
              }
          }

          #endregion
      }
      ```
    
    </div>
    </details>
  </br>
  
### Player
  * Movement  
  * PlayerCamera   
  * PlayerInput  
  ![player-min](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/628e45a4-5e3c-47d4-9773-8e81139389fc)   
  플레이어 애니메이션 및 이동, 카메라 처리를 구현했습니다.  
  Unity의 InputSystem을 사용하여 플레이어 Input 처리 스크립트를 작성하였습니다.  
    <details>
    <summary>PlayerInput Code</summary>
    <div markdown="1">

      ```c#
      using System.Collections;
      using System.Collections.Generic;
      using System.ComponentModel.Design.Serialization;
      using UnityEngine;
      using UnityEngine.InputSystem;

      /// <summary>
      /// InputSystem을 통해 플레이어 입력을 받는 스크립트
      /// </summary>
      /// <remarks>
      /// 입력을 받아서 플레이어에게 전달하는 역할입니다.
      /// </remarks>
      /// @date last change 2023/03/02
      /// @author yws
      /// @class PlayerInput
      public class P_Input : MonoBehaviour
      {
          #region Fields

          [SerializeField] private Vector2 move;
          [SerializeField] private Vector2 look;
          [SerializeField] private bool jump;
          [SerializeField] private bool sprint;
          [SerializeField] private bool use;
          [SerializeField] private bool mouseLeft;
          [SerializeField] private bool mouseRight;
          [SerializeField] private bool hook;
          [SerializeField] private bool holdStarted;
          [SerializeField] private bool holdCanceled;
          [SerializeField] private bool buttonStarted;
          [SerializeField] private bool buttonCanceled;

          private bool previousJump;
          private bool previousUse;
          private bool previousHook;

          private bool PBS; // previous button started
          private bool PBC; // previous button canceled
          private bool PHS; // previous hold started
          private bool PHC; // previous hold canceled

          #endregion

          #region Property

          public Vector2 Move { get { return move; } }
          public Vector2 Look { get { return look; } }
          public bool Jump
          {
              get
              {
                  jump = previousJump == jump ? false : true;
                  previousJump = jump;

                  return jump;
              }
          }
          public bool Sprint { get { return sprint; } }
          public bool Use
          {
              get
              {
                  use = previousUse == use ? false : true;
                  previousUse = use;

                  return use;
              }
          }
          public bool MouseLeft { get { return mouseLeft; } }
          public bool MouseRight { get { return mouseRight; } }
          public bool Hook
          {
              get
              {
                  hook = previousHook == hook ? false : true;
                  previousHook = hook;

                  return hook;
              }
          }
          public bool ButtonStarted {
              get
              {
                  buttonStarted = PBS == buttonStarted? false : true;
                  PBS = buttonStarted;
                  return buttonStarted;
              }
          }
          public bool ButtonCanceled {
              get
              {
                  buttonCanceled = PBC == buttonCanceled ? false : true;
                  PBC = buttonCanceled;
                  return buttonCanceled;
              }
          }
          public bool HoldStarted
          {
              get
              {
                  holdStarted = PHS == holdStarted ? false : true;
                  PHS = holdStarted;
                  return holdStarted;
              }
          }
          public bool HoldCanceled
          {
              get
              {
                  holdCanceled = PHC == holdCanceled ? false : true;
                  PHC = holdCanceled;
                  return holdCanceled;
              }
          }

          #endregion

          #region Methods

          /// <summary>
          /// 마우스 좌클릭 입력을 받는 함수
          /// </summary>
          /// <param name="context"></param>
          public void OnMouseLeft(InputAction.CallbackContext context)
          {
              mouseLeft = context.performed;
          }

          /// <summary>
          /// 마우스 우클릭 입력을 받는 함수
          /// </summary>
          /// <param name="context"></param>
          public void OnMouseRight(InputAction.CallbackContext context) 
          {
              mouseRight = context.started;
          }

          /// <summary>
          /// 키보드로 상하좌우 움직임을 받는 함수입니다
          /// </summary>
          /// <param name="value"></param>
          public void OnMove(InputAction.CallbackContext context)
          {
              move = context.ReadValue<Vector2>();
          }

          /// <summary>
          /// shift키를 누르는 여부에 따라 점프 여부를 받는 함수입니다
          /// </summary>
          /// <param name="value"></param>
          public void OnJump(InputAction.CallbackContext context)
          {
              jump = context.started;
          }

          /// <summary>
          /// 마우스 움직임에 따라 입력을 받는 함수입니다
          /// </summary>
          /// <param name="value"></param>
          public void OnLook(InputAction.CallbackContext context)
          {
              look = context.ReadValue<Vector2>();
          }

          /// <summary>
          /// 스페이스바를 누르는 여부에 따라 달리기 여부를 받는 함수입니다.
          /// </summary>
          /// <param name="value"></param>
          public void OnSprint(InputAction.CallbackContext context)
          {
              sprint = context.performed;
          }

          public void OnUse(InputAction.CallbackContext context)
          {
              use = context.started;
          }

          /// <summary>
          /// F키를 누르면 호출되는 함수로, 미션을 수행할 수 있습니다.
          /// </summary>
          /// <remarks>
          /// Action Type = Button, Interactions = Press, Trigger Behavior = Release Only
          /// </remarks>
          public void OnButton(InputAction.CallbackContext context)
          {
              //button = context.performed;
              buttonStarted = context.started;    // F키를 누를 동안 계속 호출
              buttonCanceled = context.canceled;  // F키를 떼고 나서 계속 호출
          }

          /// <summary>
          /// 마우스 오른쪽 키를 누르고 있을 때 호출되며, 이 함수를 통해 열쇠 오브젝트를 들고 내릴 수 있습니다.
          /// </summary>
          /// <remarks>
          /// Action Type = Button, Interactions = Press, Trigger Behavior = Release Only
          /// </remarks>
          public void OnHold(InputAction.CallbackContext context)
          {
              holdStarted = context.started;
              holdCanceled = context.canceled;
          }

          /// <summary>
          /// 
          /// </summary>
          /// <param name="context"></param>
          public void OnHook(InputAction.CallbackContext context)
          {
              hook = context.started;
          }

          #endregion
      }
      ```
    
    </div>
    </details>
  </br>

  * GrapplingHook
  ![gra1-min](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/4c8290b5-d915-4a7c-acd4-e9a375d676b4)
  ![gra2-min](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/2ce471ad-daf5-4fd0-b09b-686c64e3a057)  
  플레이어의 로프 액션에 사용되는 그래플링 훅입니다.  
  목표 지점으로 날아가는 액션과 목표 지점에 로프로 매달리는 액션을 구현하였습니다. 

### Contents
  * Car
    ![car5-min](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/4da816fa-c0e4-4141-a246-30be5f59335a)  
    2인승 자동차입니다.  
    운전석의 플레이어는 전/후 이동, 조수석의 플레이어는 좌/우 이동을 담당합니다.

  * Platform  
    ![plat](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/2e155e7d-1078-4359-bef7-51b6a0dae4bb)  
    이동하는 플랫폼의 네트워크 동기화를 구현하였습니다.  
    플레이어가 위에 올라탄 경우 플랫폼과 플레이어의 부모-자식 관계를 설정하여  
    플랫폼이 이동할 때 플레이어도 같이 이동하도록 구현하였습니다.
