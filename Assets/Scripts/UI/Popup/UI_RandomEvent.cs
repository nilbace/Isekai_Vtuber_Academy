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
    WeekEventData _eventData;
    public Sprite[] CutsceneSprites;
    enum Buttons
    {
        ResultBTN, ResultBTN2
    }
    enum Texts
    {
        EventText,
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
        _eventData = Managers.RandEvent.GetProperEvent();
        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<TMPro.TMP_Text>(typeof(Texts));

        GetImage((int)Images.CutSceneIMG).sprite = LoadIMG(_eventData);

        GetText((int)Texts.EventText).text = _eventData.EventInfoString;
        GetButton((int)Buttons.ResultBTN).onClick.AddListener(ChooseBTN1);
        GetButton((int)Buttons.ResultBTN2).onClick.AddListener(ChooseBTN2);
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
        UI_DefaultPopup.SetDefaultPopupUI( DefaultPopupState.Normal, _eventData.BTN1ResultText, "1번");
        Managers.UI_Manager.ShowPopupUI<UI_DefaultPopup>();
        Managers.Sound.Play(Define.Sound.SmallBTN);

    }

    void ChooseBTN2()
    {
        DoOption(isOption1: false);
        UI_DefaultPopup.SetDefaultPopupUI(DefaultPopupState.Normal, _eventData.BTN2ResultText, "2번");
        Managers.UI_Manager.ShowPopupUI<UI_DefaultPopup>();
        Managers.Sound.Play(Define.Sound.SmallBTN);

    }


    void DoOption(bool isOption1)
    {
        float[] optionArray;
        if (isOption1) optionArray = _eventData.Option1;
        else
        {
            optionArray = _eventData.Option2;
        }
        

        //하트 별 변화량
        Managers.Data.PlayerData.ChangeHeart(optionArray[1]);
        //스텟 변화량
        float[] eventStatValues = new float[6];
        for(int i = 0; i<6;i++)
        {
            eventStatValues[i] = optionArray[i + 2];
        }

        Managers.Data.PlayerData.ChangeStatAndPlayAnimation(eventStatValues);
    }
}
