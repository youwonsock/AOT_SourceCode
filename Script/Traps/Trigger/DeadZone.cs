using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviourPunCallbacks
{
    private float damage = 25f;
    [SerializeField] List<HandMission> keys = new();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamageAble>().TakeDamage(transform.position, damage, 0);
            other.gameObject.transform.position = GameManager.Instance.RespawnPoint;

            for (int i = 0; i < keys.Count; i++)
                if (keys[i] != null && keys[i].gameObject.GetComponent<Collider>().enabled)
                    keys[i].OnDeadzone();
        }
    }
}
