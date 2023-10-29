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

    public Slider volumeSlider;

    private void Start()
    {
        Init();
    }

    
    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        
        GetButton((int)Buttons.CloseBTN).onClick.AddListener(CloseBTN);
    }

    void CloseBTN()
    {
        Managers.UI_Manager.ClosePopupUI();
    }

    public void OnValueChange()
    {
        // 슬라이더 값에 따라 볼륨을 조절합니다.
        float volume = volumeSlider.value;
        AudioListener.volume = volume;
    }
}
