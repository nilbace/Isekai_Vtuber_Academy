using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    enum Buttons
    {
        Googlelink, CloudSave, CloudLoad, Achievement, Credit, CloseBTN
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
        BGMSlider.onValueChanged.AddListener(BGM_ValueChanged);
        SFXSlider.onValueChanged.AddListener(SFX_ValueChanged);
    }

    void CloseBTN()
    {
        Managers.UI_Manager.ClosePopupUI();
    }

    void BGM_ValueChanged(float value)
    {
        Managers.Sound.ChangeVolume(Define.Sound.Bgm, value*0.2f);
    }

    void SFX_ValueChanged(float value)
    {
        Managers.Sound.ChangeVolume(Define.Sound.Effect, value);
    }
}
