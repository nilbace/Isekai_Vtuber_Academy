using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ConfirmReset : UI_Popup
{
    public Sprite[] EndingIMGs;
    public EasyTransition.TransitionSettings transition;


    enum Buttons
    {
        StartResetBTN
    }
   

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.StartResetBTN).onClick.AddListener(ResetBTN);
    }

    void ResetBTN()
    {
        StartCoroutine(ResetGame());
        Managers.Sound.Play(Define.Sound.ReturnBTN);
    }

    IEnumerator ResetGame()
    {
        
        if(UI_Tutorial.instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
        EasyTransition.TransitionManager.Instance().Transition(transition, 0);
        yield return new WaitForSeconds(1f);

        if (UI_Tutorial.instance == null)
        {
            Managers.Data.PersistentUser.ResetCount++;
        }
        UI_MainBackUI.instance.ChangeUnderRedDotState(true);
        UI_MainBackUI.instance.StartScreenAnimation("WaitingArea");
        Managers.UI_Manager.CloseALlPopupUI();
        Managers.Data.PlayerData = new Define.PlayerData();
        Managers.Data.SaveData();
        UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
        Managers.UI_Manager.ShowPopupUI<UI_BeforeSelectNickName>();
    }


}
