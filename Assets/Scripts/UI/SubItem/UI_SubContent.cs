using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UI_SchedulePopup;

public class UI_SubContent : UI_Base, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public OneDayScheduleData thisSubSchedleData;
    public Button thisBTN;
    ScrollRect scrollRect;

    enum Images
    {
        HeartUD, StarUD
    }
    enum Texts
    { 
        NameTMP,
        InfoTMP,
        SubUp,
        GoldUp
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

    public void SetInfo(OneDayScheduleData scheduleData, OneDayScheduleData settedData)
    {
        thisSubSchedleData = scheduleData;
        GetText((int)Texts.NameTMP).text = thisSubSchedleData.KorName;
        GetText((int)Texts.InfoTMP).text = thisSubSchedleData.infotext;
        GetText((int)Texts.SubUp).text = thisSubSchedleData.KorName;
        GetText((int)Texts.GoldUp).text = thisSubSchedleData.KorName;

        if(settedData == null)
        {
            thisBTN.onClick.AddListener(() => SetSchedule(0));
        }
        else
        {
            thisBTN.onClick.AddListener(() => SetSchedule(settedData.MoneyCost));
        }
        thisBTN.interactable = true;

        if(settedData != null)
        {
            if (Managers.Data._myPlayerData.nowGoldAmount + settedData.MoneyCost < scheduleData.MoneyCost)
            {
                thisBTN.interactable = false;
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
    float offset = 5.5f;
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
        GetComponent<Image>().sprite = thisBTN.spriteState.pressedSprite;
        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            tr.localPosition += new Vector3(0, -offset, 0);
        }
    }   
}
