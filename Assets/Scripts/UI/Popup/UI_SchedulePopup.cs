using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using DG.Tweening;

public class UI_SchedulePopup : UI_Popup
{
    public static UI_SchedulePopup instance;
    public Transform ParentTR;
    public GameObject UISubContent;
    private ScrollRect scrollRect;

    public Sprite[] DaysSprites;

    [SerializeField] Ease ease;
    [SerializeField] float moveDuration;
    [SerializeField] Color[] baseTextColor;
    Image[] Under7dayIMGs;
    Button StartScheduleBTN_Main;
    Button BackBTN_Main;
    Button[] CategoryBTNs;

    SevenDays _nowSelectedDay = SevenDays.Monday;
    OneDayScheduleData[] _SevenDayScheduleDatas = new OneDayScheduleData[7];
    float[] _SeveDayScrollVarValue = new float[7];

    enum Buttons
    {
        MondayBTN,
        TuesdayBTN,
        WednesdayBTN,
        ThursdayBTN,
        FridayBTN,
        SaturdayBTN,
        SundayBTN,
        BroadcastBTN,
        RestBTN,
        GoOutBTN,
    }
    enum Texts
    {
        PointText,
        ScoreText,
    }
    enum GameObjects
    {
        Days7,
        Contents3,
        SubContents,
        Category,
        Content,
        Sub0, Sub1, Sub2, Sub3, 
    }
    enum Images
    {
        ItemIcon,
        NowDayFrame,
    }

    private void Start()
    {
        Init();
    }
       
    public override void Init()
    {
        //UI초기화
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Under7dayIMGs = UI_MainBackUI.instance.Under7Imges;
        StartScheduleBTN_Main = UI_MainBackUI.instance.GetStartScheduleBTN();
        BackBTN_Main = UI_MainBackUI.instance.GetBackBTN();
        scrollRect = GetComponentInChildren<ScrollRect>();
        CategoryBTNs = GetGameObject((int)GameObjects.Category).GetComponentsInChildren<Button>();
        _SeveDayScrollVarValue = Managers.Data._SeveDayScrollVarValue;
        _SevenDayScheduleDatas = Managers.Data._SevenDayScheduleDatas;
        

        //싱글톤
        if (instance == null)
            instance = this;

        //뮹뮹 상태 변경
        MM.Inst.SetState(MMState.OnSchedule);

        //캔버스 위치 조절
        Canvas canvas = Util.GetOrAddComponent<Canvas>(gameObject);
        canvas.sortingOrder = -3;

        //요일 버튼 연결
        for (int i = 0; i<7; i++)
        {
            int inttemp = i;
            Button temp = GetButton(i);
            temp.onClick.AddListener(() => { ClickDayBTN(inttemp); });
        }

        //버튼 소리 추가
        Button[] allbuttons = GetComponentsInChildren<Button>();
        foreach(Button btn in allbuttons)
        {
            btn.onClick.AddListener(() => Managers.Sound.Play(Sound.SmallBTN));
        }
        
        //방송, 휴식, 외출 버튼 연결
        GetButton((int)Buttons.BroadcastBTN).onClick.AddListener(()=>ChooseContentType(ContentType.BroadCast));
        GetButton((int)Buttons.RestBTN).onClick.AddListener(() => ChooseContentType(ContentType.Rest));
        GetButton((int)Buttons.GoOutBTN).onClick.AddListener(() => ChooseContentType(ContentType.GoOut));

        //마지막 처리들
        GetGameObject((int)GameObjects.SubContents).SetActive(false);
        GetGameObject((int)GameObjects.Category).SetActive(false);
        SetNowSelectedFrameBox();
        UpdateDayBTN_Interactable();
        ClickLastDay_PlusOne();
    }
    
    void ClickDayBTN(int dayindex)
    {
        _nowSelectedDay = (SevenDays)dayindex;
        if (_SevenDayScheduleDatas[(int)_nowSelectedDay] == null)
        {
            ShowThreeContentType();
        }
        else
        {
            if (_SevenDayScheduleDatas[(int)_nowSelectedDay].scheduleType == ContentType.BroadCast)
            {
                ChooseContentType(ContentType.BroadCast);
                GetGameObject((int)GameObjects.Category).SetActive(true);
            }
            else if (_SevenDayScheduleDatas[(int)_nowSelectedDay].scheduleType == ContentType.Rest)
            {
                ChooseContentType(ContentType.Rest);
            }
            else
            {
                ChooseContentType(ContentType.GoOut);
                GetGameObject((int)GameObjects.Category).SetActive(true);
            }
        }
        UpdateColor_Interactable_DaySelectedbtns();
    }
    void ShowThreeContentType()
    {
        GetGameObject((int)GameObjects.Contents3).SetActive(true);
        GetGameObject((int)GameObjects.SubContents).SetActive(false);
        GetGameObject((int)GameObjects.Category).SetActive(false);
    }

    void ShowSubcontent()
    {
        GetGameObject((int)GameObjects.Contents3).SetActive(false);
        GetGameObject((int)GameObjects.SubContents).SetActive(true);
    }
    
    void UpdateDayBTN_Interactable()
    {
        int i = 0;

        for(;i<7;i++)
        {
            StartScheduleBTN_Main.interactable = false;
            StartScheduleBTN_Main.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            if (_SevenDayScheduleDatas[i] == null)
            {
                _nowSelectedDay = (SevenDays)i;
                break;
            }
            if (i == 6)
            {
                StartScheduleBTN_Main.interactable = true;
                StartScheduleBTN_Main.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            }
        }
        if (i == 7) i = 6;
        for(int j = 0; j<7;j++)
        {
            GetButton(j).interactable = (j <= i) ? true : false;
        }
        
    }
    
    void UpdateColor_Interactable_DaySelectedbtns()
    {
        for(int i = 0; i<7;i++)
        {
            if (i == (int)_nowSelectedDay)
            {
                GetImage(1).transform.DOMoveX((i - 3) * 40, moveDuration).SetEase(ease);
            }

            if (_SevenDayScheduleDatas[i] == null)
            {
                GetButton(i).GetComponentInChildren<TMP_Text>().color = baseTextColor[0];
                if (GetButton(i).interactable == true)
                {
                    GetButton(i).GetComponent<Image>().sprite = DaysSprites[3];
                    Under7dayIMGs[i].sprite = DaysSprites[3];
                }
                else
                {
                    GetButton(i).GetComponent<Image>().sprite = DaysSprites[4];
                    Under7dayIMGs[i].sprite = DaysSprites[4];
                }
            }
            else if(_SevenDayScheduleDatas[i].scheduleType == ContentType.BroadCast)
            {
                GetButton(i).GetComponent<Image>().sprite = DaysSprites[0];
                Under7dayIMGs[i].sprite = DaysSprites[0];
                GetButton(i).GetComponentInChildren<TMP_Text>().color = baseTextColor[1];
                Under7dayIMGs[i].GetComponentInChildren<TMP_Text>().color = baseTextColor[1];
            }
            else if(_SevenDayScheduleDatas[i].scheduleType == ContentType.Rest)
            {
                GetButton(i).GetComponent<Image>().sprite = DaysSprites[1];
                Under7dayIMGs[i].sprite = DaysSprites[1];
                GetButton(i).GetComponentInChildren<TMP_Text>().color = baseTextColor[2];
                Under7dayIMGs[i].GetComponentInChildren<TMP_Text>().color = baseTextColor[2];
            }
            else
            {
                GetButton(i).GetComponent<Image>().sprite = DaysSprites[2];
                Under7dayIMGs[i].sprite = DaysSprites[2];
                GetButton(i).GetComponentInChildren<TMP_Text>().color = baseTextColor[3];
                Under7dayIMGs[i].GetComponentInChildren<TMP_Text>().color = baseTextColor[3];
            }
        }
    }

    public void Show3Contents()
    {
        GetGameObject((int)GameObjects.Contents3).SetActive(true);
        GetGameObject((int)GameObjects.SubContents).SetActive(false);
        GetGameObject((int)GameObjects.Category).SetActive(false);
    }

    public Sprite[] CategoryIMGS;
    public float[] BC_ScrollXPoz;
    public float[] GoOutScrollXPoz;
    void SetSubContentSize(ContentType taskType)
    {
        var SubcontentsGO = GetGameObject((int)GameObjects.SubContents);
        var SubcontRect = SubcontentsGO.GetComponent<RectTransform>();
        var LayoutGroup = GetGameObject((int)GameObjects.Content).GetComponent<HorizontalLayoutGroup>();

        switch (taskType)
        {
            case ContentType.BroadCast:
                GetGameObject((int)GameObjects.Category).SetActive(true);
                SubcontRect.sizeDelta = new Vector2(254, SubcontRect.sizeDelta.y);
                SubcontRect.transform.position = new Vector2(18, SubcontRect.transform.position.y);
                LayoutGroup.padding.left = 2;
                for (int i = 0; i < 6; i++)
                {
                    CategoryBTNs[i].onClick.RemoveAllListeners();
                    CategoryBTNs[i].onClick.AddListener(() => Managers.Sound.Play(Sound.SmallBTN));
                    if (i<3)
                    {
                        int j = i;
                        CategoryBTNs[i].onClick.AddListener(() => DOTween.To(() => scrollRect.horizontalScrollbar.value, x => scrollRect.horizontalScrollbar.value = x, BC_ScrollXPoz[j], moveDuration).SetEase(ease));
                        CategoryBTNs[i].GetComponent<Image>().sprite = CategoryIMGS[0];
                        CategoryBTNs[i].spriteState = new SpriteState { pressedSprite = CategoryIMGS[1] };
                    }
                    else
                    {
                        CategoryBTNs[i].gameObject.SetActive(false);
                    }
                }
                break;
            case ContentType.Rest:
                GetGameObject((int)GameObjects.Category).SetActive(false);
                SubcontRect.sizeDelta = new Vector2(282, SubcontRect.sizeDelta.y);
                SubcontRect.transform.position = new Vector2(0, SubcontRect.transform.position.y);
                LayoutGroup.padding.left = 2;
                break;
            case ContentType.GoOut:
                GetGameObject((int)GameObjects.Category).SetActive(true);
                SubcontRect.sizeDelta = new Vector2(254, SubcontRect.sizeDelta.y);
                SubcontRect.transform.position = new Vector2(18, SubcontRect.transform.position.y);
                LayoutGroup.padding.left = 2;
                for (int i = 0; i < 6; i++)
                {
                    CategoryBTNs[i].onClick.RemoveAllListeners();
                    CategoryBTNs[i].onClick.AddListener(() => Managers.Sound.Play(Sound.SmallBTN));
                    int j = i;
                    CategoryBTNs[i].gameObject.SetActive(true);
                    CategoryBTNs[i].onClick.AddListener(() => DOTween.To(() => scrollRect.horizontalScrollbar.value, x => scrollRect.horizontalScrollbar.value = x, GoOutScrollXPoz[j], moveDuration).SetEase(ease));
                    CategoryBTNs[i].GetComponent<Image>().sprite = CategoryIMGS[2];
                    CategoryBTNs[i].spriteState = new SpriteState { pressedSprite = CategoryIMGS[3] };
                }
                break;
        }
    }

    void ChooseContentType(ContentType contentType)
    {
        ShowSubcontent();
        SetSubContentSize(contentType);
        ChooseScheduleTypeAndFillList(contentType);
    }


    List<OneDayScheduleData> nowSelectScheduleTypeList = new List<OneDayScheduleData>();
    void ChooseScheduleTypeAndFillList(ContentType type)
    {
        nowSelectScheduleTypeList.Clear();
        DeleteAllChildren();
        switch (type)
        {
            case ContentType.BroadCast:
                for (int i = 0; i < (int)BroadCastType.MaxCount_Name; i++)
                {
                    nowSelectScheduleTypeList.Add(Managers.Data.GetOneDayDataByName((BroadCastType)i));
                    GameObject go = Instantiate(UISubContent, ParentTR, false);
                    go.GetComponent<UI_SubContent>().SetInfo(Managers.Data.GetOneDayDataByName((BroadCastType)i),_SevenDayScheduleDatas[(int)_nowSelectedDay], _nowSelectedDay, StatName.FALSE);
                }
                break;

            case ContentType.Rest:
                for (int i = 0; i < (int)RestType.MaxCount; i++)
                {
                    nowSelectScheduleTypeList.Add(Managers.Data.GetOneDayDataByName((RestType)i));
                    GameObject go = Instantiate(UISubContent, ParentTR, false);
                    go.GetComponent<UI_SubContent>().SetInfo(Managers.Data.GetOneDayDataByName((RestType)i), _SevenDayScheduleDatas[(int)_nowSelectedDay], _nowSelectedDay, StatName.FALSE);
                }
                break;

            case ContentType.GoOut:
                for (int i = 0; i < (int)GoOutType.MaxCount; i++)
                {
                    nowSelectScheduleTypeList.Add(Managers.Data.GetOneDayDataByName((GoOutType)i));
                    GameObject go = Instantiate(UISubContent, ParentTR, false);
                    go.GetComponent<UI_SubContent>().SetInfo(Managers.Data.GetOneDayDataByName((GoOutType)i), _SevenDayScheduleDatas[(int)_nowSelectedDay], _nowSelectedDay, (StatName)(i/3) );
                }
                break;
        }
        StartCoroutine(moveScroll());
    }

    IEnumerator moveScroll()
    {
        yield return new WaitForEndOfFrame();
        if (_SevenDayScheduleDatas[(int)_nowSelectedDay] != null)
        {
            float targetValue = _SeveDayScrollVarValue[(int)_nowSelectedDay]; // 목표 값 설정

            // Dotween을 사용하여 scrollRect.horizontalScrollbar.value를 목표 값까지 움직이기
            DOTween.To(() => scrollRect.horizontalScrollbar.value, x => scrollRect.horizontalScrollbar.value = x, targetValue, moveDuration).SetEase(ease);
        }
        else
        {
            scrollRect.horizontalScrollbar.value = 0;
        }
    }

    public void DeleteAllChildren()
    {
        int childCount = ParentTR.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            Destroy(ParentTR.GetChild(i).gameObject);
        }
    }


    public void SetDaySchedule(OneDayScheduleData data)
    {
        _SevenDayScheduleDatas[(int)_nowSelectedDay] = data;
        Managers.Data._SevenDayScheduleDatas = _SevenDayScheduleDatas;
        GetGameObject((int)GameObjects.Category).SetActive(false);
        UpdateDayBTN_Interactable();
        ClickLastDay_PlusOne();
    }

    void ClickLastDay_PlusOne()
    {
        int i = 0;
        for (;i<7;i++)
        {
            if(_SevenDayScheduleDatas[i] == null)
            {
                _nowSelectedDay = (SevenDays)i;
                break;
            }
        }
        if (i == 7) i = (int)_nowSelectedDay;
        ClickDayBTN(i);
        UpdateColor_Interactable_DaySelectedbtns();
    }

    public void ResetSchedule()
    {
        int temp = 0;
        for (int i = 0; i < 7; i++)
        {
            if(_SevenDayScheduleDatas[i] != null) temp += _SevenDayScheduleDatas[i].MoneyCost;
            _SevenDayScheduleDatas[i] = null;
            _SeveDayScrollVarValue[i] = 0;
            Managers.Data._SevenDayScheduleDatas[i] = null;
            Managers.Data._SeveDayScrollVarValue[i] = 0;
            
        }
        Managers.Data.PlayerData.nowGoldAmount += temp;
        UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
        SetNowSelectedFrameBox();
        UpdateDayBTN_Interactable();
        ClickLastDay_PlusOne();
    }

    private void OnDisable()
    {
        if(MM.Inst != null)
            MM.Inst.SetState(MMState.usual);
        UI_MainBackUI.instance.BackBTNWithoutSound();
    }

    public bool IsShowing3ContentsUI()
    {
        if (GetGameObject((int)GameObjects.Contents3) != null)
        {
            if (GetGameObject((int)GameObjects.Contents3).activeSelf)
                return true;
        }
        return false;
    }

    public void SaveScrollVarValue(float value)
    {
        _SeveDayScrollVarValue[(int)_nowSelectedDay] = value;
        Managers.Data._SeveDayScrollVarValue = _SeveDayScrollVarValue;
    }
    /// <summary>
    /// 요일 선택된 모양 표시 박스
    /// </summary>
    void SetNowSelectedFrameBox()
    {
        for (int i = 0; i < 7; i++)
        {
            if (_SevenDayScheduleDatas[i] == null)
            {
                _nowSelectedDay = (SevenDays)i;
                break;
            }
        }
        GetImage((int)Images.NowDayFrame).transform.DOMoveX(((int)_nowSelectedDay - 3) * 40, 0f);
    }
}