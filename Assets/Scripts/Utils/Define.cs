using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        MM,
        ScheduleBTN,BigBTN, SmallBTN,
        Effect,
        MaxCount,
    }
    public enum UIEvent
    {
        Click,
        Drag,
        
    }
    public enum MouseEvent{
        Press,
        Click,
    }
    public enum CameraMode{
        QuaterView,
    }

    public enum StatName {
        Game, Song, Draw, Strength, Mental, Luck, FALSE, Subs, Week
    }

    public enum EventDataType {
        Main, Random, Conditioned
    }

    public struct Item
    {
        public string ItemName;
        public int Cost;
        public float[] SixStats;
        public string ItemImageName;
        public int EntWeek;

        public Item(string itemName = "", int cost = 0, string itemImageName = "", int entWeek = 0)
            : this(itemName, cost, itemImageName, entWeek, new float[6])
        {
        }

        public Item(string itemName, int cost, string itemImageName, int entWeek, float[] sixStats)
        {
            ItemName = itemName;
            Cost = cost;
            SixStats = sixStats;
            ItemImageName = itemImageName;
            EntWeek = entWeek;
        }
    }

    public struct Bonus
    {
        public int SubBonus;
        public int IncomeBonus;
    }

    public enum BroadCastType
    {
        Healing, LOL, Horror, Challenge, Sing, PlayInst, Compose, Sketch, Commission, MaxCount_Name
    }

    public static StatName GetStatNameByBroadCastType(BroadCastType broadCastType)
    {
        StatName temp;

        if (broadCastType == BroadCastType.Healing || broadCastType == BroadCastType.LOL
            || broadCastType == BroadCastType.Horror || broadCastType == BroadCastType.Challenge)
            temp = StatName.Game;
        else if (broadCastType == BroadCastType.Sing || broadCastType == BroadCastType.PlayInst || broadCastType == BroadCastType.Compose)
            temp = StatName.Song;
        else
            temp = StatName.Draw;

        return temp;
    }

    public enum RestType
    {
        hea1, hea2, hea3, men1, men2, men3, MaxCount
    }

    public enum GoOutType
    {
        game1, game2, game3, song1, song2, song3, chat1, chat2, chat3,
        hea1, hea2, hea3, men1, men2, men3, luck1, luck2, luck3,
        MaxCount
    }

    public enum SevenDays
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
    }

    public enum ScheduleType
    {
        Null, BroadCast, Rest, GoOut
    }

    public class OneDayScheduleData
    {
        public string KorName;
        public string infotext;
        public ScheduleType scheduleType;
        public BroadCastType broadcastType;
        public RestType restType;
        public GoOutType goOutType;
        public float FisSubsUpValue;
        public float PerSubsUpValue;
        public float HeartVariance;
        public float StarVariance;
        public float InComeMag;
        public int MoneyCost;
        public float[] Six_Stats;
        public string PathName;

        public OneDayScheduleData()
        {
            KorName = "";
            this.scheduleType = ScheduleType.Null;
            this.broadcastType = BroadCastType.MaxCount_Name;
            this.restType = RestType.MaxCount;
            this.goOutType = GoOutType.MaxCount;
            this.infotext = "";
            FisSubsUpValue = 0;
            PerSubsUpValue = 0;
            HeartVariance = 0;
            StarVariance = 0;
            InComeMag = 0;
            MoneyCost = 0;
            Six_Stats = new float[6];
        }
    }

    public class WeekReceiptData
    {
        public int Subs;
        public int Gold;

        public float[] SixStat;

        public void ResetData()
        {
            Subs = 0;
            for (int i = 0; i < 6; i++)
            {
                SixStat[i] = 0;
            }
        }

        public void FillDatas()
        {
            Subs = Managers.Data._myPlayerData.nowSubCount;
            Gold = Managers.Data._myPlayerData.nowGoldAmount;
            for (int i = 0; i < 7; i++)
            {
                Gold += Managers.Data._SevenDayScheduleDatas[i].MoneyCost;
            }


            SixStat = new float[Managers.Data._myPlayerData.SixStat.Length];
            Array.Copy(Managers.Data._myPlayerData.SixStat, SixStat, SixStat.Length);
        }
    }

    public enum MMState { usual, OnSchedule }

    [System.Serializable]
    public struct Dialogue
    {
        public string name; // 캐릭터 이름
        public string sentence; // 대사
        public bool isLeft;
    }


}
