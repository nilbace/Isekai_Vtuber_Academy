using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Define;

public class UI_Ending : UI_Popup
{
    public EasyTransition.TransitionSettings Endingtransition;
    public EasyTransition.TransitionSettings Resettransition;
    public Sprite[] EndingIMGs;
    public TextAsset[] EndingAssets;
    public static EndingName EndingName;
    public static bool ArchiveMode;

    enum Buttons
    {
        NextBTN,
        FinishBTN
    }
    enum Texts
    {
        EndingNameTMP,
        EndingIndexTMP,
        EndingTextTMP
    }

    enum Images {
        EndingIMG
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TMPro.TMP_Text>(typeof(Texts));

        GetButton((int)Buttons.FinishBTN).onClick.AddListener(ResetBTN);

        StartCoroutine(StartTransition());
    }

    IEnumerator StartTransition()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        EasyTransition.TransitionManager.Instance().Transition(Endingtransition, 0);

        yield return new WaitForSeconds(1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        GetImage((int)Images.EndingIMG).sprite = EndingIMGs[(int)GetValidEndingName()];

    }

    EndingName GetValidEndingName()
    {
        int temp = 0;
        if (Managers.Data.PlayerData.RubiaKarma >= 3) temp += 0;
        else if (Managers.Data.PlayerData.RubiaKarma <= -3) temp += 1;
        else temp += 2;

        if (Managers.Data.PlayerData.GetHigestStatName() == StatName.Song) temp += 3;
        else if (Managers.Data.PlayerData.GetHigestStatName() == StatName.Draw) temp += 6;

        Managers.NickName.OpenNickname(temp + 1);
        Managers.NickName.OpenNickname(temp + 22);
        Managers.NickName.CheckPerfectNickName();
        return (EndingName)temp;
    }

    void ResetBTN()
    {
        StartCoroutine(ResetGame());
        Managers.Sound.Play(Define.Sound.ReturnBTN);
    }

    IEnumerator ResetGame()
    {

        if (UI_Tutorial.instance == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }
        EasyTransition.TransitionManager.Instance().Transition(Resettransition, 0);
        yield return new WaitForSeconds(1f);

        if (UI_Tutorial.instance == null)
        {
            Managers.Data.PersistentUser.InCreaseResetCount();
        }

        Managers.UI_Manager.CloseALlPopupUI();
        Managers.Data.PlayerData = new Define.PlayerData();
        Managers.Data.SaveData();
        UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
        Managers.UI_Manager.ShowPopupUI<UI_BeforeSelectNickName>();
    }

    private void OnDisable()
    {
        ArchiveMode = false;
    }
}
