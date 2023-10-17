using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_MainBackUI : UI_Scene
{
    enum Texts
    {
        HealthTMP,  //현재 건강 상태
        MentalTMP,  //현재 정신 상태
        MyMoneyTMP, //현재 보유 골드
        MySubsTMP,  //현재 보유 구독자수
        NowWeekTMP,
        GameStatTMP,
        SongStatTMP,
        ChatStatTMP,
        StrStatTMP,
        MentalStatTMP,
        LuckStatTMP
    }

    enum Buttons
    {
        CreateScheduleBTN,
        GameStatBTN,
        SongStatBTN,
        ChatStatBTN,
        StrStatBTN, MentalStatBTN, LuckStatBTN
    }

    public static UI_MainBackUI instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Init();
    }

    public StatName NowSelectStatProperty;
    private void Init()
    {
        base.Init();
        Bind<TMPro.TMP_Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        Button CreateScheduleBTN = Get<Button>((int)Buttons.CreateScheduleBTN);

        CreateScheduleBTN.onClick.AddListener(ShowOrCloseCreateSchedulePopup);
        GetButton((int)Buttons.GameStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Game));
        GetButton((int)Buttons.SongStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Song));
        GetButton((int)Buttons.ChatStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Chat));
        GetButton((int)Buttons.StrStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Health));
        GetButton((int)Buttons.MentalStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Mental));
        GetButton((int)Buttons.LuckStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Luck));


        UpdateUItexts();
    }

    void ShowStatProperty(StatName statName)
    {
        var Go = Managers.UI_Manager.ShowPopupUI<UI_StatProperty>();
        NowSelectStatProperty = statName;
    }

    /// <summary>
    /// 메인화면 텍스트들 갱신
    /// </summary>
    public void UpdateUItexts()
    {
        foreach (Texts textType in System.Enum.GetValues(typeof(Texts)))
        {
            TMPro.TMP_Text tmpText = Get<TMPro.TMP_Text>((int)textType);
            tmpText.text = GetInitialTextForType(textType);
        }

        Get<TMPro.TMP_Text>((int)Texts.GameStatTMP).text =   "게임 : " + Managers.Data._myPlayerData.SixStat[0];
        Get<TMPro.TMP_Text>((int)Texts.SongStatTMP).text =   "노래 : " + Managers.Data._myPlayerData.SixStat[1];
        Get<TMPro.TMP_Text>((int)Texts.ChatStatTMP).text =   "저챗 : " + Managers.Data._myPlayerData.SixStat[2];
        Get<TMPro.TMP_Text>((int)Texts.StrStatTMP).text =    "근력 : " + Managers.Data._myPlayerData.SixStat[3];
        Get<TMPro.TMP_Text>((int)Texts.MentalStatTMP).text = "멘탈 : " + Managers.Data._myPlayerData.SixStat[4];
        Get<TMPro.TMP_Text>((int)Texts.LuckStatTMP).text =   "행운 : " + Managers.Data._myPlayerData.SixStat[5];
    }

    private string GetInitialTextForType(Texts textType)
    {
        switch (textType)
        {
            case Texts.HealthTMP:
                return GetNowConditionToString(Managers.Data._myPlayerData.NowHeart);
            case Texts.MentalTMP:
                return GetNowConditionToString(Managers.Data._myPlayerData.NowStar);
            case Texts.MyMoneyTMP:
                return Managers.Data._myPlayerData.nowGoldAmount.ToString();
            case Texts.MySubsTMP:
                return Managers.Data._myPlayerData.nowSubCount.ToString();
            case Texts.NowWeekTMP:
                return "방송 " +Managers.Data._myPlayerData.NowWeek.ToString()+"주차";
            default:
                return "";
        }
    }

    string GetNowConditionToString(int n)
    {
        string temp = "";
        if (n >= 75)
        {
            temp = "건강";
        }
        else if (n >= 50)
        {
            temp = "주의";
        }
        else if (n >= 25)
        {
            temp = "위험";
        }
        else temp = "심각";

        return temp;
    }

    bool isPopupOpen = false;
    public void ShowOrCloseCreateSchedulePopup()
    {
        TMP_Text CreateScheduleTMP = Get<Button>((int)Buttons.CreateScheduleBTN).GetComponentInChildren<TMP_Text>();
        if (isPopupOpen)
        {
            Managers.UI_Manager.ClosePopupUI();
            CreateScheduleTMP.text = "스케쥴 작성하기";
        }
        else
        {
            Managers.UI_Manager.ShowPopupUI<UI_SchedulePopup>();
            CreateScheduleTMP.text = "방으로 돌아가기";
        }        
        isPopupOpen = !isPopupOpen;
    }
}
