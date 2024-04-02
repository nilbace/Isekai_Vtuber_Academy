using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    private const string BGMVolumeKey = "BGMVolume";
    private const string SFXVolumeKey = "SFXVolume";

    enum Buttons
    {
        Googlelink, CloudSave, CloudLoad, Achievement, Credit, CloseBTN, ResetBTN, CouponBTN
    }

    public Slider BGMSlider;
    public Slider SFXSlider;

    private void Start()
    {
        Init();
    }

    
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        BGMSlider = GameObject.Find("BGMSlider").GetComponent<Slider>();
        SFXSlider = GameObject.Find("SFXSlider").GetComponent<Slider>();


        GetButton((int)Buttons.CloseBTN).onClick.AddListener(CloseBTN);
        GetButton((int)Buttons.ResetBTN).onClick.AddListener(ResetBTN);
        GetButton((int)Buttons.CouponBTN).onClick.AddListener(CouponBTN);

        //이름 바뀐거 맞음 
        BGMSlider.onValueChanged.AddListener(BGM_ValueChanged);
        BGMSlider.value = LoadBGMVolume();
        SFXSlider.onValueChanged.AddListener(SFX_ValueChanged);
        SFXSlider.value = LoadSFXVolume();
    }

    void CloseBTN()
    {
        Managers.UI_Manager.ClosePopupUI();
        Managers.Sound.Play(Define.Sound.SmallBTN);
    }

    void CouponBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        Managers.UI_Manager.ShowPopupUI<UI_CouponPopup>();
    }

    void ResetBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        Managers.UI_Manager.ShowPopupUI<UI_ConfirmReset>();
    }

    void BGM_ValueChanged(float value)
    {
        Managers.Sound.ChangeVolume(Define.Sound.Bgm, value);
        PlayerPrefs.SetFloat(BGMVolumeKey, value);
        PlayerPrefs.Save();
    }

    void SFX_ValueChanged(float value)
    {
        Managers.Sound.ChangeVolume(Define.Sound.Effect, value);
        PlayerPrefs.SetFloat(SFXVolumeKey, value);
        PlayerPrefs.Save();
    }

    public float LoadBGMVolume()
    {
        if (PlayerPrefs.HasKey(BGMVolumeKey))
        {
            return PlayerPrefs.GetFloat(BGMVolumeKey);
        }
        else
        {
            return 1.0f; // 기본값 설정
        }
    }

    public float LoadSFXVolume()
    {
        if (PlayerPrefs.HasKey(SFXVolumeKey))
        {
            return PlayerPrefs.GetFloat(SFXVolumeKey);
        }
        else
        {
            return 1.0f; // 기본값 설정
        }
    }
}
