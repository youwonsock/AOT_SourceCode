# Alone Or Together

## Developer Info
* 이름 : 유원석(You Won Sock)
* GitHub : https://github.com/youwonsock
* Mail : qazwsx233434@gmail.com

## Our Game
### Youtube



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
    
  * UpdateManager   
    ![UpdateManager](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/d11a9e65-e2ed-4edf-9967-6f742303cd05)  
    최적화를 위해 각 컴포넌트에서 Update를 호출하는 것이 아닌 UpdateManager에 이벤트로 등록하여  
    UpdateManager에서 Update를 호출하는 방식으로 구현하였습니다.

  * ObjectPool  
    * Projectile  
      
  ![object pool](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/57d5ea76-3651-4baa-8ffe-557f190ca5f5)  
  오브젝트 풀링을 위한 매니저입니다.  
  오브젝트 풀에 오브젝트가 없는 경우 동적으로 생성하여 사용하고, 사용이 끝난 오브젝트는 오브젝트 풀에 반환합니다. 

  * AudioManager  
    게임 내에서 사용되는 사운드를 관리하는 매니저입니다.
  
### Player
  * Movement  
  * PlayerCamera   
  * PlayerInput  
  ![input](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/4cf099ff-4f4f-4a67-abef-1f973b4715e1)   
  ![player-min](https://github.com/youwonsock/AOT_SourceCode/assets/46276141/628e45a4-5e3c-47d4-9773-8e81139389fc)   
  플레이어 애니메이션 및 이동, 카메라 처리를 구현했습니다.  
  Unity의 InputSystem을 사용하여 플레이어 Input 처리 스크립트를 작성하였습니다.  

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
