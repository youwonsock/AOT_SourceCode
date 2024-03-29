using System.Collections;
using UnityEngine;

public class ActiveEvent : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Move());

        //AudioManager.Instance.AddSfxSoundData(SFXClip.MoveRock,false,this.transform.position);
    }

    IEnumerator Move()
    {
        var wfs = new WaitForSecondsRealtime(0.02f);
        Vector3 des = new Vector3(85, -31, 116);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, des, 0.1f);

            if(Vector3.Distance(des, transform.position) < 1)
                break;
            
            yield return wfs;
        }

        yield return null;
    }
}


