using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_SubContent : UI_Base, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public OneDayScheduleData thisSubSchedleData;
    [HideInInspector] public Button thisBTN;
    public Sprite[] TaskPannelSprites;
    public Sprite[] MiniStatIcons;
    SevenDays thisBTNDay;
    ScrollRect scrollRect;

    [System.Serializable]
    public struct Pozs
    {
        public Vector3 HeartPoz;
        public Vector3 StarPoz;
        public Vector3 StatIconPoz;
        public Vector3 SubTextPoz;
        public Vector3 GoldTextPoz;
        public Vector3 StatTextPoz;
    }

    public Pozs BroadCastpoz = new Pozs();
    public Pozs Restpoz = new Pozs();
    public Pozs GOoutpoz = new Pozs();

    void SetPozition(Pozs pozs)
    {
        GetImage((int)Images.StatIcon).transform.localPosition = pozs.StatIconPoz;
        GetText((int)Texts.HeartTMP).transform.localPosition = pozs.HeartPoz;
        GetText((int)Texts.StarTMP).transform.localPosition = pozs.StarPoz;
        GetText((int)Texts.SubTMP).transform.localPosition = pozs.SubTextPoz;
        GetText((int)Texts.GoldTMP).transform.localPosition = pozs.GoldTextPoz;
        GetText((int)Texts.StatTMP).transform.localPosition = pozs.StatTextPoz;
    }
 
    enum Images
    {
        StatIcon
    }
    enum Texts
    { 
        NameTMP,
        InfoTMP,
        SubTMP,
        GoldTMP,
        HeartTMP,
        StarTMP,
        StatTMP
    }

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        thisBTN = GetComponent<Button>();
        Bind<TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        scrollRect = GetComponentInParent<ScrollRect>();
    }

    public void SetInfo(OneDayScheduleData scheduleData, OneDayScheduleData settedData, SevenDays nowDay, StatName statName)
    {
        thisSubSchedleData = scheduleData;
        thisBTNDay = nowDay;    
        SpriteState ButtonBackImage = new SpriteState();
        switch (scheduleData.scheduleType)
        {
            case ScheduleType.BroadCast:
                GetComponent<Image>().sprite = TaskPannelSprites[0];
                ButtonBackImage.pressedSprite = TaskPannelSprites[1];
                thisBTN.spriteState = ButtonBackImage;
                SetPozition(BroadCastpoz);
                break;
            case ScheduleType.Rest:
                GetComponent<Image>().sprite = TaskPannelSprites[2];
                ButtonBackImage.pressedSprite = TaskPannelSprites[3];
                thisBTN.spriteState = ButtonBackImage;
                SetPozition(Restpoz);
                break;
            case ScheduleType.GoOut:
                GetComponent<Image>().sprite = TaskPannelSprites[4];
                ButtonBackImage.pressedSprite = TaskPannelSprites[5];
                thisBTN.spriteState = ButtonBackImage;
                SetPozition(GOoutpoz);
                break;
        }

        GetText((int)Texts.NameTMP).text    = thisSubSchedleData.KorName;
        GetText((int)Texts.InfoTMP).text    = thisSubSchedleData.infotext;
        GetText((int)Texts.SubTMP).text     = thisSubSchedleData.KorName;
        GetText((int)Texts.GoldTMP).text    = (-thisSubSchedleData.MoneyCost).ToString();
        GetText((int)Texts.HeartTMP).text = HeartStarVarianceToString(scheduleData, StatName.Strength);
        GetText((int)Texts.StarTMP).text  = HeartStarVarianceToString(scheduleData, StatName.Mental);
        if (scheduleData.scheduleType == ScheduleType.GoOut)
        {
            GetText((int)Texts.StatTMP).text = thisSubSchedleData.Six_Stats[(int)statName].ToString();
        }

        if (scheduleData.scheduleType == ScheduleType.BroadCast)
        {
            SetMoneyAndSubData_BroadCast();
        }

        if(settedData == null)
        {
            thisBTN.onClick.AddListener(() => SetSchedule(0));
        }
        else
        {
            thisBTN.onClick.AddListener(() => SetSchedule(settedData.MoneyCost));
        }
        thisBTN.interactable = true;

        //외출, 휴식이라면
        if(scheduleData.scheduleType != ScheduleType.BroadCast)
        {
            //먼저 선택된 것이 없다면
            if(settedData == null)
            {
                //내 소지금보다 비싼 활동이라면 안눌림
                if(Managers.Data._myPlayerData.nowGoldAmount < scheduleData.MoneyCost && scheduleData.MoneyCost != 0)
                {
                    thisBTN.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    thisBTN.interactable = false;
                    TruelyInteractable = false;
                }
            }
            //선택된것이 있고, 비싸다면 안눌림
            else
            {
                if (Managers.Data._myPlayerData.nowGoldAmount + settedData.MoneyCost < scheduleData.MoneyCost && scheduleData.MoneyCost != 0)
                {
                    thisBTN.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    thisBTN.interactable = false;
                    TruelyInteractable = false;
                }
            }
            
        }
        
        if(scheduleData == settedData)
        {
            LikePressed();
        }

        if(statName != StatName.FALSE)
        {
            GetImage((int)Images.StatIcon).sprite = MiniStatIcons[(int)statName];
        }
    }

    void SetSchedule(int nowCost)
    {
        Managers.Sound.Play(Define.Sound.SubcontentBTN);
        if (OverOffset) return;
        Managers.Data._myPlayerData.nowGoldAmount += nowCost;
        Managers.Data._myPlayerData.nowGoldAmount -= thisSubSchedleData.MoneyCost;
        UI_MainBackUI.instance.UpdateUItexts();
        UI_SchedulePopup.instance.SetDaySchedule(thisSubSchedleData);
    }



    [SerializeField] float offset = 5.5f;
    private bool isPressed = false;
    bool TruelyInteractable =  true;
    Vector2 pressedPosition;
    [SerializeField] float MouseDragOffset;

    public void OnPointerDown(PointerEventData eventData)
    {
        OverOffset = false;

        if (!TruelyInteractable) { scrollRect.OnBeginDrag(eventData); return; }
        isPressed = true;
        
        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            tr.localPosition += new Vector3(0, -offset, 0);
        }
        pressedPosition = eventData.position; // 눌린 위치 저장

        scrollRect.OnBeginDrag(eventData);
    }
    
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!TruelyInteractable) { scrollRect.OnBeginDrag(eventData); return; }
        isPressed = false;

        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            tr.localPosition += new Vector3(0, offset, 0);
        }
        scrollRect.OnEndDrag(eventData);
        UI_SchedulePopup.instance.SetScrollVarValue(scrollRect.horizontalScrollbar.value);
    }

    bool OverOffset = false;
    public void OnDrag(PointerEventData eventData)
    {
        PointerEventData temp = new PointerEventData(EventSystem.current);
        if (isPressed) temp.pointerPress = gameObject;
        float deltaX = Mathf.Abs(eventData.position.x - pressedPosition.x);
        if (deltaX > MouseDragOffset)
        {
            OverOffset = true;
        }

        scrollRect.OnDrag(eventData);
    }

    void LikePressed()
    {
        TruelyInteractable = false;
        thisBTN.interactable = false;
        switch (thisSubSchedleData.scheduleType)
        {
            case ScheduleType.BroadCast:
                GetComponent<Image>().sprite = TaskPannelSprites[1];
                break;
            case ScheduleType.Rest:
                GetComponent<Image>().sprite = TaskPannelSprites[3];
                break;
            case ScheduleType.GoOut:
                GetComponent<Image>().sprite = TaskPannelSprites[5];
                break;
        }
        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            tr.localPosition += new Vector3(0, -offset, 0);
        }
    }

    void SetMoneyAndSubData_BroadCast()
    {
        int tempSub = Managers.Data._myPlayerData.nowSubCount;
       
        for(int i = 0; i< (int)thisBTNDay; i++)
        {
            tempSub += ExpectedSub(tempSub, Managers.Data._SevenDayScheduleDatas[i]);
        }

        GetText((int)Texts.SubTMP).text  = ExpectedSub(tempSub, thisSubSchedleData).ToString();
        GetText((int)Texts.GoldTMP).text = ExpectedInCome(tempSub, thisSubSchedleData).ToString();
    }

    int ExpectedSub(int SubCount, OneDayScheduleData oneDayScheduleData)
    {
        if(oneDayScheduleData.scheduleType == ScheduleType.BroadCast)
        {
            Bonus temp = Managers.Data.GetMainProperty(GetStatNameByBroadCastType(oneDayScheduleData.broadcastType));
            int newSubs = ScheduleExecuter.Inst.CalculateSubAfterDay(SubCount, oneDayScheduleData.FisSubsUpValue, oneDayScheduleData.PerSubsUpValue, 1);
            int bonus = Mathf.CeilToInt(newSubs * (temp.SubBonus) / 100f);

            return newSubs + bonus;
        }
        return 0;
    }

    int ExpectedInCome(int SubCount, OneDayScheduleData oneDayScheduleData)
    {
        if (oneDayScheduleData.scheduleType == ScheduleType.BroadCast)
        {
            Bonus temp = Managers.Data.GetMainProperty(GetStatNameByBroadCastType(oneDayScheduleData.broadcastType));

            int Income = Mathf.CeilToInt(Mathf.Log10(SubCount)*30 * oneDayScheduleData.InComeMag);
            int bonus = Mathf.CeilToInt(Income * (temp.IncomeBonus) / 100f);

            return Income + bonus;
        }
        return 0;
    }


    string HeartStarVarianceToString(OneDayScheduleData oneDayScheduleData, StatName HeartOrStar)
    {
        string temp = "";
        float value;
        if (HeartOrStar == StatName.Strength)
            value = oneDayScheduleData.HeartVariance;
        else
            value = oneDayScheduleData.StarVariance;
        
        if(oneDayScheduleData.scheduleType != ScheduleType.Rest)value = value * ScheduleExecuter.Inst.GetSubStatProperty(HeartOrStar);

        temp = value.ToString("F0");

        return temp;
    }
}