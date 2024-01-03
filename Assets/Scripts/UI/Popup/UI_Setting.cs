using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    public EasyTransition.TransitionSettings transition;

    enum Buttons
    {
        Googlelink, CloudSave, CloudLoad, Achievement, Credit, CloseBTN, ResetBTN
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
        GetButton((int)Buttons.ResetBTN).onClick.AddListener(ResetBTN);
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

    void ResetBTN()
    {
        StartCoroutine(ResetGame());
    }

    IEnumerator ResetGame()
    {
        EasyTransition.TransitionManager.Instance().Transition(transition, 0);
        yield return new WaitForSeconds(1f);
        Managers.UI_Manager.CloseALlPopupUI();
        Managers.Data._myPlayerData.ResetData();
        Managers.Data.SaveData();
        UI_MainBackUI.instance.UpdateUItexts();
    }
}
