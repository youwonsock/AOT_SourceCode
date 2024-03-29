/// <summary>
/// 미션오브젝트용 인터페이스
/// </summary>
/// <remarks>
/// IsMine 검사를 하지 않는 ActivatePuzzleTogether()은 
/// 한 명이 충돌구간에 진입하면, 모든 플레이어에서 함수가 동시에 호출됩니다.
/// </remarks>
/// 
/// @author 이은수
/// @date last change 2023/04/03
public interface IMission
{
    void ActivatePuzzle(MissionInPlayer player, int type); // IsMine 검사 o
    void ActivatePuzzleTogether(MissionInPlayer player, bool enter); // IsMine 검사 x 
}

public enum TriggerType
{
    Enter = 1, Stay = 2, Exit = 3,
}
