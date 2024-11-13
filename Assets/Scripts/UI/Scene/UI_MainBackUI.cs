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

    //��Ʈ, ��
    public Sprite[] StatusBar;
    [Header("�ǰ� ���� ��")]
    [SerializeField] Color[] HeartStarTextColors;

    //��� ���� ������ �ִϸ�����
    Animator[] IconBaseAnis = new Animator[6];

    //�ϴ� 7�� �̹���
    public Image[] Under7Imges;
    Image[] DayResultSeals = new Image[7];

    //���� ȭ�� �ִϸ��̼�
    public const float ScreenAniSpeed = 0.05555556f;
    public bool IsFastMode = false;
    public Sprite[] SpeedBTNSprite;

    enum Texts
    {
        HeartTMP,  //���� �ǰ� ����
        StarTMP,  //���� ���� ����
        MyMoneyTMP, //���� ���� ���
        MySubsTMP,  //���� ���� �����ڼ�
        NowWeekTMP,
        TempGameTMP,
        TempSongTMP,
        TempDrawTMP,
        TempStrTMP,
        TempMenTMP,
        TempLuckTMP,
        CommunicationTMP,
        BCTitleTMP,
        NickNameTMP
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
        CommuiBTN,
        StartScheduleBTN, BackBTN,
        SpeedBTN,
        ArchiveBTN,
        RubiaNickNameBTN
    }

    enum GameObjects
    {
        HeartBar, StarBar, HeartCover, StarCover,
        GameStat_Cover, SongStat_Cover, DrawStat_Cover, StrStat_Cover, MenStat_Cover, LuckStat_Cover,
        Stats, Days7, CallenderBottom, BroadCastTitle
    }

    enum Images
    {
        HeartBar, StarBar, Reddot, Reddot2
    }
    enum Animators
    {
        ScreenIMG, RubiaIMG
    }


    public static UI_MainBackUI instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        base.Init();
        Bind<TMPro.TMP_Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<Animator>(typeof(Animators));

        //����
        GetButton((int)Buttons.CreateScheduleBTN).onClick.AddListener(ShowSchedulePopup);
        GetButton((int)Buttons.GameStatBTN).onClick.AddListener(() => ShowStatPropertyUI(StatName.Game));
        GetButton((int)Buttons.SongStatBTN).onClick.AddListener(() => ShowStatPropertyUI(StatName.Song));
        GetButton((int)Buttons.DrawStatBTN).onClick.AddListener(() => ShowStatPropertyUI(StatName.Draw));
        GetButton((int)Buttons.StrStatBTN).onClick.AddListener(() => ShowStatPropertyUI(StatName.Strength));
        GetButton((int)Buttons.MentalStatBTN).onClick.AddListener(() => ShowStatPropertyUI(StatName.Mental));
        GetButton((int)Buttons.LuckStatBTN).onClick.AddListener(() => ShowStatPropertyUI(StatName.Luck));
        GetButton((int)Buttons.StartScheduleBTN).onClick.AddListener(StartScheduleBTN);
        GetButton((int)Buttons.BackBTN).onClick.AddListener(BackBTN);
        GetButton((int)Buttons.ArchiveBTN).onClick.AddListener(ArchiveBTN);
        GetButton((int)Buttons.CommuiBTN).onClick.AddListener(CommunicationBTN);
        GetButton((int)Buttons.StartScheduleBTN).gameObject.SetActive(false);
        GetButton((int)Buttons.BackBTN).gameObject.SetActive(false); 
        GetButton((int)Buttons.SpeedBTN).onClick.AddListener(SpeedBTN);
        GetButton((int)Buttons.RubiaNickNameBTN).onClick.AddListener(NickNameBTN);
        GetButton((int)Buttons.SettingBTN).onClick.AddListener(SettingBTN);
        SpeedBTNInit();

        //���� ������ 6�� ����
        for (int i = 0; i < 6; i++)
        {
            IconBaseAnis[i] = GetGameObject((int)GameObjects.Stats).transform.GetChild(i).GetChild(0).GetComponent<Animator>();
            IconBaseAnis[i].speed = AniSpeed;
        }

        //�ϴ� ���� �� 7�� ����
        for (int i = 0; i < 7; i++)
        {
            DayResultSeals[i] = GetGameObject((int)GameObjects.Days7).transform.GetChild(i).GetChild(1).GetComponent<Image>();
        }

   

        //��� Ÿ��Ʋ ���������� ���� ����
        GetGameObject((int)GameObjects.BroadCastTitle).transform.localPosition += new Vector3(XOffset, 0, 0);


        //���꽺�丮 ����
        if (Managers.Data.PlayerData.NowWeek != Managers.Data.PlayerData.SubStoryIndex.Count)
        {
            //���� 1������ �߻���
            SetSubStoryIndex();
        }
        else
        {
            //�ٸ� ������� ����� ���꽺�丮 �ε��� ����
            NowWeekSubStoryIndex = Managers.Data.PlayerData.SubStoryIndex[Managers.Data.PlayerData.NowWeek - 1];
        }


        Get<Animator>((int)Animators.ScreenIMG).speed = ScreenAniSpeed;
        UpdateReddot();
        UpdateUItextsAndCheckNickname();
        RegisterActionToOtherScripts();
    }

    void RegisterActionToOtherScripts()
    {
        ScheduleExecuter.Inst.WeekOverAction -= SetSubStoryIndex;
        ScheduleExecuter.Inst.WeekOverAction += SetSubStoryIndex;
        ScheduleExecuter.Inst.WeekOverAction -= FinishWeek;
        ScheduleExecuter.Inst.WeekOverAction += FinishWeek;
        ScheduleExecuter.Inst.SetAniSpeedAction -= SetScreenAniSpeed;
        ScheduleExecuter.Inst.SetAniSpeedAction += SetScreenAniSpeed;
    }

    //����UIȭ�鿡�� ������ ��� ��ư��
    #region Buttons
    //��� Īȣ ��ư
    void NickNameBTN()
    {
        Managers.Sound.Play("SmallBTN", Sound.Effect);
        Managers.UI_Manager.ShowPopupUI<UI_NickName>();
    }
    //��� ��ī�̺� ��ư
    void ArchiveBTN()
    {
        Managers.Sound.Play(Define.Sound.SmallBTN);
        Managers.UI_Manager.ShowPopupUI<UI_Archive>();
    }
    //��� ���� ��ư
    public void SettingBTN()
    {
        Managers.Sound.Play("SmallBTN", Sound.Effect);
        Managers.UI_Manager.ShowPopupUI<UI_Setting>();
    }

    //�ߴ� ���� �˾�â
    void ShowStatPropertyUI(StatName statName)
    {
        Managers.Sound.Play("SmallBTN", Sound.Effect);
        StartCoroutine(ShowStatProperty(statName));
    }

    IEnumerator ShowStatProperty(StatName statName)
    {
        if (UI_StatProperty.instance == null)
        {
            var Go = Managers.UI_Manager.ShowPopupUI<UI_StatProperty>();
            yield return new WaitForEndOfFrame();
            Go.Setting(statName);
        }
        else
        {
            UI_StatProperty.instance.Setting(statName);
        }
    }

    public void UpdateReddot()
    {
        if (Managers.Data.PersistentUser.WatchedScehdule.ContainsValue(false) || Managers.Data.PersistentUser.WatchedRandEvent.ContainsValue(false) || Managers.Data.PersistentUser.WatchedEndingName.ContainsValue(false))
        {
            GetImage((int)Images.Reddot).gameObject.SetActive(true);
        }
        else
        {
            GetImage((int)Images.Reddot).gameObject.SetActive(false);
        }
        GetImage((int)Images.Reddot2).gameObject.SetActive(PlayerPrefs.GetInt("UnderR_D_State", 0) == 1);
    }

    public void ChangeUnderRedDotState(bool newState)
    {
        PlayerPrefs.SetInt("UnderR_D_State", newState ? 1 : 0);
        PlayerPrefs.Save();

        UpdateReddot();
    }

    //�ϴ� ��� ��ư
    //ȭ�� �ִϸ��̼� �ӵ� ����
    void SpeedBTNInit()
    {
        if (PlayerPrefs.HasKey("IsFastModeKey"))
        {
            IsFastMode = PlayerPrefs.GetInt("IsFastModeKey") == 1;
        }

        if (!IsFastMode)
        {
            GetButton((int)Buttons.SpeedBTN).GetComponent<Image>().sprite = SpeedBTNSprite[0];
        }
        else
        {
            GetButton((int)Buttons.SpeedBTN).GetComponent<Image>().sprite = SpeedBTNSprite[1];
        }
    }

    void SpeedBTN()
    {
        IsFastMode = !IsFastMode;
        SaveIsFastMode();
        if (!IsFastMode)
        {
            GetButton((int)Buttons.SpeedBTN).GetComponent<Image>().sprite = SpeedBTNSprite[0];
        }
        else
        {
            GetButton((int)Buttons.SpeedBTN).GetComponent<Image>().sprite = SpeedBTNSprite[1];
        }
    }
    void SaveIsFastMode()
    {
        PlayerPrefs.SetInt("IsFastModeKey", IsFastMode ? 1 : 0);
        PlayerPrefs.Save();
    }
    //�ϴ� ������ ¥�� ��ư
    //��� �˾�â�� ������ �ý���
    public void ShowSchedulePopup()
    {
        Managers.Sound.Play(Sound.BigBTN);
        Managers.UI_Manager.ShowPopupUI<UI_SchedulePopup>();
        GetButton((int)Buttons.StartScheduleBTN).gameObject.SetActive(true);
        GetButton((int)Buttons.BackBTN).gameObject.SetActive(true);
        Get<Button>((int)Buttons.CreateScheduleBTN).gameObject.SetActive(false);
    }

    //������ �����ϱ� ��ư
    void StartScheduleBTN()
    {
        Managers.Sound.Play(Sound.ScheduleBTN);
        StartScheduleAndSetUI();
        Managers.instance.StartSchedule();
        Managers.UI_Manager.ClosePopupUI();
    }


    //�ϴ� �ڷΰ��� ��ư
    public void BackBTN()
    {
        Managers.Sound.Play("SmallBTN", Sound.Effect);
        BackBTNWithoutSound();
    }
    public void BackBTNWithoutSound()
    {
        if (UI_SchedulePopup.instance.IsShowing3ContentsUI())
        {
            Get<Button>((int)Buttons.CreateScheduleBTN).gameObject.SetActive(true);
            GetButton((int)Buttons.StartScheduleBTN).gameObject.SetActive(false);
            GetButton((int)Buttons.BackBTN).gameObject.SetActive(false);
            Managers.UI_Manager.ClosePopupUI();
        }
        else
        {
            UI_SchedulePopup.instance.Show3Contents();
        }
    }


    //���ϴ� ���꽺�丮 ��ư
    int NowWeekSubStoryIndex;
    public void SetSubStoryIndex()
    {
        int temp;
        while (true)
        {
            temp = Random.Range(0, (int)SubStoryName.Max);
            if (!Managers.Data.PlayerData.SubStoryIndex.Contains(temp)) break;
        }
        Managers.Data.PlayerData.SubStoryIndex.Add(temp);
        NowWeekSubStoryIndex = temp;
        UpdateUItextsAndCheckNickname();
        Managers.Data.SaveData();
    }

    void CommunicationBTN()
    {
        ChangeUnderRedDotState(false);
        StartCoroutine(ShowSubStoryCor(NowWeekSubStoryIndex));
    }

    IEnumerator ShowSubStoryCor(int index)
    {
        Managers.UI_Manager.ShowPopupUI<UI_Communication>();
        Managers.Sound.Play(Sound.SmallBTN);
        yield return new WaitForEndOfFrame();
        SubStoryParser.Inst.StartStory(index);
    }
    #endregion

    //������ ���� �ִϸ��̼�
    #region ScheduleAnimation

    public void StartScreenAnimation(string KorName, string RubiaAniIndex = "")
    {
        //Debug.Log($"{KorName} ����");
        var ScreenAnimator = Get<Animator>((int)Animators.ScreenIMG);
        var RubiaAnimator = Get<Animator>((int)Animators.RubiaIMG);

        ScreenAnimator.StopPlayback();
        ScreenAnimator.SetTrigger(KorName);
        

        if(RubiaAniIndex != "")
        {
            RubiaAnimator.gameObject.SetActive(true);
            RubiaAnimator.SetTrigger(RubiaAniIndex);
        }
        else
        {
            RubiaAnimator.gameObject.SetActive(false);
        }
    }

    public void SetScreenAniSpeed(int speed)
    {
        var ScreenAnimator = Get<Animator>((int)Animators.ScreenIMG);
        var RubiaAnimator = Get<Animator>((int)Animators.RubiaIMG);

        ScreenAnimator.speed = speed * ScreenAniSpeed;
        RubiaAnimator.speed = speed * ScreenAniSpeed;
    }

    public void StopScreenAni()
    {
        var ScreenAnimator = Get<Animator>((int)Animators.ScreenIMG);
        ScreenAnimator.StopPlayback();
    }


    #endregion

    //UI���� ���� ��ũ��Ʈ��
    #region UI_updates
    //������ ����Ҷ� �۵��ϴ� �ִϸ��̼�
    public void GlitterStat(int i)
    {
        IconBaseAnis[i].CrossFade("Shine", 0);
    }

    //��ü������ ��� ���ڵ� ����+�г��� üũ
    public void UpdateUItextsAndCheckNickname()
    {
        foreach (Texts textType in System.Enum.GetValues(typeof(Texts)))
        {
            TMPro.TMP_Text tmpText = Get<TMPro.TMP_Text>((int)textType);
            tmpText.text = GetInitialTextForType(textType);
        }

        float nowHeart = Managers.Data.PlayerData.NowHeart;
        float nowStar = Managers.Data.PlayerData.NowStar;

        GetImage((int)Images.HeartBar).sprite =
            StatusBar[GetStatusBarImageIndex(nowHeart)];
        GetImage((int)Images.StarBar).sprite =
            StatusBar[GetStatusBarImageIndex(nowStar)];

        GetGameObject((int)GameObjects.HeartCover).transform.localScale =
            new Vector3(1 - (float)Managers.Data.PlayerData.NowHeart / 100f, 1, 1);
        GetGameObject((int)GameObjects.StarCover).transform.localScale =
            new Vector3(1 - (float)Managers.Data.PlayerData.NowStar / 100f, 1, 1);

        GetText((int)Texts.HeartTMP).color =
            HeartStarTextColors[GetStatusBarImageIndex(nowHeart)];
        GetText((int)Texts.StarTMP).color =
            HeartStarTextColors[GetStatusBarImageIndex(nowStar)];

        for (int i = 0; i < 6; i++)
        {
            GetGameObject((int)GameObjects.GameStat_Cover + i).transform.localScale =
           new Vector3(1 - (float)Managers.Data.PlayerData.SixStat[i] / 200f, 1, 1);
            GetText((int)Texts.TempGameTMP + i).text = Managers.Data.PlayerData.SixStat[i].ToString("F0");
        }

        GetText((int)Texts.CommunicationTMP).text = ReplaceDashWithSpace(((SubStoryName)NowWeekSubStoryIndex).ToString());
        GetText((int)Texts.NickNameTMP).text = Managers.Data.PlayerData.NowNickName;

        //�� ���� ���� ���� �˻� �κ�
        if (Managers.Data.PlayerData.SixStat[5] >= 200) Managers.NickName.OpenNickname(NickNameKor.�����);
        if (Managers.Data.PlayerData.SixStat[3] >= 200) Managers.NickName.OpenNickname(NickNameKor.�ܴ���);
        if (Managers.Data.PlayerData.SixStat[4] >= 200) Managers.NickName.OpenNickname(NickNameKor.�����);
        if (Managers.Data.PlayerData.nowGoldAmount >= 100000) Managers.NickName.OpenNickname(NickNameKor.�θ��־�);
        if (Managers.Data.PlayerData.nowSubCount >= 100000) Managers.NickName.OpenNickname(NickNameKor.��������);
        if (Managers.Data.PlayerData.nowSubCount >= 1000000) Managers.NickName.OpenNickname(NickNameKor.��Ʃ��);
        UpdateReddot();
    }

    private string GetInitialTextForType(Texts textType)
    {
        switch (textType)
        {
            case Texts.HeartTMP:
                return GetNowConditionToString(Managers.Data.PlayerData.NowHeart);
            case Texts.StarTMP:
                return GetNowConditionToString(Managers.Data.PlayerData.NowStar);
            case Texts.MyMoneyTMP:
                return Util.FormatMoney(Managers.Data.PlayerData.nowGoldAmount);
            case Texts.MySubsTMP:
                return Util.FormatSubs(Managers.Data.PlayerData.nowSubCount);
            case Texts.NowWeekTMP:
                return "��� " + Managers.Data.PlayerData.NowWeek.ToString() + "����";
            default:
                return "";
        }
    }

    
    string GetNowConditionToString(float n)
    {
        string temp = "";
        if (n >= 75)
        {
            temp = $"�ǰ� {n}";
        }
        else if (n >= 50)
        {
            temp = $"���� {n}";
        }
        else if (n >= 25)
        {
            temp = $"���� {n}";
        }
        else temp = $"�ɰ� {n}";
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

    string ReplaceDashWithSpace(string input)
    {
        string result = input.Replace('_', ' ');
        return result;
    }

    //������ ¥�� ��ư,�ڷΰ��� ��ư �� ������ �� ��ư�� �¿� �̵�
    float moveDuration = 0.52f;
    float XOffset = 350;
    [Header("��Ʈ�� �ִϸ��̼�")]
    [SerializeField] Ease ease;

    public void StartScheduleAndSetUI()
    {
        StartCoroutine(StartScheduleAndSetUICor());
    }

    IEnumerator StartScheduleAndSetUICor()
    {
        CleanSealsOnCallenderBottom();

        Transform BroadCastTitle_tr = GetGameObject((int)GameObjects.BroadCastTitle).transform;
        Transform callenderB_tr = GetGameObject((int)GameObjects.CallenderBottom).transform;
        Transform PlayerSB_BTN_tr = GetButton((int)Buttons.CommuiBTN).transform;
        Transform CreateScheduleBTN_tr = GetButton((int)Buttons.CreateScheduleBTN).transform;
        Transform StartScheduleBTN_TR = GetButton((int)Buttons.StartScheduleBTN).transform;

        callenderB_tr.DOMoveY(callenderB_tr.localPosition.y + 55, moveDuration).SetEase(ease);
        PlayerSB_BTN_tr.DOMoveY(PlayerSB_BTN_tr.localPosition.y - 55, moveDuration).SetEase(ease);
        CreateScheduleBTN_tr.DOMoveX(CreateScheduleBTN_tr.localPosition.x - XOffset, moveDuration).SetEase(ease);
        StartScheduleBTN_TR.DOMoveX(StartScheduleBTN_TR.localPosition.x - XOffset, moveDuration).SetEase(ease);
        var tween = BroadCastTitle_tr.DOMoveX(BroadCastTitle_tr.localPosition.x - XOffset, moveDuration).SetEase(ease);

        yield return tween;
    }
    //�ϴ� ��ƼĿ�� ����
    public void CleanSealsOnCallenderBottom()
    {
        for (int i = 0; i < 7; i++)
        {
            DayResultSeals[i].sprite = null;
            DayResultSeals[i].color = new Color(0, 0, 0, 0);
        }
    }

    public void BottomSeal(int day, int SealType)
    {
        DayResultSeals[day].color = new Color(1, 1, 1, 1);

        if (SealType == 0)
            DayResultSeals[day].GetComponent<Animator>().SetTrigger("StarAni");
        else if (SealType == 1)
            DayResultSeals[day].GetComponent<Animator>().SetTrigger("OAni");
        else if (SealType == 2)
        {
            DayResultSeals[day].GetComponent<Animator>().SetTrigger("XAni");
        }
    }

    //���� ������
    void FinishWeek()
    {
        ChangeUnderRedDotState(true);
        EndScheduleAndSetUI();
        UpdateUItextsAndCheckNickname();
    }

    public void EndScheduleAndSetUI()
    {
        StartCoroutine(EndScheduleAndSetUICor());
    }

    IEnumerator EndScheduleAndSetUICor()
    {
        GetButton((int)Buttons.CreateScheduleBTN).gameObject.SetActive(true);
        GetButton((int)Buttons.StartScheduleBTN).gameObject.SetActive(false);

        Transform BroadCastTitle_tr = GetGameObject((int)GameObjects.BroadCastTitle).transform;
        Transform callenderB_tr = GetGameObject((int)GameObjects.CallenderBottom).transform;
        Transform PlayerSB_BTN_tr = GetButton((int)Buttons.CommuiBTN).transform;
        Transform CreateScheduleBTN_tr = GetButton((int)Buttons.CreateScheduleBTN).transform;
        Transform StartScheduleBTN_TR = GetButton((int)Buttons.StartScheduleBTN).transform;

        callenderB_tr.DOMoveY(callenderB_tr.localPosition.y - 55, moveDuration).SetEase(ease);
        PlayerSB_BTN_tr.DOMoveY(PlayerSB_BTN_tr.localPosition.y + 55, moveDuration).SetEase(ease);
        CreateScheduleBTN_tr.DOMoveX(CreateScheduleBTN_tr.localPosition.x + XOffset, moveDuration).SetEase(ease);
        StartScheduleBTN_TR.DOMoveX(StartScheduleBTN_TR.localPosition.x + XOffset, moveDuration).SetEase(ease);
        var tween = BroadCastTitle_tr.DOMoveX(BroadCastTitle_tr.localPosition.x + XOffset, moveDuration).SetEase(ease);

        yield return tween;
    }

    #endregion

    //�ν��Ͻ� ���� ��Ʈ
    #region GetInstance
    public Button GetStartScheduleBTN()
    {
        return GetButton((int)Buttons.StartScheduleBTN);
    }

    public Button GetBackBTN()
    {
        return GetButton((int)Buttons.BackBTN);
    }
    #endregion
}
