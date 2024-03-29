using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 플레이어의 체력 UI를 수정하는 View 입니다.
/// </summary>
/// @date last change 2023/05/30
/// @author MW
/// @class UI

public class UI_PlayerView : MonoBehaviour
{
    public Image hearts;
    /**
     * @brief 플레이어의 최대 체력에 대한 현재 체력의 비율로 체력 UI를 수정합니다.
     * @param float _currentHealth 플레이어의 현재 체력
     * @param float _maxHealth 플레이어의 최대 체력
     */
    public void UpdateHealthBar(float _currentHealth, float _maxHealth)
    {
        hearts.fillAmount = _currentHealth / _maxHealth;
    }
}
