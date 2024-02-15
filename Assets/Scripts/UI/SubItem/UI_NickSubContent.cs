using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_NickSubContent : MonoBehaviour
{
    Button thisButton;
    TMPro.TMP_Text NameTMP;
    public NickName ThisNickName;
    void Awake()
    {
        thisButton = GetComponent<Button>();
        NameTMP = GetComponentInChildren<TMPro.TMP_Text>();
    }

    public void Setting(NickName nickName, bool isOwned)
    {
        if(!isOwned)
        {
            thisButton.GetComponentInChildren<Image>().color = thisButton.colors.disabledColor;
            thisButton.onClick.AddListener( () => Alarm.ShowAlarm("¹ÌÈ¹µæ ÄªÈ£ÀÔ´Ï´Ù."));
            NameTMP.text = "???";
        }
        else
        {
            NameTMP.text = nickName.NicknameString;
            thisButton.onClick.AddListener(() => ShowNicknamePopup(nickName));
        }
    }

    public void SetForSelectNickName(NickName nickName, bool isOwned)
    {
        ThisNickName = nickName;
        if (!isOwned)
        {
            thisButton.GetComponentInChildren<Image>().color = thisButton.colors.disabledColor;
            thisButton.onClick.AddListener(() => Alarm.ShowAlarm("¹ÌÈ¹µæ ÄªÈ£ÀÔ´Ï´Ù."));
            NameTMP.text = "???";
        }
        else
        {
            NameTMP.text = nickName.NicknameString;
            thisButton.onClick.AddListener(() => UI_SelectNickName.instance.SetInfoTexts(ThisNickName));
        }
    }

    void ShowNicknamePopup(NickName nickName)
    {
        Managers.UI_Manager.ShowPopupUI<UI_NicknamePopup>();
        UI_NicknamePopup.nickName = nickName;
        Managers.Sound.Play(Sound.SmallBTN);
    }
}
