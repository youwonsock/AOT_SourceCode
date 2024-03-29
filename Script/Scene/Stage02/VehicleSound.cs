using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSound : MonoBehaviourPunCallbacks
{
    private AudioSource source;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
            source.Play();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
            source.Stop();
    }

    private void Update()
    {
        source.volume = (AudioManager.Instance.Master * AudioManager.Instance.Sfx);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
}
