using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using System;

//추가 예정
public class NicknameManager
{
    /// <summary>
    /// 칭호에 처음 접근할 때 사용되는 함수
    /// </summary>
    public void OpenBaseNickName()
    {
        var OwnedCheckList = Managers.Data.PersistentUser.OwnedNickNameBoolList;
        var NickNameList = DataParser.Inst.NickNameList;
        while (OwnedCheckList.Count < NickNameList.Count)
        {
            OwnedCheckList.Add(false);
        }
        OpenNicknameWithoutAlarm(0);
        OpenNicknameWithoutAlarm(21);
    }
    public void OpenNickname(int n)
    {
        if(Managers.Data.PersistentUser.OwnedNickNameBoolList[n] == false)
        {
            Managers.Data.PersistentUser.OwnedNickNameBoolList[n] = true;
            NickName temp = DataParser.Inst.NickNameList[n];
            Alarm.ShowAlarm($"칭호 {temp.NicknameString}을 획득하였습니다.");
        }
    }

    public void OpenNicknameWithoutAlarm(int n)
    {
        if (Managers.Data.PersistentUser.OwnedNickNameBoolList[n] == false)
        {
            Managers.Data.PersistentUser.OwnedNickNameBoolList[n] = true;
            NickName temp = DataParser.Inst.NickNameList[n];
        }
    }

    public void OpenNickname(NickNameKor kor)
    {
        OpenNickname((int)kor);
    }


    public Action UnlockNicknameIfConditionsMetAction;

    public void UnlockNicknameIfConditionsMet()
    {
        UnlockNicknameIfConditionsMetAction?.Invoke();
    }
}
