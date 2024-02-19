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
    public string[] EndingNameStrings;
    public static EndingName Static_EndingName;
    public static bool ArchiveMode;

    enum Buttons
    {
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

        if (!ArchiveMode) Static_EndingName = GetValidEndingName();

        Setting();
    }

    //제목, 이미지, 텍스트 채워넣기
    void Setting()
    {
        GetText((int)Texts.EndingIndexTMP).text = $"Ending. 0{(int)Static_EndingName+1}";
        GetText((int)Texts.EndingNameTMP).text = EndingNameStrings[(int)Static_EndingName];
        GetImage((int)Images.EndingIMG).sprite = EndingIMGs[(int)Static_EndingName];
        GetText((int)Texts.EndingTextTMP).text = EndingAssets[(int)Static_EndingName].text;
    }

    EndingName GetValidEndingName()
    {
        var data = Managers.Data.PlayerData;
        int temp = 0;
        if (data.RubiaKarma >= 3 && data.nowSubCount >=1000000 && data.GetHigestMainStatValue()>=200) temp += 0;
        else if (data.RubiaKarma <= -3 && data.nowSubCount >= 1000000 && data.GetHigestMainStatValue() >= 200) temp += 1;
        else temp += 2;

        if (Managers.Data.PlayerData.GetHigestMainStatName() == StatName.Song) temp += 3;
        else if (Managers.Data.PlayerData.GetHigestMainStatName() == StatName.Draw) temp += 6;

        Managers.NickName.OpenNickname(temp + 1);
        Managers.NickName.OpenNickname(temp + 22);
        if (Managers.Data.PlayerData.RubiaKarma >= 6) Managers.NickName.OpenNickname(NickNameKor.드래곤);
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

        yield return new WaitForSeconds(0.1f);
        EasyTransition.TransitionManager.Instance().Transition(Resettransition, 0);
        yield return new WaitForSeconds(1f);

        Managers.Data.PersistentUser.InCreaseResetCount();
        Managers.UI_Manager.CloseALlPopupUI();
        Managers.Data.PlayerData = new PlayerData();
        Managers.Data.SaveData();
        UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
        Managers.UI_Manager.ShowPopupUI<UI_BeforeSelectNickName>();
    }

    private void OnDisable()
    {
        ArchiveMode = false;
    }
}
