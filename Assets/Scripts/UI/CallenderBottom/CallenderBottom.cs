using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CallenderBottom : MonoBehaviour
{
    [SerializeField] float Duration;
    [SerializeField] Ease ease;
    private void OnEnable()
    {
        transform.localPosition += new Vector3(0, -55, 0);
        transform.DOMoveY(transform.position.y + 55, Duration).SetEase(ease);
    }

    void Update()
    {
        
    }
}
