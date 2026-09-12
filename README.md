# Alone Or Together

Unity와 Photon PUN을 기반으로 제작한 2인 협동 3D 멀티플레이 플랫폼 게임입니다.

## 목차

- [프로젝트 개요](#프로젝트-개요)
- [프로젝트 요약](#프로젝트-요약)
- [사용 기술](#사용-기술)
- [클래스 구조 UML](#클래스-구조-uml)
- [기능 상세](#기능-상세)
- [실행 및 저장소 안내](#실행-및-저장소-안내)

## 프로젝트 개요

| 항목 | 내용 |
| --- | --- |
| 개발 형태 | 팀 프로젝트 — [전체 개발 인원 입력 필요] |
| 담당 개발자 | 유원석 (You Won Sock) |
| GitHub | [youwonsock](https://github.com/youwonsock) |
| 이메일 | qazwsx233434@gmail.com |
| 개발 기간 | 2022.10 ~ 2023.05 |
| 프로젝트 목적 | 두 플레이어의 역할 분담과 협동 기믹을 중심으로 한 온라인 플랫폼 게임 구현 |
| 장르 | 3D Multiplayer Platformer |
| 개발 언어 | C# |
| 게임 엔진 | Unity — [사용 버전 입력 필요] |
| 주요 기술 | Photon PUN, Unity Input System, UniTask, DOTween |
| 지원 플랫폼 | Windows |
| 플레이 영상 | [YouTube](https://www.youtube.com/watch?v=gvBp5dKf-1s) |
| 실행 파일 | [Google Drive](https://drive.google.com/file/d/1AwyvXkEYygXMgfZHVrXq-GjrmRQxMoTE/view?usp=drive_link) |

## 프로젝트 요약

Alone Or Together는 두 플레이어가 서로 다른 역할을 수행하며 퍼즐과 장애물을 해결하는 온라인 협동 게임입니다. 플레이어 이동·카메라·입력 처리부터 Photon 기반 로비와 동기화, 그래플링 훅, 아이템, 이동 플랫폼, 2인승 차량, 공통 매니저와 UI까지 게임 플레이에 필요한 클라이언트 시스템을 구현했습니다.

주요 실행 흐름은 다음과 같습니다.

1. Photon 접속 및 방 생성·입장을 처리하고 대기실에서 플레이어 상태를 관리합니다.
2. 로컬 플레이어의 입력을 이동, 카메라, 상호작용, 그래플링 시스템에 전달합니다.
3. RPC와 상태 직렬화를 이용해 플레이어 및 협동 콘텐츠의 상태를 다른 클라이언트와 동기화합니다.
4. 아이템, 퍼즐, 이동 플랫폼, 차량 등 두 플레이어가 함께 해결하는 스테이지 콘텐츠를 진행합니다.
5. GameManager, UpdateManager, ObjectPool, AudioManager가 공통 게임 흐름과 반복 자원을 관리합니다.

[![Alone Or Together 플레이 영상](https://img.youtube.com/vi/gvBp5dKf-1s/0.jpg)](https://www.youtube.com/watch?v=gvBp5dKf-1s)

> **스크린샷 플레이스홀더** — 대표 게임 화면 추가 예정

## 사용 기술

### Unity / C#

MonoBehaviour 기반 컴포넌트 구조로 플레이어, 아이템, 퍼즐, 차량, 플랫폼과 UI를 구성했습니다. 공통 기능은 인터페이스와 매니저로 분리하고, 프로젝트 스크립트는 `AloneOrTogether.asmdef` 어셈블리로 관리합니다.

### Photon PUN

Photon 접속, 방 생성·입장, 대기실과 씬 전환을 처리합니다. `PhotonView`, RPC, `IPunObservable`을 이용해 플레이어 행동과 그래플링 상태, 이동 플랫폼, 차량 등 협동 콘텐츠의 상태를 동기화합니다.

### Unity Input System

`P_Input`이 이동, 시점, 점프, 달리기, 상호작용과 그래플링 입력을 수집합니다. 입력 상태를 플레이어 기능에 전달하여 입력 처리와 실제 행동 로직을 분리했습니다.

### UniTask / DOTween

UniTask로 그래플링과 사운드 반환처럼 시간 흐름이 필요한 작업을 비동기로 처리합니다. DOTween은 웨이포인트 기반 플랫폼 이동과 반복·왕복 동작을 구성하는 데 사용합니다.

### 공통 매니저

`GameManager`가 씬 로드, 리스폰 지점과 시점 전환 이벤트를 관리합니다. `UpdateManager`는 Update 계열 콜백을 이벤트로 모으고, `ObjectPool`과 `AudioManager`는 반복 생성되는 발사체와 AudioSource를 재사용합니다.

## 클래스 구조 UML

클래스 구조 원본은 [`UML.plantuml`](UML.plantuml)에서 확인할 수 있습니다.

핵심 관계는 다음과 같습니다.

- `Player` → `Movement`, `P_Input`, `PlayerCamera`, `Inventory`, `GrapplingHook`
- `Player` → `IPunObservable`을 통한 네트워크 상태 직렬화
- `GrapplingHook` → 목표 지점 판정, 이동·스윙 처리, 로프 표현과 RPC 동기화
- `ProjectileDispenser` → `Projectile_SO`, `ObjectPool`, `Projectile`
- `GameManager`, `UpdateManager`, `AudioManager` → 전역 게임 흐름과 공통 서비스 관리

> **UML 이미지 플레이스홀더** — `UML.plantuml` 렌더링 이미지 추가 예정

### 클래스별 역할

- `GameManager`: 씬 로드 이벤트, 리스폰 지점, 애니메이션 해시와 1·3인칭 시점 전환 이벤트를 관리합니다.
- `UpdateManager`: Update, FixedUpdate, LateUpdate 구독 콜백을 한 곳에서 호출합니다.
- `Player`: 로컬·원격 플레이어 상태와 플레이어 하위 기능을 조정합니다.
- `Movement`: 걷기, 달리기, 점프, 중력과 회전 보간을 처리합니다.
- `P_Input`: Unity Input System 콜백을 게임에서 사용할 입력 상태로 변환합니다.
- `PlayerCamera`: 플레이어 시점과 카메라 회전을 처리합니다.
- `Inventory`: 획득한 아이템과 사용 흐름을 관리합니다.
- `GrapplingHook`: 발사, 목표 지점 이동, 스윙, 종료와 로프 렌더링을 처리합니다.
- `ObjectPool`: 발사체를 타입별 Queue에 보관하고 대여·반환합니다.
- `AudioManager`: BGM과 SFX 재생, 볼륨, AudioSource 재사용을 관리합니다.
- `WaypointMoving`: 웨이포인트 이동과 플랫폼 탑승 상태를 네트워크로 동기화합니다.

## 기능 상세

### Photon 멀티플레이 흐름

**목적**

두 플레이어가 같은 방에 접속해 대기실을 거쳐 동일한 게임 씬을 진행하도록 구성합니다.

**핵심 구현**

- `Connect`에서 Photon 서버 접속을 시작합니다.
- `CreateAndJoin`이 방 생성과 입장 요청을 처리합니다.
- `Waiting_Room`이 참가자 상태와 게임 시작 조건을 관리합니다.
- `Scene_Load`가 네트워크 플레이 흐름에 맞춰 씬 전환을 처리합니다.
- 게임 콘텐츠는 `PhotonView`, RPC와 상태 직렬화를 사용해 클라이언트 간 상태를 공유합니다.

> **스크린샷 플레이스홀더** — 로비 및 대기실 화면 추가 예정

### 플레이어 이동·입력·카메라

**목적**

로컬 플레이어의 이동과 시점, 점프, 상호작용 입력을 기능별 컴포넌트에 전달합니다.

**핵심 구현**

- `P_Input`이 Input System 콜백에서 이동, 시점, 점프, 달리기와 상호작용 입력을 수집합니다.
- `Movement`가 걷기·달리기 속도, 회전 보간, 중력과 다중 점프를 처리합니다.
- `PlayerCamera`와 시점 변경 인터페이스를 이용해 1인칭·3인칭 전환을 구성합니다.
- 애니메이터 파라미터 해시는 `GameManager`에서 공통으로 관리합니다.

![플레이어 이동 및 카메라](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/628e45a4-5e3c-47d4-9773-8e81139389fc)

### 그래플링 훅

**목적**

플레이어가 목표 지점으로 빠르게 이동하거나 로프에 매달려 스윙할 수 있는 액션을 제공합니다.

**핵심 구현**

- Raycast로 사거리 안의 그래플링 목표 지점을 판정합니다.
- 목표 지점으로 이동하는 Hook 동작과 로프에 매달리는 Swing 동작을 분리했습니다.
- 발사와 종료 상태를 RPC로 다른 클라이언트에 전달합니다.
- LineRenderer와 Spring 계산으로 로프의 이동과 파동을 표현합니다.
- 씬별 그래플링 거리를 설정해 스테이지 환경에 맞게 동작 범위를 조정합니다.

![그래플링 훅 이동](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/4c8290b5-d915-4a7c-acd4-e9a375d676b4)

![그래플링 훅 스윙](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/2ce471ad-daf5-4fd0-b09b-686c64e3a057)

### 아이템 및 세이브 지점

**목적**

플레이어가 아이템을 획득·사용하고 진행 중 리스폰 지점을 변경할 수 있도록 합니다.

**핵심 구현**

- 공통 `Item` 클래스로 획득과 활성 상태 변경 흐름을 구성합니다.
- 로컬 소유 플레이어의 충돌만 아이템 획득으로 처리합니다.
- `JumpItem`은 사용한 플레이어에게 점프 효과를 적용합니다.
- `SaveItem`은 저장 지점 오브젝트를 생성해 이후 리스폰 위치로 사용합니다.
- 아이템 사용 효과음은 `AudioManager`를 통해 재생합니다.

![점프 아이템](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/6385c7a1-e335-4143-bb00-8a507b7ad81c)

![세이브 아이템](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/f220a099-570c-4564-9bd3-0ae0ff54cee7)

### 중앙 Update 관리

**목적**

여러 컴포넌트에 분산된 Unity Update 계열 호출을 공통 진입점에서 관리합니다.

**핵심 구현**

- `UpdateManager`가 Update, FixedUpdate, LateUpdate 이벤트를 제공합니다.
- 각 기능은 활성화 시 필요한 콜백을 구독하고 비활성화 시 해제합니다.
- 차량과 AudioManager 등 반복 갱신이 필요한 기능이 공통 호출 흐름을 사용합니다.
- 구독과 해제 메서드를 분리해 컴포넌트 생명주기에 맞춰 관리합니다.

> **스크린샷 플레이스홀더** — UpdateManager 동작 구조 또는 프로파일링 화면 추가 예정

### 발사체 오브젝트 풀

**목적**

반복해서 생성·삭제되는 발사체를 재사용해 런타임 할당과 생성 비용을 줄입니다.

**핵심 구현**

- 발사체 타입별 Queue를 생성하고 초기 오브젝트를 미리 준비합니다.
- 요청한 타입의 Queue가 비어 있으면 새 발사체를 생성합니다.
- 사용이 끝난 발사체는 비활성화한 뒤 해당 Queue로 반환합니다.
- `Projectile_SO`와 `ProjectileDispenser_SO`로 발사체 설정 데이터를 분리합니다.

> **스크린샷 플레이스홀더** — 발사체 및 ObjectPool 동작 화면 추가 예정

### 2인 협동 차량

**목적**

두 플레이어가 서로 다른 조작을 담당해야 운행할 수 있는 협동 콘텐츠를 구현합니다.

**핵심 구현**

- 운전석 플레이어는 전진·후진을 담당합니다.
- 조수석 플레이어는 좌·우 조향을 담당합니다.
- Master Client를 기준으로 차량 동작을 제어하고 RPC로 입력과 상태를 공유합니다.
- 가속, 제동, 조향, 드리프트와 리스폰 동작을 차량 컨트롤러에서 처리합니다.

![2인 협동 차량](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/4da816fa-c0e4-4141-a246-30be5f59335a)

### 네트워크 이동 플랫폼

**목적**

이동하는 발판과 탑승 플레이어가 각 클라이언트에서 일관된 위치 관계를 유지하도록 합니다.

**핵심 구현**

- DOTween 웨이포인트로 이동 경로, 시간, 대기, 반복과 왕복 방식을 설정합니다.
- Master Client에서 플랫폼 이동을 시작합니다.
- `IPunObservable`을 이용해 플랫폼의 활성 상태를 동기화합니다.
- 탑승 시 플레이어와 플랫폼의 부모·자식 관계를 설정하고 RPC로 관계 변경을 공유합니다.
- 하차 시 부모 관계를 해제해 플레이어가 독립적으로 이동하도록 복원합니다.

![네트워크 이동 플랫폼](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/2e155e7d-1078-4359-bef7-51b6a0dae4bb)

## 실행 및 저장소 안내

이 저장소는 포트폴리오 검토를 위한 **담당 C# 스크립트와 UML 중심의 소스 공개 저장소**입니다. Unity 프로젝트 전체에 필요한 Scene, Prefab, Package 설정과 아트 에셋이 포함되어 있지 않아 이 저장소만으로는 Unity Editor에서 직접 빌드할 수 없습니다.

게임 실행본은 [Google Drive](https://drive.google.com/file/d/1AwyvXkEYygXMgfZHVrXq-GjrmRQxMoTE/view?usp=drive_link)에서 내려받을 수 있으며, 전체 플레이 흐름은 [YouTube 영상](https://www.youtube.com/watch?v=gvBp5dKf-1s)에서 확인할 수 있습니다.

### 소스 코드 구성

- [`Script/Player`](Script/Player): 플레이어, 이동, 입력, 카메라, 인벤토리, 그래플링 훅
- [`Script/Manager`](Script/Manager): 게임, Update, 오디오, 셰이더와 오브젝트 풀 관리
- [`Script/Photon Server`](Script/Photon%20Server): 서버 접속, 방 생성·입장, 대기실과 씬 전환
- [`Script/Item`](Script/Item): 공통 아이템, 점프 아이템, 세이브 아이템
- [`Script/Platform`](Script/Platform): 웨이포인트 이동과 네트워크 플랫폼
- [`Script/Scene`](Script/Scene): 스테이지별 퍼즐, 차량과 진행 콘텐츠
- [`Script/Traps`](Script/Traps): 발사체와 트랩
- [`Script/UI`](Script/UI): HUD, 인벤토리, 시점·감도·사운드 설정 UI
