using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DataManager;
using static Define;
using static REventManager;

public class UI_RandomEvent : UI_Popup
{
    public static UI_RandomEvent instance;
    EventData _eventData;
    enum Buttons
    {
        ResultBTN,
    }
    enum Texts
    {
        EventText,
    }

    enum Images
    {
        CutScene,
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        _eventData = Managers.RandEvent.GetProperEvent();
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<TMPro.TMP_Text>(typeof(Texts));

        Debug.Log(_eventData.EventName);

        GetText((int)Texts.EventText).text = _eventData.EventInfoString;
        GetButton((int)Buttons.ResultBTN).onClick.AddListener(Close);
    }

    void Close()
    {
        Managers.Data._myPlayerData.NowWeek++;
        ProcessData();
        UI_MainBackUI.instance.UpdateUItexts();
        Managers.Data.SaveData();
        Managers.UI_Manager.ClosePopupUI();
    }

    void ProcessData()
    {
        //돈 변화
        Managers.Data._myPlayerData.nowGoldAmount += _eventData.Change[0];

        //구독자 변화
        Managers.Data._myPlayerData.nowSubCount = Mathf.CeilToInt((0.01f)*(float)(_eventData.Change[1]+100)*Managers.Data._myPlayerData.nowSubCount);

        //하트 별 변화량
        Managers.Data._myPlayerData.NowHeart += _eventData.Change[2];
        Managers.Data._myPlayerData.NowStar += _eventData.Change[3];

        //스텟 변화량
        Managers.Data._myPlayerData.SixStat[0] += _eventData.Change[4];
        Managers.Data._myPlayerData.SixStat[1] += _eventData.Change[5];
        Managers.Data._myPlayerData.SixStat[2] += _eventData.Change[6];
        Managers.Data._myPlayerData.SixStat[3] += _eventData.Change[7];
        Managers.Data._myPlayerData.SixStat[4] += _eventData.Change[8];
        Managers.Data._myPlayerData.SixStat[5] += _eventData.Change[9];
    }
   
}
