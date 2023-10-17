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

        thisBTN.onClick.AddListener(OnClicked);

        LikePressed();
    }

    public void SetInfo(OneDayScheduleData scheduleData, int nowSchCost, bool isSelected)
    {
        thisBTN.onClick.RemoveAllListeners();
        thisSubSchedleData = scheduleData;
        GetText((int)Texts.NameTMP).text = thisSubSchedleData.KorName;
        GetText((int)Texts.InfoTMP).text = thisSubSchedleData.infotext;
        GetText((int)Texts.SubUp).text = thisSubSchedleData.KorName;
        GetText((int)Texts.GoldUp).text = thisSubSchedleData.KorName;

        thisBTN.onClick.AddListener(() => SetSchedule(nowSchCost));
        thisBTN.interactable = true;
        if (Managers.Data._myPlayerData.nowGoldAmount+nowSchCost < scheduleData.MoneyCost)
        {
            thisBTN.interactable = false;
        }

        
    }

    void SetSchedule(int nowCost)
    {
        Managers.Data._myPlayerData.nowGoldAmount += nowCost;
        Managers.Data._myPlayerData.nowGoldAmount -= thisSubSchedleData.MoneyCost;
        UI_MainBackUI.instance.UpdateUItexts();
        UI_SchedulePopup.instance.SetDaySchedule(thisSubSchedleData);
    }
    float offset = 5.5f;
    private bool isPressed = false;
    bool TruelyInteractable =  false;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (TruelyInteractable) return;
        isPressed = true;
        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            tr.localPosition += new Vector3(0, -offset, 0);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (TruelyInteractable) return;
        isPressed = false;
        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            tr.localPosition += new Vector3(0, offset, 0);
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (TruelyInteractable) return;
        if (isPressed)
            eventData.pointerPress = gameObject;
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

    void OnClicked()
    {
        Debug.Log("클릭 완료");
    }
}
