using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Stamp : MonoSingleton<UI_Stamp>
{
    public enum StampState
    {
        transparent = -1,
        Fail,
        Success,
        BicSuccess
    }
    public Image StampIMG;
    public Sprite[] TempStampImg;
    public float StampResetTime;
    public Ease StampEase;
   
    void Start()
    {
        SetStamp(StampState.transparent);
    }

    public void SetStamp(StampState StampState)
    {
        switch (StampState)
        {
            case StampState.transparent:
                StampIMG.gameObject.SetActive(false);
                break;
            case StampState.Fail:
                StampIMG.gameObject.SetActive(true);
                StampIMG.sprite = TempStampImg[0];
                break;
            case StampState.Success:
                StampIMG.gameObject.SetActive(true);
                StampIMG.sprite = TempStampImg[1];
                break;
            case StampState.BicSuccess:
                StampIMG.gameObject.SetActive(true);
                StampIMG.sprite = TempStampImg[2];
                break;
        }
        StampIMG.transform.localScale = Vector3.one * 5f;
        StampIMG.transform.DOScale(1, StampResetTime).SetEase(StampEase);
    }
}
