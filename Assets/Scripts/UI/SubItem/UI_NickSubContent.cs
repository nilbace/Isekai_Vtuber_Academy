using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;
using UnityEngine.EventSystems;

public class UI_NickSubContent : MonoBehaviour, IPointerClickHandler
{
    Button thisButton;
    TMPro.TMP_Text NameTMP;
    public NickName ThisNickName;
    public Sprite NormalSprite;
    public Sprite SelectedSprite;
    public Image Reddot;

    void Awake()
    {
        thisButton = GetComponent<Button>();
        NameTMP = GetComponentInChildren<TMPro.TMP_Text>();
    }

    void SetRedDot()
    {
        bool isKeyExists = Managers.Data.PersistentUser.OwnedNickname.ContainsKey((NickNameKor)ThisNickName.NicknameIndex);
        bool shouldSetActive = isKeyExists && Managers.Data.PersistentUser.OwnedNickname[(NickNameKor)ThisNickName.NicknameIndex];

        if (!isKeyExists || shouldSetActive)
        {
            Reddot.gameObject.SetActive(false);
        }

    }


    public void SetFrameImage()
    {
        var Image = GetComponent<Image>();
        if(UI_SelectNickName.instance.SelectedPrefix.NicknameIndex == ThisNickName.NicknameIndex || UI_SelectNickName.instance.SelectedSuffix.NicknameIndex == ThisNickName.NicknameIndex)
        {
            Image.sprite = SelectedSprite;
        }
        else
        {
            Image.sprite = NormalSprite;
        }
        SetRedDot();
    }

   

    public void Setting(NickName nickName, bool isOwned)
    {
        ThisNickName = nickName;
        thisButton.onClick.RemoveAllListeners();
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
        SetRedDot();
    }

    public void SetForSelectNickName(NickName nickName, bool isOwned)
    {
        ThisNickName = nickName;
        thisButton.onClick.RemoveAllListeners();
        if (!isOwned)
        {
            thisButton.GetComponentInChildren<Image>().color = thisButton.colors.disabledColor;
            thisButton.onClick.AddListener(() => Alarm.ShowAlarm("¹ÌÈ¹µæ ÄªÈ£ÀÔ´Ï´Ù."));
            NameTMP.text = "???";
        }
        else
        {
            NameTMP.text = nickName.NicknameString;
            thisButton.onClick.AddListener(() => UI_SelectNickName.instance.SelectNickName(ThisNickName));
        }
        SetRedDot();
    }

    void ShowNicknamePopup(NickName nickName)
    {
        Managers.UI_Manager.ShowPopupUI<UI_NicknamePopup>();
        UI_NicknamePopup.nickName = nickName;
        Managers.Sound.Play(Sound.SmallBTN);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool OwnThisNickname = Managers.Data.PersistentUser.OwnedNickname.ContainsKey(((NickNameKor)ThisNickName.NicknameIndex));
        if (ThisNickName.NicknameIndex != -1 && OwnThisNickname)
        {
            Managers.Data.PersistentUser.OwnedNickname[(NickNameKor)ThisNickName.NicknameIndex] = true;
            Managers.Data.SavePersistentData();
            if (UI_SelectNickName.instance != null) UI_SelectNickName.instance.CheckOwnedNickName();
            if (UI_NickName.instance != null) UI_NickName.instance.CheckOwnedNickName();
        }
    }
}
