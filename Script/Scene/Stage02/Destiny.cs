using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 엔딩을 결정 짓기 위해 플레이어를 분리시키는 스크립트
/// </summary>
/// 
/// @date last change 2023/05/27
/// @author LSM
/// @class Destiny
public class Destiny : MonoBehaviour
{
    [SerializeField] GameObject destiny;
    [SerializeField] GameObject partition;

    [SerializeField] GameObject endinginfo;

    bool button = false;

    public bool Button { get { return button; } }

    private void OnEnable()
    {
        UpdateManager.SubscribeToUpdate(UpdateWork);
    }

    private void OnDisable()
    {
        UpdateManager.UnsubscribeFromUpdate(UpdateWork);
    }

    void UpdateWork()
    {
        if (button)
        {
            if (partition.transform.position.y < 200)
                partition.transform.position += new Vector3(0, 0.1f, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            destiny.GetComponent<BoxCollider>().enabled = false;
            button = true;
            endinginfo.SetActive(true);
        }
    }
}