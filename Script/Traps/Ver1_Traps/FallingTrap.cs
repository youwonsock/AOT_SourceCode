using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 밟으면 떨어지는 발판
/// </summary>
/// @author 이은수
/// @date last change 2022/10/
public class FallingTrap : MonoBehaviour
{
    Rigidbody rigid;
    Movement mv;

    private void Awake()
    {
        rigid = gameObject.GetComponentInChildren<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.TryGetComponent<Movement>(out mv);
            if (mv.Grounded)
            {
                rigid.useGravity = true;
                Invoke(nameof(DestroyTrap), 2.0f); // 2초 뒤에 파괴
            }
        }
    }
    void DestroyTrap()
    {
        Destroy(gameObject);
    }
}
