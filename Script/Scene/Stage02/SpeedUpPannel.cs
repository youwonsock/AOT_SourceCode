using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpPannel : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Car"))
        {
            collision.gameObject.TryGetComponent<IChangeCarSpeed>(out IChangeCarSpeed changeCarSpeed);
            changeCarSpeed?.ChangeCarSpeed();
        }
    }
}
