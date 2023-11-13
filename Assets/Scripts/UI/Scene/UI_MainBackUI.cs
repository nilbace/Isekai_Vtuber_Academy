using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using DG.Tweening;

public class UI_MainBackUI : UI_Scene
{
    [SerializeField] float AniSpeed;
    enum Texts
    {
        HeartTMP,  //현재 건강 상태
        StarTMP,  //현재 정신 상태
        MyMoneyTMP, //현재 보유 골드
        MySubsTMP,  //현재 보유 구독자수
        NowWeekTMP,
        TempGameTMP,
        TempSongTMP,
        TempDrawTMP,
        TempStrTMP,
        TempMenTMP,
        TempLuckTMP,
    }

    enum Buttons
    {
        CreateScheduleBTN,
        GameStatBTN,
        SongStatBTN,
        DrawStatBTN,
        StrStatBTN, 
        MentalStatBTN,
        LuckStatBTN,
        SettingBTN,
        PlayerSB_BTN,
        StartScheduleBTN, BackBTN
    }

    enum GameObjects
    {
        HeartBar, StarBar, HeartCover, StarCover,
        GameStat_Cover, SongStat_Cover, DrawStat_Cover, StrStat_Cover, MenStat_Cover, LuckStat_Cover,
        Stats, Days7, CallenderBottom, BroadCastTitle
    }

    enum Images
    {
        HeartBar, StarBar
    }


    Animator[] IconBaseAnis = new Animator[6];
    Image[] DayResultSeals = new Image[7];

    public static UI_MainBackUI instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init();
    }

    public StatName NowSelectStatProperty;
    public void Init()
    {
        base.Init();
        Bind<TMPro.TMP_Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        Button CreateScheduleBTN = Get<Button>((int)Buttons.CreateScheduleBTN);

        CreateScheduleBTN.onClick.AddListener(ShowSchedulePopup);
        GetButton((int)Buttons.GameStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Game));
        GetButton((int)Buttons.SongStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Song));
        GetButton((int)Buttons.DrawStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Draw));
        GetButton((int)Buttons.StrStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Strength));
        GetButton((int)Buttons.MentalStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Mental));
        GetButton((int)Buttons.LuckStatBTN).onClick.AddListener(() => ShowStatProperty(StatName.Luck));
        GetButton((int)Buttons.StartScheduleBTN).onClick.AddListener(StartScheduleBTN);
        GetButton((int)Buttons.BackBTN).onClick.AddListener(BackBTN);
        GetButton((int)Buttons.StartScheduleBTN).gameObject.SetActive(false);
        GetButton((int)Buttons.BackBTN).gameObject.SetActive(false);

        for (int i = 0;i<6;i++)
        {
            IconBaseAnis[i] = GetGameObject((int)GameObjects.Stats).transform.GetChild(i).GetChild(0).GetComponent<Animator>();
            IconBaseAnis[i].speed = AniSpeed;
        }
        for (int i = 0; i < 7; i++)
        {
            DayResultSeals[i] = GetGameObject((int)GameObjects.Days7).transform.GetChild(i).GetChild(1).GetComponent<Image>();
        }
        GetButton((int)Buttons.SettingBTN).onClick.AddListener(SettingBTN);

        //방송 타이틀 오른쪽으로 뺴고 시작
        GetGameObject((int)GameObjects.BroadCastTitle).transform.localPosition += new Vector3(XOffset,0,0);


        UpdateUItexts();
        Managers.Sound.Play("bgm1", Sound.Bgm);
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

        float nowHeart = Managers.Data._myPlayerData.NowHeart;
        float nowStar = Managers.Data._myPlayerData.NowStar;

        GetImage((int)Images.HeartBar).sprite =
            Managers.MSM.StatusBar[GetStatusBarImageIndex(nowHeart)];
        GetImage((int)Images.StarBar).sprite =
            Managers.MSM.StatusBar[GetStatusBarImageIndex(nowStar)];

        GetGameObject((int)GameObjects.HeartCover).transform.localScale =
            new Vector3( 1 - (float)Managers.Data._myPlayerData.NowHeart/100f, 1, 1);
        GetGameObject((int)GameObjects.StarCover).transform.localScale =
            new Vector3( 1 - (float)Managers.Data._myPlayerData.NowStar / 100f, 1, 1);

        GetText((int)Texts.HeartTMP).color =
            HeartStarTextColors[GetStatusBarImageIndex(nowHeart)];
        GetText((int)Texts.StarTMP).color =
            HeartStarTextColors[GetStatusBarImageIndex(nowStar)];

        for (int i = 0; i<6;i++)
        {
            GetGameObject((int)GameObjects.GameStat_Cover+i).transform.localScale =
           new Vector3(1 - (float)Managers.Data._myPlayerData.SixStat[i] / 200f, 1, 1);
            GetText((int)Texts.TempGameTMP+i).text = Managers.Data._myPlayerData.SixStat[i].ToString();
        }
    }

    private string GetInitialTextForType(Texts textType)
    {
        switch (textType)
        {
            case Texts.HeartTMP:
                return GetNowConditionToString(Managers.Data._myPlayerData.NowHeart);
            case Texts.StarTMP:
                return GetNowConditionToString(Managers.Data._myPlayerData.NowStar);
            case Texts.MyMoneyTMP:
                return Util.FormatMoney(Managers.Data._myPlayerData.nowGoldAmount);
            case Texts.MySubsTMP:
                return Util.FormatMoney(Managers.Data._myPlayerData.nowSubCount);
            case Texts.NowWeekTMP:
                return "방송 " +Managers.Data._myPlayerData.NowWeek.ToString()+"주차";
            default:
                return "";
        }
    }

    [Header("건강 상태 색")]
    [SerializeField] Color[] HeartStarTextColors;
    string GetNowConditionToString(float n)
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

    int GetStatusBarImageIndex(float n)
    {
        int temp = -1;
        if (n >= 75)
        {
            temp = 0;
        }
        else if (n >= 50)
        {
            temp = 1;
        }
        else if (n >= 25)
        {
            temp = 2;
        }
        else temp = 3;
        return temp;
    }


    
    float moveDuration = 0.52f;
    float XOffset = 350;
    [Header("닷트윈 애니메이션")]
    [SerializeField] Ease ease;
    /// <summary>
    /// 스케쥴 진행 시작시 호출되어 UI들 바꿔줌
    /// </summary>
    public void StartScheduleAndSetUI()
    {
        StartCoroutine(StartScheduleAndSetUICor());
    }

    IEnumerator StartScheduleAndSetUICor()
    {
        CleanSealsOnCallenderBottom();

        Transform BroadCastTitle_tr = GetGameObject((int)GameObjects.BroadCastTitle).transform;
        Transform callenderB_tr = GetGameObject((int)GameObjects.CallenderBottom).transform;
        Transform PlayerSB_BTN_tr = GetButton((int)Buttons.PlayerSB_BTN).transform;
        Transform CreateScheduleBTN_tr = GetButton((int)Buttons.CreateScheduleBTN).transform;
        Transform StartScheduleBTN_TR = GetButton((int)Buttons.StartScheduleBTN).transform;

        callenderB_tr.DOMoveY(callenderB_tr.localPosition.y + 55, moveDuration).SetEase(ease);
        PlayerSB_BTN_tr.DOMoveY(PlayerSB_BTN_tr.localPosition.y - 55, moveDuration).SetEase(ease);
        CreateScheduleBTN_tr.DOMoveX(CreateScheduleBTN_tr.localPosition.x - XOffset, moveDuration).SetEase(ease);
        StartScheduleBTN_TR.DOMoveX(StartScheduleBTN_TR.localPosition.x - XOffset, moveDuration).SetEase(ease);
        var tween = BroadCastTitle_tr.DOMoveX(BroadCastTitle_tr.localPosition.x - XOffset, moveDuration).SetEase(ease);

        yield return tween;
    }

    /// <summary>
    /// 스케쥴 종료시 UI들 다시 전환
    /// </summary>
    public void EndScheduleAndSetUI()
    {
        Debug.Log($"{Managers.Data._myPlayerData.NowWeek -1 } / 구독자_{Managers.Data._myPlayerData.nowSubCount} / 게임 스텟_{Managers.Data._myPlayerData.SixStat[0]}");
        StartCoroutine(EndScheduleAndSetUICor());
    }

    IEnumerator EndScheduleAndSetUICor()
    {
        GetButton((int)Buttons.CreateScheduleBTN).gameObject.SetActive(true);
        GetButton((int)Buttons.StartScheduleBTN).gameObject.SetActive(false);

        Transform BroadCastTitle_tr = GetGameObject((int)GameObjects.BroadCastTitle).transform;
        Transform callenderB_tr = GetGameObject((int)GameObjects.CallenderBottom).transform;
        Transform PlayerSB_BTN_tr = GetButton((int)Buttons.PlayerSB_BTN).transform;
        Transform CreateScheduleBTN_tr = GetButton((int)Buttons.CreateScheduleBTN).transform;
        Transform StartScheduleBTN_TR = GetButton((int)Buttons.StartScheduleBTN).transform;

        callenderB_tr.DOMoveY(callenderB_tr.localPosition.y - 55, moveDuration).SetEase(ease);
        PlayerSB_BTN_tr.DOMoveY(PlayerSB_BTN_tr.localPosition.y + 55, moveDuration).SetEase(ease);
        CreateScheduleBTN_tr.DOMoveX(CreateScheduleBTN_tr.localPosition.x + XOffset, moveDuration).SetEase(ease);
        StartScheduleBTN_TR.DOMoveX(StartScheduleBTN_TR.localPosition.x + XOffset, moveDuration).SetEase(ease);
        var tween = BroadCastTitle_tr.DOMoveX(BroadCastTitle_tr.localPosition.x + XOffset, moveDuration).SetEase(ease);

        yield return tween;
    }

    public void GlitterStat(int i)
    {
        IconBaseAnis[i].CrossFade("Shine", 0);
    }

    public void CleanSealsOnCallenderBottom()
    {
        for(int i = 0; i<7;i++)
        {
            DayResultSeals[i].sprite = null;
            DayResultSeals[i].color = new Color(0, 0, 0, 0);
        }
    }

    public void StampSeal(int day, int SealType)
    {
        DayResultSeals[day].color = new Color(1, 1, 1, 1);

        if (SealType == 0)
            DayResultSeals[day].GetComponent<Animator>().SetTrigger("StarAni");
        else if(SealType == 1)
            DayResultSeals[day].GetComponent<Animator>().SetTrigger("OAni");
        else
        {
            Debug.Log("XXXXX");
            DayResultSeals[day].GetComponent<Animator>().SetTrigger("XAni");
        }
    }

    public void ShowSchedulePopup()
    {
        Managers.UI_Manager.ShowPopupUI<UI_SchedulePopup>();
        GetButton((int)Buttons.StartScheduleBTN).gameObject.SetActive(true);
        GetButton((int)Buttons.BackBTN).gameObject.SetActive(true);
        Get<Button>((int)Buttons.CreateScheduleBTN).gameObject.SetActive(false);
    }

    void StartScheduleBTN()
    {
        StartScheduleAndSetUI();
        Managers.instance.StartSchedule();
        Managers.UI_Manager.ClosePopupUI();
    }
    void BackBTN()
    {
        if (UI_SchedulePopup.instance.SubContentSelectPhase)
        {
            UI_SchedulePopup.instance.Show3Contents();
        }
        else
        {
            Get<Button>((int)Buttons.CreateScheduleBTN).gameObject.SetActive(true);
            GetButton((int)Buttons.StartScheduleBTN).gameObject.SetActive(false);
            GetButton((int)Buttons.BackBTN).gameObject.SetActive(false);
            Managers.UI_Manager.ClosePopupUI();

        }
    }

    public Button GetStartScheduleBTN()
    {
        return GetButton((int)Buttons.StartScheduleBTN);
    }

    public Button GetBackBTN()
    {
        return GetButton((int)Buttons.BackBTN);
    }

    public void SettingBTN()
    {
        Managers.UI_Manager.ShowPopupUI<UI_Setting>();
    }
}
