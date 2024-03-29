using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 2스테이지 문을 여는 스크립트
/// </summary>
/// @date last change 2023/05/27
/// @author LSM
/// @class Open
public class Open : MonoBehaviour
{
    [SerializeField] GameObject a;
    [SerializeField] GameObject b;
    [SerializeField] GameObject RespawnPoint;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            a.transform.Translate(new Vector3(447, 0.4f, 67.2f));
            b.transform.Translate(new Vector3(447, 0.4f, 224.8f));
            RespawnPoint.transform.position = new Vector3(538.51f, 2.4f, -140.04f);
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
