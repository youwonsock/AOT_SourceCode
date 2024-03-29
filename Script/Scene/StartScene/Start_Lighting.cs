using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 시작씬 조명 처리 
/// </summary>
/// @date last change 2023/05/27
/// @author LSM
/// @class Start_Lighting
public class Start_Lighting : MonoBehaviour
{
    [SerializeField]
    GameObject Connected;

    Vector3 CubePosition;
    Vector3 LookAtPosition;

    float xdiff;
    float ydiff;
    float zdiff;

    int randomx;
    int randomy;

    private void OnEnable()
    {
        UpdateManager.SubscribeToUpdate(UpdateWork);
    }

    public void OnDisable()
    {
        UpdateManager.UnsubscribeFromUpdate(UpdateWork);
    }

    // Start is called before the first frame update
    void Start()
    {
        CubePosition =Connected.GetComponent<Transform>().position;
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-10,10), 0, Random.Range(-10, 10)),ForceMode.Impulse);
    }

    // Update is called once per frame
    void UpdateWork()
    {
        xdiff = Mathf.Abs(CubePosition.x - transform.position.x);
        ydiff = Mathf.Abs(CubePosition.y - transform.position.y);
        zdiff = Mathf.Abs(CubePosition.z - transform.position.z);

        if (CubePosition.x > transform.position.x)
        {
            LookAtPosition.x = transform.position.x - xdiff;
        }
        else
        {
            LookAtPosition.x = transform.position.x + xdiff;
        }
        if (CubePosition.y > transform.position.y)
        {
            LookAtPosition.y = transform.position.y - ydiff;
        }
        else
        {
            LookAtPosition.y = transform.position.y + ydiff;
        }
        if (CubePosition.z > transform.position.z)
        {
            LookAtPosition.z = transform.position.z - zdiff;
        }
        else
        {
            LookAtPosition.z = transform.position.z + zdiff;
        }
        transform.LookAt(LookAtPosition);
    }
}
