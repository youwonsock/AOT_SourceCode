using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 열쇠의 움직임을 만듭니다.
/// </summary>
/// <remarks>
/// 열쇠가 위아래로 움직이고, 회전하게 합니다.
/// </remarks>
/// @author 이민우
/// @date last date 22/10/10
/// 

/**
     * @param degree 열쇠의 회전 속도
     * @param door 열쇠를 획득했을 때 열리는 문
     * @param key 열쇠입니다. 획득했을 경우 object를 destroy합니다
     * @param delta 상하로 움직이는 열쇠의 y값 한계
     * @param speed 상하로 움직이는 열쇠의 속도
     */
public class KeyMoving : MonoBehaviour
{
    
    [SerializeField] private float degree;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject key;
    [SerializeField] private float delta;
    [SerializeField] private float speed;
    private Vector3 pos;

    private void Awake()
    {
        pos = transform.position;
        delta = 0.3f;
        speed = 3f;
    }


    void Update()
    {
        transform.Rotate(new Vector3(1, 1, 0) * Time.deltaTime * degree);
        Vector3 v = pos;
        v.y = 1.2f + delta * Mathf.Sin(Time.time * speed );
        transform.position = v;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            door.GetComponent<DoorOpen_key>().isGotKey = true;
            Destroy(key);
        }
    }
}
