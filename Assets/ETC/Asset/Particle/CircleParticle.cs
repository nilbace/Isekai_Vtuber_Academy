using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleParticle : MonoBehaviour
{
    public float DisableTime;
    public void Start()
    {
        StopAllCoroutines();
        StartCoroutine(DisableCor());
    }

    IEnumerator DisableCor()
    {
        yield return new WaitForSeconds(DisableTime);
        gameObject.SetActive(false);
    }
}
