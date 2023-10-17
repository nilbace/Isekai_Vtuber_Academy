using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using static UI_SchedulePopup;

public class UI_SubContent : UI_Base
{
    public OneDayScheduleData thisSubSchedleData;
    public Button thisBTN;

    enum Texts
    { Text }

    string _name;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        thisBTN = GetComponent<Button>();
        Bind<TMP_Text>(typeof(Texts));
    }

    public void SetInfo(OneDayScheduleData scheduleData, int nowSchCost)
    {
        if (scheduleData == null)
        {
            GetText(0).text = "";
            thisBTN.interactable = false;
            return;
        }

        thisBTN.onClick.RemoveAllListeners();
        thisSubSchedleData = scheduleData;
        GetText(0).text = thisSubSchedleData.KorName;
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
}
