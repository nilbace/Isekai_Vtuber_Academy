using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DataManager;
using static Define;
using static REventManager;

public class UI_RandomEvent : UI_Popup
{
    public static UI_RandomEvent instance;
    public static WeekEventData _eventData;
    public Sprite[] CutsceneSprites;
    public static bool ArchiveMode;

    enum Buttons
    {
        ResultBTN, ResultBTN2, Panel
    }
    enum Texts
    {
        EventText, BTN1Text, BTN2Text
    }

    enum Images
    {
        CutSceneIMG,
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        //도감으로 등장한 것이 아닌 게임 진행중 등장했다면
        //랜덤 이벤트를 받아옴
        if(!ArchiveMode) _eventData = Managers.RandEvent.GetProperEvent();
        _eventData.CheckAndAddIfNotWatched();
        Managers.NickName.CheckPerfectNickName();
        UpdateRedDot();
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<TMPro.TMP_Text>(typeof(Texts));

        GetImage((int)Images.CutSceneIMG).sprite = LoadIMG(_eventData);

        GetText((int)Texts.EventText).text = _eventData.EventInfoString;
        GetText((int)Texts.BTN1Text).text = _eventData.BTN1text;
        GetText((int)Texts.BTN2Text).text = _eventData.BTN2text;
        GetButton((int)Buttons.ResultBTN).onClick.AddListener(ChooseBTN1);
        GetButton((int)Buttons.ResultBTN2).onClick.AddListener(ChooseBTN2);
        GetButton((int)Buttons.Panel).onClick.AddListener(CloseIfArchive);
        if (_eventData.EventDataType == EventDataType.Main) GetButton((int)Buttons.ResultBTN2).interactable = false;
    }

    Sprite LoadIMG(WeekEventData Data)
    {
        Sprite sprite = null;
        if(Data.CutSceneName == "")
        {
            Debug.Log("미완성");
        }
        else
        {
            foreach (Sprite CutsceneSprite in CutsceneSprites)
            {
                if (CutsceneSprite.name == Data.CutSceneName)
                {

                    sprite = CutsceneSprite;
                }
            }
        }

        return sprite;
    }

    void ChooseBTN1()
    {
        DoOption(isOption1: true);
        UI_DefaultPopup.SetDefaultPopupUI(ArchiveMode ? DefaultPopupState.RandEventArchive : DefaultPopupState.Normal, _eventData.BTN1ResultText, "확인");
        Managers.UI_Manager.ShowPopupUI<UI_DefaultPopup>();
        Managers.Sound.Play(Define.Sound.SmallBTN);

    }

    void ChooseBTN2()
    {
        DoOption(isOption1: false);
        UI_DefaultPopup.SetDefaultPopupUI(ArchiveMode ? DefaultPopupState.RandEventArchive : DefaultPopupState.Normal, _eventData.BTN2ResultText, "확인");

        Managers.UI_Manager.ShowPopupUI<UI_DefaultPopup>();
        Managers.Sound.Play(Define.Sound.SmallBTN);

    }


    void DoOption(bool isOption1)
    {
        if(!ArchiveMode)
        {
            float[] optionArray;
            if (isOption1) optionArray = _eventData.Option1;
            else
            {
                optionArray = _eventData.Option2;
            }


            //하트 별 변화량
            Managers.Data.PlayerData.NowHeart += optionArray[0];
            Managers.Data.PlayerData.NowStar += optionArray[1];
            //스텟 변화량
            float[] eventStatValues = new float[6];
            for (int i = 0; i < 6; i++)
            {
                eventStatValues[i] = optionArray[i + 2];
            }

            Managers.Data.PlayerData.ChangeStatAndPlayUIAnimation(eventStatValues);
        }
    }

    void UpdateRedDot()
    {
        if (ArchiveMode && Managers.Data.PersistentUser.WatchedRandEvent.ContainsKey(_eventData.eventName))
        {
            Managers.Data.PersistentUser.WatchedRandEvent[_eventData.eventName] = true;
            Managers.Data.SavePersistentData();
        }
        //레드닷 업데이트 파트
        if (UI_ArchiveList.instance != null)
        {
            UI_ArchiveList.instance.SetListByState();
        }

        if (UI_Archive.instance != null)
        {
            UI_Archive.instance.UpdateRedDot();
        }

        if (UI_MainBackUI.instance != null)
        {
            UI_MainBackUI.instance.UpdateReddot();
        }
    }

    void CloseIfArchive()
    {
        if(ArchiveMode)
        {
            Managers.UI_Manager.ClosePopupUI();
        }
    }

    private void OnDisable()
    {
        ArchiveMode = false;
    }
}
