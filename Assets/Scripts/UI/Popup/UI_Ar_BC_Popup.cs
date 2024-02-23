using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Ar_BC_Popup : UI_Popup
{
    public static object tasktype;
    public static bool isCold;
    public static bool isRunAway;
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
            if (isCold || isRunAway)
            {
                ScreenAnimator.SetTrigger(isCold ? "Cold" : "RunAway");
            }
            else
            {
                ScreenAnimator.SetTrigger(oneDayScheduleData.PathName);
                RubiaAnimator.SetTrigger(oneDayScheduleData.RubiaAni);
            }
        }
    }

    public override void Init()
    {
        base.Init();

        BTN_Close.onClick.AddListener(CloseBTN);
        if(!isCold && !isRunAway)
        {
            oneDayScheduleData = Managers.Data.GetOneDayDataByScheduleType((ScheduleType)tasktype);
            Managers.Data.PersistentUser.WatchedScehdule[(ScheduleType)tasktype] = true;
            Managers.Data.SavePersistentData();
        }
        ScreenAnimator.speed = RubiaAnimator.speed = ScreenAniSpeed;
        if(isCold || isRunAway)
        {
            ScreenAnimator.SetTrigger(isCold ? "Cold" : "RunAway");
            Infotext.text = isCold ?
                "지나친 방송은 컨디션 난조를 초래합니다. 뮹뮹이의 간호를 받도록 합시다." :
                "가출한 어린 드래곤을 어떻게 달래야 집으로 돌아올까요?";
            if(isCold)
            {
                Managers.Data.PersistentUser.WatchedScehdule[ScheduleType.Caught] = true;
                Managers.Data.SavePersistentData();
            }
            else if(isRunAway)
            {
                Managers.Data.PersistentUser.WatchedScehdule[ScheduleType.RunAway] = true;
                Managers.Data.SavePersistentData();
            }
        }
        else
        {
            ScreenAnimator.SetTrigger(oneDayScheduleData.PathName);
            Infotext.text = oneDayScheduleData.ArchiveInfoText;
        }
     
        if(tasktype !=null &&  (int)tasktype < (int)ScheduleType.Commission)
        {
            RubiaAnimator.SetTrigger(oneDayScheduleData.RubiaAni);
        }
        else
        {
            RubiaAnimator.gameObject.SetActive(false);
        }

        //레드닷 업데이트 파트
        UI_ArchiveList.instance.SetListByState();
        UI_Archive.instance.UpdateRedDot();
        UI_MainBackUI.instance.UpdateReddot();
    }

    private void OnDisable()
    {
        isCold = false;
        isRunAway = false;
    }
}
