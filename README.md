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
    
    사용 시 플레이어를 점프시키는 아이템입니다.

  * SaveItem
  
    사용 시 세이브 포인트를 생성하는 아이템입니다.

### Manager
  * GameManager
    
    게임의 전체적인 흐름을 관리하는 매니저
    
  * UpdateManager
    
    최적화를 위해 각 컴포넌트에서 Update를 호출하는 것이 아닌 UpdateManager에 이벤트로 등록하여  
    UpdateManager에서 Update를 호출하는 방식으로 구현하였습니다.

  * ObjectPool
    * Projectile  
    
  
  오브젝트 풀링을 위한 매니저입니다.  
  오브젝트 풀에 오브젝트가 없는 경우 동적으로 생성하여 사용하고, 사용이 끝난 오브젝트는 오브젝트 풀에 반환합니다. 

  * AudioManager
    게임 내에서 사용되는 사운드를 관리하는 매니저입니다.
  
### Player
  * Movement
  * PlayerCamera
  * PlayerInput
  

  * GrapplingHook
  
    플레이어의 로프 액션에 사용되는 그래플링 훅입니다.  
    목표 지점으로 날라가는 액션과 목표 지점에 로프로 매달리는 액션을 구현하였습니다. 

### Contents
  * Car
  
    2인승 자동차입니다.  
    운전석의 플레이어는 전/후 이동, 조수석의 플레이어는 좌/우 이동을 담당합니다.

  * Platform
  
    이동하는 플랫폼의 네트워크 동기화를 구현하였습니다.  
    플레이어가 위에 올라탄 경우 플랫폼과 플레이어의 부모-자식 관계를 설정하여  
    플랫폼이 이동할 때 플레이어도 같이 이동하도록 구현하였습니다.