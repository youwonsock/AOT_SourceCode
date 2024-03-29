using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class VariablePlatform : MonoBehaviour
{

    #region Fields

    //- Private -
    [SerializeField] private bool activeInFPV;

    //- Public -

    #endregion



    #region Property
    //- Private -

    //- Public -
    #endregion



    #region Methods
    //- Private -

    private void ChangeAciveFPV()
    {
        gameObject.SetActive(activeInFPV);
    }

    private void ChangeAciveTPV()
    {
        gameObject.SetActive(!activeInFPV);
    }

    //- Public -

    #endregion



    #region Coroutine
    //- Private -

    //- Public -

    #endregion



    #region UnityEvent

    private void Awake()
    {
        GameManager.Instance.AddChangePVEvent(ChangeAciveFPV, 1);
        GameManager.Instance.AddChangePVEvent(ChangeAciveTPV, 3);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null)
            return;

        GameManager.Instance.RemoveChangePVEvent(ChangeAciveFPV, 1);
        GameManager.Instance.RemoveChangePVEvent(ChangeAciveTPV, 3);
    }

    #endregion
}
