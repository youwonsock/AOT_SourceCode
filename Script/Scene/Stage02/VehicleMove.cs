using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 2스테이지의 장애물의 이동을 맡은 스크립트
/// </summary>
/// @author LSM
/// @date last change 2023/04/23
/// @class VehicleMove
public class VehicleMove : MonoBehaviour
{
    [SerializeField] Transform[] movePos;
    [SerializeField] float speed;
    int index = 1;

    private void OnEnable()
    {
        UpdateManager.SubscribeToUpdate(UpdateWork);
    }

    private void OnDisable()
    {
        UpdateManager.UnsubscribeFromUpdate(UpdateWork);
    }

    // Update is called once per frame
    void UpdateWork()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePos[index].transform.position, speed * Time.deltaTime);
        transform.LookAt(movePos[index].transform.position);
        if (transform.position == movePos[index].transform.position)
            index++;
        if (index == movePos.Length)
            index = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamageAble>().TakeDamage(transform.position, 25f, 5f);
        }
    }
}
