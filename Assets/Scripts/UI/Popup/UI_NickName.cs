using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_NickName : UI_Popup
{
    public Transform PrefixParent;
    UI_NickSubContent[] PrefixGOs;
    public Transform SuffixParent;
    UI_NickSubContent[] SuffixGOs;
    enum Buttons
    {
        CloseBTN
    }


    private void Start()
    {
        Init();
    }

    

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.CloseBTN).onClick.AddListener(CloseBTN);
        PrefixGOs = PrefixParent.GetComponentsInChildren<UI_NickSubContent>();
        SuffixGOs = SuffixParent.GetComponentsInChildren<UI_NickSubContent>();
        CheckOwnedNickName();
    }
    
    public void CheckOwnedNickName()
    {
        var OwnedCheckList = Managers.Data.PersistentUser.OwnedNickNameBoolList;
        var NickNameList = DataParser.Inst.NickNameList;
        int prefixIndex = 0;
        int suffixIndex = 0;

        // OwnedCheckList를 NickNameList와 동일한 길이로 맞춰줌
        while (OwnedCheckList.Count < NickNameList.Count)
        {
            OwnedCheckList.Add(false);
        }

        for (int i = 0; i < NickNameList.Count; i++)
        {
            if (NickNameList[i].NicknameType == NickNameType.prefix)
            {
                if (prefixIndex < PrefixGOs.Length)
                {
                    PrefixGOs[prefixIndex].Setting(NickNameList[i], OwnedCheckList[i]);
                    prefixIndex++;
                }
            }
            else if (NickNameList[i].NicknameType == NickNameType.suffix)
            {
                if (suffixIndex < SuffixGOs.Length)
                {
                    SuffixGOs[suffixIndex].Setting(NickNameList[i], OwnedCheckList[i]);
                    suffixIndex++;
                }
            }
        }

        //나머지 비활성화
        for (int j = prefixIndex; j < PrefixGOs.Length; j++)
        {
            PrefixGOs[j].gameObject.SetActive(false);
        }

        for (int j = suffixIndex; j < SuffixGOs.Length; j++)
        {
            SuffixGOs[j].gameObject.SetActive(false);
        }

        Managers.Data.SaveData();
    }
}
