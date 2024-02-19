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
        OpenNicknameWithoutAlarm(0);
        OpenNicknameWithoutAlarm(21);
    }

    /// <summary>
    /// 어떤 칭호를 해금했는지 알람과 보여줌
    /// </summary>
    /// <param name="n"></param>
    public void OpenNickname(int n)
    {
        if(Managers.Data.PersistentUser.OwnedNickNameBoolList[n] == false)
        {
            Managers.Data.PersistentUser.OwnedNickNameBoolList[n] = true;
            NickName temp = DataParser.Inst.NickNameList[n];
            Alarm.ShowAlarm($"칭호 '{temp.NicknameString}'을 획득하였습니다.");
        }
    }

    public void CheckPerfectNickName()
    {
        if (Managers.Data.PersistentUser.WatchedEndingName.Count == (int)EndingName.MaxCount &&
            Managers.Data.PersistentUser.WatchedRandEvent.Count == 18&&
            Managers.Data.PersistentUser.WatchedBroadCast.Count == (int)BroadCastType.MaxCount_Name)
            OpenNickname(NickNameKor.완벽주의자);
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
}
