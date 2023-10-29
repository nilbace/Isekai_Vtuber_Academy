using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DataManager;
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
    bool SubContentSelectPhase = false;
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

        StartScheduleBTN,
        BackBTN,

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
        Sub0, Sub1, Sub2, Sub3, 
    }
    enum Images
    {
        ItemIcon,
        ScheduleSlotSelected,
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        Init();
        MM.instance.NowMMState = MM.MMState.OnSchedule;
    }

    /// <summary>
    /// 방송 휴식 외출 선택
    /// </summary>
    void State_SelectType()
    {
        SubContentSelectPhase = false;
        GetGameObject((int)GameObjects.Contents3).SetActive(true);
        GetGameObject((int)GameObjects.SubContents).SetActive(false);
    }

    /// <summary>
    /// 하위 컨텐츠 선택
    /// </summary>
    void State_SelectSubContent()
    {
        SubContentSelectPhase = true;
        GetGameObject((int)GameObjects.Contents3).SetActive(false);
        GetGameObject((int)GameObjects.SubContents).SetActive(true);
    }
    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));


        for (int i = 0; i<7; i++)
        {
            int inttemp = i;
            Button temp = GetButton(i);
            temp.onClick.AddListener( () => ClickDay(inttemp));
        }

        scrollRect = GetComponentInChildren<ScrollRect>();
        GetGameObject((int)GameObjects.SubContents).SetActive(false);
        GetButton((int)Buttons.BroadCastBTN).onClick.       AddListener(ClickBroadCastBTN);
        GetButton((int)Buttons.RestBTN).onClick.            AddListener(ClickRestBTN);
        GetButton((int)Buttons.GoOutBTN).onClick.           AddListener(ClickGoOutBTN);
        GetButton((int)Buttons.StartScheduleBTN).onClick.   AddListener(StartScheduleBTN);
        GetButton((int)Buttons.BackBTN).onClick.            AddListener(BackBTN);

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

    public void StoreScrollVarValue(float value)
    {
        _SeveDayScrollVarValue[(int)_nowSelectedDay] = value;
        Managers.Data._SeveDayScrollVarValue = _SeveDayScrollVarValue;
    }

    /// <summary>
    /// 선택된 요일 표시
    /// </summary>
    void SetSelectBox()
    {
        int i = 0;
        for (; i < 7; i++)
        {
            if (_SevenDayScheduleDatas[i] == null)
            {
                _nowSelectedDay = (SevenDays)i;
                break;
            }
        }
        if (i == 7) i = 6;

        //저장된 곳에서 선택박스 위치시키게 함
        GetImage(1).transform.DOMoveX(((int)_nowSelectedDay - 3) * 40, 0f);
    }
    
    void ClickDay(int i)
    {
        _nowSelectedDay = (SevenDays)i;
        if (_SevenDayScheduleDatas[(int)_nowSelectedDay] != null)
        {
            if(_SevenDayScheduleDatas[(int)_nowSelectedDay].scheduleType == ScheduleType.BroadCast)
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
        else
        {
            State_SelectType();
        }
        UpdateColorAndSelected();
    }

    /// <summary>
    /// 버튼이 interactable 갱신
    /// </summary>
    void UpdateBTN_Interactable()
    {
        int i = 0;

        for(;i<7;i++)
        {
            GetButton((int)Buttons.StartScheduleBTN).interactable = false;
            GetButton((int)Buttons.StartScheduleBTN).GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
            if (_SevenDayScheduleDatas[i] == null)
            {
                _nowSelectedDay = (SevenDays)i;
                break;
            }
            if (i == 6)
            {
                GetButton((int)Buttons.StartScheduleBTN).interactable = true;
                GetButton((int)Buttons.StartScheduleBTN).GetComponent<Image>().color = new Color(1, 1, 1, 1f);
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
    /// <summary>
    /// 색상 지정용 함수
    /// </summary>
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


    #region Schedules

    void ClickBroadCastBTN()
    {
        State_SelectSubContent();
        ChooseScheduleTypeAndFillList(ScheduleType.BroadCast);
    }
    void ClickRestBTN()
    {
        State_SelectSubContent();
        ChooseScheduleTypeAndFillList(ScheduleType.Rest);
    }

    void ClickGoOutBTN()
    {
        State_SelectSubContent();
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
                for (int i = 0; i < (int)BroadCastType.MaxCount; i++)
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

    /// <summary>
    /// 세부 컨텐츠들 전부 삭제 > 다시 만들기 전에
    /// </summary>
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
        //저장소 저장
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
        ClickDay(i);
        UpdateColorAndSelected();
    }


    void StartScheduleBTN()
    {
        Managers.instance.StartSchedule();
        Managers.UI_Manager.ClosePopupUI();
    }
    void BackBTN()
    {
        if(SubContentSelectPhase)
        {
            SubContentSelectPhase = false;
            GetGameObject((int)GameObjects.Contents3).SetActive(true);
            GetGameObject((int)GameObjects.SubContents).SetActive(false);
        }
        else
        {
            UI_MainBackUI.instance.ShowCreateScheduleBTN();
            Managers.UI_Manager.ClosePopupUI();
        }
    }

    public void ResetSchedule()
    {
        for (int i = 0; i < 7; i++)
        {
            _SevenDayScheduleDatas[i] = null;
            _SeveDayScrollVarValue[i] = 0;
            Managers.Data._SevenDayScheduleDatas[i] = null;
            Managers.Data._SeveDayScrollVarValue[i] = 0;
            SetSelectBox();
            UpdateBTN_Interactable();
            ClickLastDay_PlusOne();
            UpdateColorAndSelected();
        }
    }

    private void OnDisable()
    {
        MM.instance.NowMMState = MM.MMState.usual;
    }

    #endregion
}