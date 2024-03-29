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