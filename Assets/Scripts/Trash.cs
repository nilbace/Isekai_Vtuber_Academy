using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Trash : MonoBehaviour
{
    public TMPro.TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = $"{trash(Managers.Data.GetOneDayDataByName(BroadCastType.Sing))}\n" +
            $"{trash(Managers.Data.GetOneDayDataByName(BroadCastType.PlayInst))}\n" +
            $"{trash(Managers.Data.GetOneDayDataByName(BroadCastType.Compose))}";
    }

    string trash(OneDayScheduleData one)
    {
        float heart = one.HeartVariance * ScheduleExecuter.Inst.GetSubStatProperty(StatName.Heart);
        float gold = one.InComeMag * Mathf.Log10(Managers.Data.PlayerData.nowSubCount);
        return (-gold*100 / heart).ToString("F3");
    }
}
