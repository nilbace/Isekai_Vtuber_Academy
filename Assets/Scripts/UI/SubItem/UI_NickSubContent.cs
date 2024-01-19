using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_NickSubContent : MonoBehaviour
{
    Button thisButton;
    TMPro.TMP_Text NameTMP;
    void Awake()
    {
        thisButton = GetComponent<Button>();
        NameTMP = GetComponentInChildren<TMPro.TMP_Text>();
    }

    void Update()
    {
        
    }

    public void Setting(NickName nickName, bool isOwned)
    {
        NameTMP.text = nickName.NicknameString;
        if(!isOwned)
        {
            thisButton.interactable = false;
        }
    }
}
