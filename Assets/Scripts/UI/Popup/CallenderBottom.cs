using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CallenderBottom : MonoBehaviour
{
    public static CallenderBottom instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] float Duration;
    [SerializeField] Ease ease;
    

    public void Init()
    {
        UI_MainBackUI.instance.CleanSeals();
        transform.localPosition += new Vector3(0, -55, 0);
        transform.DOMoveY(transform.position.y + 55, Duration).SetEase(ease);
    }
}
