using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    #region Fields

    // 나중에 custom editor를 통해 인스펙터에서 수정 가능하도록 변경 예정
    IActivable trap;

    #endregion



    #region Methods


    #endregion



    #region Unity Event

    private void Awake()
    {
        transform.parent.TryGetComponent<IActivable>(out trap);

        if (trap == null)
            Debug.Log("get component fail!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        trap.ActivateObject(this.transform.position);
    }

    #endregion
}
