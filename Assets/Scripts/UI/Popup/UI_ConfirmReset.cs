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
        ResetBTN
    }
   

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.ResetBTN).onClick.AddListener(ResetBTN);
    }

    void ResetBTN()
    {
        StartCoroutine(ResetGame());
        Managers.Sound.Play(Define.Sound.ReturnBTN);
    }

    IEnumerator ResetGame()
    {
        EasyTransition.TransitionManager.Instance().Transition(transition, 0);
        yield return new WaitForSeconds(1f);


        Managers.UI_Manager.CloseALlPopupUI();
        Managers.Data.PlayerData = new Define.PlayerData();
        Managers.Data.SaveData();
        UI_MainBackUI.instance.UpdateUItexts();
    }


}
