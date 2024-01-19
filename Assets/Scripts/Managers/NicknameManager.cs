using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class NicknameManager
{
    void OpenNickname(int n)
    {
        Managers.Data.PersistentUser.OwnedNickNameBoolList[n] = true;
        Managers.Data.SaveData();
    }
    public void OpenNickname(EndingName ending)
    {

    }
}
