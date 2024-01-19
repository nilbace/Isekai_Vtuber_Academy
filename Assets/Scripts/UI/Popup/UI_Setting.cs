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
        GetButton((int)Buttons.ResetBTN).onClick.AddListener(ResetBTN);

        //이름 바뀐거 맞음 
        BGMSlider.onValueChanged.AddListener(SFX_ValueChanged);
        SFXSlider.onValueChanged.AddListener(BGM_ValueChanged);
        
    }

    void CloseBTN()
    {
        Managers.UI_Manager.ClosePopupUI();
        Managers.Sound.Play(Define.Sound.SmallBTN);
    }

    void BGM_ValueChanged(float value)
    {
        Managers.Sound.ChangeVolume(Define.Sound.Bgm, value);
    }

    void SFX_ValueChanged(float value)
    {
        Managers.Sound.ChangeVolume(Define.Sound.Effect, value);
    }

    void ResetBTN()
    {
        StartCoroutine(ResetGame());
        Managers.Sound.Play(Define.Sound.ReturnBTN);
    }

    IEnumerator ResetGame()
    {
        EasyTransition.TransitionManager.Instance().Transition(transition, 0);
        yield return new WaitForSeconds(1f);


        Managers.UI_Manager.CloseALlPopupUI();
        Managers.Data.PlayerData = new Define.PlayerData();
        Managers.Data.SaveData();
        UI_MainBackUI.instance.UpdateUItexts();
    }
}
