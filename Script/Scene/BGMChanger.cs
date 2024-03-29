using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChanger : MonoBehaviour
{
    [SerializeField] private BGMClip bgmClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.SetBGM(bgmClip);
            Destroy(this.gameObject);
        }
    }
}
