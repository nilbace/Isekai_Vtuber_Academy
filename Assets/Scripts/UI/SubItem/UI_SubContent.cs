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
    public Button thisBTN;
    SevenDays thisBTNDay;
    ScrollRect scrollRect;

    [System.Serializable]
    public struct Pozs
    {
        public Vector3 HeartPoz;
        public Vector3 StarPoz;
        public Vector3 StatIconPoz;
        public Vector3 StatUDPoz;
        public Vector3 SubTextPoz;
        public Vector3 GoldTextPoz;
        
    }

    public Pozs BroadCastpoz = new Pozs();
    public Pozs Restpoz = new Pozs();
    public Pozs GOoutpoz = new Pozs();

    void SetPozition(Pozs pozs)
    {
        GetImage((int)Images.HeartUD).transform.localPosition = pozs.HeartPoz;
        GetImage((int)Images.StarUD).transform.localPosition = pozs.StarPoz;
        GetImage((int)Images.StatIcon).transform.localPosition = pozs.StatIconPoz;
        GetImage((int)Images.StatUD).transform.localPosition = pozs.StatUDPoz;
        GetText((int)Texts.SubTMP).transform.localPosition = pozs.SubTextPoz;
        GetText((int)Texts.GoldTMP).transform.localPosition = pozs.GoldTextPoz;
    }
 
    enum Images
    {
        HeartUD, StarUD, StatIcon, StatUD
    }
    enum Texts
    { 
        NameTMP,
        InfoTMP,
        SubTMP,
        GoldTMP
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

    public void SetInfo(OneDayScheduleData scheduleData, OneDayScheduleData settedData, SevenDays nowDay)
    {
        thisSubSchedleData = scheduleData;
        thisBTNDay = nowDay;    
        SpriteState spriteState = new SpriteState();
        switch (scheduleData.scheduleType)
        {
            case ScheduleType.BroadCast:
                GetComponent<Image>().sprite = Managers.MSM.DaysPannel[0];
                spriteState.pressedSprite = Managers.MSM.DaysPannel[1];
                thisBTN.spriteState = spriteState;
                SetPozition(BroadCastpoz);
                break;
            case ScheduleType.Rest:
                GetComponent<Image>().sprite = Managers.MSM.DaysPannel[2];
                spriteState.pressedSprite = Managers.MSM.DaysPannel[3];
                thisBTN.spriteState = spriteState;
                SetPozition(Restpoz);
                break;
            case ScheduleType.GoOut:
                GetComponent<Image>().sprite = Managers.MSM.DaysPannel[4];
                spriteState.pressedSprite = Managers.MSM.DaysPannel[5];
                thisBTN.spriteState = spriteState;
                SetPozition(GOoutpoz);
                break;
        }

        GetText((int)Texts.NameTMP).text    = thisSubSchedleData.KorName;
        GetText((int)Texts.InfoTMP).text    = thisSubSchedleData.infotext;
        GetText((int)Texts.SubTMP).text      = thisSubSchedleData.KorName;
        GetText((int)Texts.GoldTMP).text     =  (-thisSubSchedleData.MoneyCost).ToString();

        if(scheduleData.scheduleType == ScheduleType.BroadCast)
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

        if(scheduleData.scheduleType != ScheduleType.BroadCast)
        {
            if(settedData == null)
            {
                if(Managers.Data._myPlayerData.nowGoldAmount < scheduleData.MoneyCost)
                {
                    thisBTN.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    thisBTN.interactable = false;
                }
            }
            else
            {
                if (Managers.Data._myPlayerData.nowGoldAmount + settedData.MoneyCost < scheduleData.MoneyCost)
                {
                    thisBTN.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    thisBTN.interactable = false;
                }
            }
            
        }
        
        if(scheduleData == settedData)
        {
            LikePressed();
        }
    }

    void SetSchedule(int nowCost)
    {
        if (OverOffset) return;
        Managers.Data._myPlayerData.nowGoldAmount += nowCost;
        Managers.Data._myPlayerData.nowGoldAmount -= thisSubSchedleData.MoneyCost;
        UI_MainBackUI.instance.UpdateUItexts();
        UI_SchedulePopup.instance.SetDaySchedule(thisSubSchedleData);
    }



    [SerializeField] float offset = 5.5f;
    private bool isPressed = false;
    bool TruelyInteractable =  false;
    Vector2 pressedPosition;
    [SerializeField] float Offset;

    public void OnPointerDown(PointerEventData eventData)
    {
        OverOffset = false;
        if (TruelyInteractable) return;
        isPressed = true;
        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            tr.localPosition += new Vector3(0, -offset, 0);
        }
        pressedPosition = eventData.position; // 눌린 위치 저장
    }
    
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (TruelyInteractable) return;
        isPressed = false;

        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            tr.localPosition += new Vector3(0, offset, 0);
        }
        UI_SchedulePopup.instance.StoreScrollVarValue(scrollRect.horizontalScrollbar.value);
    }

    bool OverOffset = false;
    public void OnDrag(PointerEventData eventData)
    {
        if (isPressed) eventData.pointerPress = gameObject;
        float deltaX = Mathf.Abs(eventData.position.x - pressedPosition.x);
        if (deltaX > Offset)
        {
            OverOffset = true;
        }

        if (scrollRect != null && scrollRect.horizontalScrollbar != null && scrollRect.horizontalScrollbar.gameObject.activeInHierarchy)
        {
            float normalizedDeltaX = -eventData.delta.x / scrollRect.content.rect.width;
            float newXPos = Mathf.Clamp(scrollRect.horizontalNormalizedPosition + normalizedDeltaX, 0f, 1f); 
            scrollRect.horizontalNormalizedPosition = newXPos; 
        }
    }

    void LikePressed()
    {
        TruelyInteractable = true;
        thisBTN.interactable = false;
        switch (thisSubSchedleData.scheduleType)
        {
            case ScheduleType.BroadCast:
                GetComponent<Image>().sprite = Managers.MSM.DaysPannel[1];
                break;
            case ScheduleType.Rest:
                GetComponent<Image>().sprite = Managers.MSM.DaysPannel[3];
                break;
            case ScheduleType.GoOut:
                GetComponent<Image>().sprite = Managers.MSM.DaysPannel[5];
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

        GetText((int)Texts.SubTMP).text = ExpectedSub(tempSub, thisSubSchedleData).ToString();
        GetText((int)Texts.GoldTMP).text = Mathf.CeilToInt(tempSub * thisSubSchedleData.InComeMag).ToString();
    }

    int ExpectedSub(int SubCount, OneDayScheduleData oneDayScheduleData)
    {
        if(oneDayScheduleData.scheduleType == ScheduleType.BroadCast)
            return CalculateSubAfterDay(SubCount, oneDayScheduleData.FisSubsUpValue, oneDayScheduleData.PerSubsUpValue, Managers.Data.GetNowWeekBonusMag());
        return 0;
    }

    int CalculateSubAfterDay(int now, float fix, float per, float bonus)
    {
        float temp = (now + fix) * ((float)(100 + per) / 100f) * bonus;
        int result = Mathf.CeilToInt(temp);
        return result - now;
    }
}
