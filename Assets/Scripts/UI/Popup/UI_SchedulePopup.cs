using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using DG.Tweening;

/// <summary>
/// 스케쥴 관리와 방송 정보에 대한 정보가 담겨있는 스크립트
/// </summary>
public class UI_SchedulePopup : UI_Popup
{
    public static UI_SchedulePopup instance;
    public Transform ParentTR;
    public GameObject UISubContent;
    private ScrollRect scrollRect;

    enum Buttons
    {
        MondayBTN,
        TuesdayBTN,
        WednesdayBTN,
        ThursdayBTN,
        FridayBTN,
        SaturdayBTN,
        SundayBTN,

        BroadCastBTN,
        RestBTN,
        GoOutBTN,
    }

    Button StartScheduleBTN_Main;
    Button BackBTN_Main;
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
        Sub0, Sub1, Sub2, Sub3, 
    }
    enum Images
    {
        ItemIcon,
        NowDayFrame,
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        Init();

        MM.Inst.SetState(MMState.OnSchedule);

        Canvas canvas = Util.GetOrAddComponent<Canvas>(gameObject);
        canvas.sortingOrder = 1;
    }
   
    void TransitionToThreeContents()
    {
        GetGameObject((int)GameObjects.Contents3).SetActive(true);
        GetGameObject((int)GameObjects.SubContents).SetActive(false);
    }

    void TransitionToSelectSubContent()
    {
        GetGameObject((int)GameObjects.Contents3).SetActive(false);
        GetGameObject((int)GameObjects.SubContents).SetActive(true);
    }
    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        StartScheduleBTN_Main = UI_MainBackUI.instance.GetStartScheduleBTN();
        BackBTN_Main = UI_MainBackUI.instance.GetBackBTN();

        for (int i = 0; i<7; i++)
        {
            int inttemp = i;
            Button temp = GetButton(i);
            temp.onClick.AddListener( () => ClickDayBTN(inttemp));
        }

        scrollRect = GetComponentInChildren<ScrollRect>();
        GetGameObject((int)GameObjects.SubContents).SetActive(false);
        GetButton((int)Buttons.BroadCastBTN).onClick.       AddListener(ClickBroadCastBTN);
        GetButton((int)Buttons.RestBTN).onClick.            AddListener(ClickRestBTN);
        GetButton((int)Buttons.GoOutBTN).onClick.           AddListener(ClickGoOutBTN);

        _SeveDayScrollVarValue = Managers.Data._SeveDayScrollVarValue;
        _SevenDayScheduleDatas = Managers.Data._SevenDayScheduleDatas;

        SetSelectBox();
        UpdateBTN_Interactable();
        ClickLastDay_PlusOne();
    }

    #region ScheduleCheck
    

    SevenDays _nowSelectedDay = SevenDays.Monday;
    OneDayScheduleData[] _SevenDayScheduleDatas = new OneDayScheduleData[7];
    float[] _SeveDayScrollVarValue =  new float[7];

    public void SetScrollVarValue(float value)
    {
        _SeveDayScrollVarValue[(int)_nowSelectedDay] = value;
        Managers.Data._SeveDayScrollVarValue = _SeveDayScrollVarValue;
    }

    void SetSelectBox()
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
    
    void ClickDayBTN(int dayIndex)
    {
        _nowSelectedDay = (SevenDays)dayIndex;
        if (_SevenDayScheduleDatas[(int)_nowSelectedDay] == null)
        {
            TransitionToThreeContents();
        }
        else
        {
            if (_SevenDayScheduleDatas[(int)_nowSelectedDay].scheduleType == ScheduleType.BroadCast)
            {
                ClickBroadCastBTN();
            }
            else if (_SevenDayScheduleDatas[(int)_nowSelectedDay].scheduleType == ScheduleType.Rest)
            {
                ClickRestBTN();
            }
            else
            {
                ClickGoOutBTN();
            }
        }
        UpdateColorAndSelected();
    }


    void UpdateBTN_Interactable()
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
    [SerializeField] Ease ease;
    [SerializeField] float moveDuration;
    [SerializeField] Color[] baseTextColor;

    void UpdateColorAndSelected()
    {
        for(int i = 0; i<7;i++)
        {
            if (i == (int)_nowSelectedDay)
            {
                GetImage(1).transform.DOMoveX((i - 3) * 40, moveDuration).SetEase(ease);
            }

            if (_SevenDayScheduleDatas[i] == null)
            {
                GetButton(i).GetComponentInChildren<TMPro.TMP_Text>().color = baseTextColor[0];
                if (GetButton(i).interactable == true)
                {
                    GetButton(i).GetComponent<Image>().sprite = Managers.MSM.Days[3];
                }
                else
                {
                    GetButton(i).GetComponent<Image>().sprite = Managers.MSM.Days[4];
                }
            }
            else if(_SevenDayScheduleDatas[i].scheduleType == ScheduleType.BroadCast)
            {
                GetButton(i).GetComponent<Image>().sprite = Managers.MSM.Days[0];
                GetButton(i).GetComponentInChildren<TMPro.TMP_Text>().color = baseTextColor[1];
            }
            else if(_SevenDayScheduleDatas[i].scheduleType == ScheduleType.Rest)
            {
                GetButton(i).GetComponent<Image>().sprite = Managers.MSM.Days[1];
                GetButton(i).GetComponentInChildren<TMPro.TMP_Text>().color = baseTextColor[2];

            }
            else
            {
                GetButton(i).GetComponent<Image>().sprite = Managers.MSM.Days[2];
                GetButton(i).GetComponentInChildren<TMPro.TMP_Text>().color = baseTextColor[3];
            }
        }
    }

    public void Show3Contents()
    {
        GetGameObject((int)GameObjects.Contents3).SetActive(true);
        GetGameObject((int)GameObjects.SubContents).SetActive(false);
    }


    #region Schedules

    void ClickBroadCastBTN()
    {
        Managers.Sound.Play("SmallBTN", Sound.Effect);
        TransitionToSelectSubContent();
        ChooseScheduleTypeAndFillList(ScheduleType.BroadCast);
    }
    void ClickRestBTN()
    {
        Managers.Sound.Play("SmallBTN", Sound.Effect);
        TransitionToSelectSubContent();
        ChooseScheduleTypeAndFillList(ScheduleType.Rest);
    }

    void ClickGoOutBTN()
    {
        Managers.Sound.Play("SmallBTN", Sound.Effect);
        TransitionToSelectSubContent();
        ChooseScheduleTypeAndFillList(ScheduleType.GoOut);
    }

    List<OneDayScheduleData> nowSelectScheduleTypeList = new List<OneDayScheduleData>();
    void ChooseScheduleTypeAndFillList(ScheduleType type)
    {
        nowSelectScheduleTypeList.Clear();
        DeleteAllChildren();
        switch (type)
        {
            case ScheduleType.BroadCast:
                for (int i = 0; i < (int)BroadCastType.MaxCount_Name; i++)
                {
                    nowSelectScheduleTypeList.Add(Managers.Data.GetOneDayDataByName((BroadCastType)i));
                    GameObject go = Instantiate(UISubContent, ParentTR, false);
                    go.GetComponent<UI_SubContent>().SetInfo(Managers.Data.GetOneDayDataByName((BroadCastType)i),_SevenDayScheduleDatas[(int)_nowSelectedDay], _nowSelectedDay, StatName.FALSE);
                }
                break;

            case ScheduleType.Rest:
                for (int i = 0; i < (int)RestType.MaxCount; i++)
                {
                    nowSelectScheduleTypeList.Add(Managers.Data.GetOneDayDataByName((RestType)i));
                    GameObject go = Instantiate(UISubContent, ParentTR, false);
                    go.GetComponent<UI_SubContent>().SetInfo(Managers.Data.GetOneDayDataByName((RestType)i), _SevenDayScheduleDatas[(int)_nowSelectedDay], _nowSelectedDay, StatName.FALSE);
                }
                break;

            case ScheduleType.GoOut:
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
        
        UpdateBTN_Interactable();
        ClickLastDay_PlusOne();
    }
    #endregion

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
        UpdateColorAndSelected();
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
        Managers.Data._myPlayerData.nowGoldAmount += temp;
        UI_MainBackUI.instance.UpdateUItexts();
        SetSelectBox();
        UpdateBTN_Interactable();
        ClickLastDay_PlusOne();
    }

    private void OnDisable()
    {
        if(MM.Inst != null)
            MM.Inst.SetState(MMState.usual);
    }

    public bool IsShowing3ContentsUI()
    {
        if (GetGameObject((int)GameObjects.Contents3).activeSelf)
            return true;
        return false;
    }

    #endregion
}