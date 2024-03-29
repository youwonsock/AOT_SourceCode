using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// 플레이어가 밟으면 플레이어를 날려버리는 함정
/// </summary>
/// <remarks>
/// 발판을 갖고있는 실린더가 회전하면서 플레이어를 위로 날립니다.
/// </remarks>
/// @author 이민우
/// @date last date 22/12/06
public class Trap4 : MonoBehaviour
{
    Rigidbody my_rb;

    void Start()
    {
        my_rb = GetComponent<Rigidbody>();
        my_rb.angularDrag = 4.0f;

    }
    /**
     * @details 플레이어가 발판을 밟으면 실행.
     * 발판이 누워있고 정지 상태일 경우에,
     * 플레이어와 닿을 경우 실린더를 회전시키고,
     * 플레이어를 위로 날립니다.
     * 공중에서 낙하하며 발판과 닿았을 때,
     * 다시 날아가고, 발판이 회전하지 않도록 하기 위함입니다.
     */
    private void OnTriggerEnter(Collider other)
    {
        float x_rotate = transform.eulerAngles.x;
        if (other.CompareTag("Player"))
        {
            if(x_rotate <= 90 && 0<= x_rotate && my_rb.IsSleeping())
                //발판이 누워있을 경우 and 정지상태
            {
                my_rb.AddTorque(Vector3.right * 12, ForceMode.Impulse);
                Rigidbody other_rb = other.GetComponent<Rigidbody>();
                other_rb.AddForce(Vector3.up * 12, ForceMode.Impulse);
            }
        }
    }

    /**
     *@details 발판이 세워져있는 경우를 검사합니다.
     *플레이어와 충돌하여 발판이 세워져 있고, 정지 하였을 때,
     *실린더를 회전시켜 발판을 원래 위치로 돌려놓습니다.
     */
    void FixedUpdate()
    {
        float x_rotate = transform.eulerAngles.x;
        if (0 < x_rotate)
            if (10 < x_rotate && my_rb.IsSleeping())
                //발판이 세워져있을 경우 and 정지 상태
            {
                my_rb.AddTorque(Vector3.left * 11, ForceMode.Impulse);
            }
        
    }
}
