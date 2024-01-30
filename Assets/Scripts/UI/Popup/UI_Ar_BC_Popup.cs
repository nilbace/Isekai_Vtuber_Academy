using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Ar_BC_Popup : UI_Popup
{
    public static object tasktype;
    OneDayScheduleData oneDayScheduleData;
    public Animator ScreenAnimator;
    public Animator RubiaAnimator;
    public TMPro.TMP_Text Infotext;
    public Button BTN_Close;
    void Start()
    {
        Init();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            ScreenAnimator.SetTrigger(oneDayScheduleData.PathName);
            RubiaAnimator.SetTrigger(oneDayScheduleData.RubiaAni);
        }
    }

    public override void Init()
    {
        base.Init();

        BTN_Close.onClick.AddListener(CloseBTN);
        oneDayScheduleData = Managers.Data.GetOneDayDataByObject(tasktype);
        ScreenAnimator.speed = RubiaAnimator.speed = ScreenAniSpeed;
        ScreenAnimator.SetTrigger(oneDayScheduleData.PathName);
        if(tasktype is BroadCastType)
        {
            RubiaAnimator.SetTrigger(oneDayScheduleData.RubiaAni);
        }
        else
        {
            RubiaAnimator.gameObject.SetActive(false);
        }
        Infotext.text = oneDayScheduleData.ArchiveInfoText;
    }
}
