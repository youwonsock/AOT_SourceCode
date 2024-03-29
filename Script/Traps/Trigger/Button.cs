using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// 버튼을 밟고 있는 동안만 문이 열립니다.
/// </summary>
/// <remarks>
/// 버튼을 밟고 있는지 상태를 읽어와 문의 위치를 이동시킵니다.
/// </remarks>
/// @author 이민우
/// @date last date 22/12/06
/// 
/**
     * @param isPressed 버튼이 눌리고 있는지 검사합니다.
     * @param left/right_door 여닫히는 오브젝트 입니다.
     * @param isStart 버튼이 한번도 눌리지 않은 상태를 검사합니다.
     */
public class ButtonScript : MonoBehaviour
{
    
    [SerializeField] private bool isPressed;
    [SerializeField] private GameObject left_door;
    [SerializeField] private GameObject right_door;
    private bool isStart;


    private void Awake()
    {
        isPressed = false;
        isStart = false;
    }

    /**
     * @details isStart로 버튼이 한번도 눌리지 않았는지 우선 검사합니다.
     * 버튼이 눌리거나 떼지면 문이 여닫히도록 오브젝트를 움직입니다.
     * @bug 문이 움직일 때 약간 출렁이면서 움직입니다.
     */
    void FixedUpdate()
    {
        Vector3 velo = Vector3.zero;
        if (isStart)
        {
            switch (isPressed)
            {
                case true:
                    transform.position = Vector3.Lerp(transform.position, new Vector3(8.15f, 0.15f, 3.5f), Time.deltaTime);
                                      
                    left_door.transform.position = Vector3.MoveTowards(left_door.transform.position, new Vector3(6.7f, 1.2f, 11), Time.deltaTime);
                    right_door.transform.position = Vector3.MoveTowards(right_door.transform.position, new Vector3(9.7f, 1.2f, 11), Time.deltaTime);
                    break;

                case false:
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(8.15f, 0.25f, 3.5f), ref velo, 0.1f);

                    left_door.transform.position = Vector3.MoveTowards(left_door.transform.position, new Vector3(7.7f, 1.25f, 10.7f), Time.deltaTime);
                    right_door.transform.position = Vector3.MoveTowards(right_door.transform.position, new Vector3(8.7f, 1.25f, 10.7f), Time.deltaTime);
                    break;
            }
        }

        
    }
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            isPressed = true;
            isStart = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPressed = false;
        }
    }


}
