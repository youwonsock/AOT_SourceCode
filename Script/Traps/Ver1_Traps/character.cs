using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 간이 캐릭터입니다.
/// </summary>
/// <remarks>
/// 입력에 따라 간단하게 움직이는 오브젝트입니다.
/// </remarks>
/// @author 이민우
/// @date last date 22/09/30
public class character : MonoBehaviour
{
    float m_fSpeed = 5.0f;

    void Update()
    {
        float fHorizontal = UnityEngine.Input.GetAxis("Horizontal");
        float fVertical = UnityEngine.Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * Time.deltaTime * m_fSpeed * fHorizontal, Space.World);
        transform.Translate(Vector3.forward * Time.deltaTime * m_fSpeed * fVertical, Space.World);

        Jump();

    }
    void Jump()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();
            rb.AddForce(Vector3.up * 18, ForceMode.Impulse);
        }
    }
}
