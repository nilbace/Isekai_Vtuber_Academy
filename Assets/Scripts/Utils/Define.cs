using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
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
        Bgm, Effect,
        MM,
        ScheduleBTN,BigBTN, SmallBTN,
        ReturnBTN, TaskBTN, SubcontentBTN,
        NextWeekBTN,
        BigSuccess,
        Success,
        Fail,
        Buy,
        Receipt,
        Chat1, Chat2, Chat3,

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
        Game, Song, Draw, Strength, Mental, Luck, FALSE, Subs, Week, Karma
    }

    public enum StatNameKor { 
        °ÔÀÓ, ³ë·¡, ±×¸², ±Ù·Â, ¸àÅ», Çà¿î
    }

    public enum EndingName
    {
        GameGood,GameEvil,GameFail,
        SongGood,SongEvil,SongFail,
        DrawGood,DrawEvil,DrawFail
    }

    public static string GetStatKorName(StatName statName)
    {
        string temp = "";
        temp += ((StatNameKor)((int)statName)).ToString();
        return temp;
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

    public enum MainStory { Game1, Game2, Game3, Game4, Song1, Song2, Song3, Song4, Draw1, Draw2, Draw3, Draw4}

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

    public static Color alpha0 = new Color(1, 1, 1, 0);
    public static Color alpha1 = new Color(1, 1, 1, 1);

    public enum MMState { usual, OnSchedule }

    [System.Serializable]
    public class Dialogue
    {
        public string name;
        public string sentence;
        public bool isLeft;
        public int CostGold;
        public List<RewardStat> rewardStats;

        public Dialogue(string name = "", string sentence = "", bool isLeft = false, int costGold = 0)
        {
            this.name = name;
            this.sentence = sentence;
            this.isLeft = isLeft;
            this.CostGold = costGold;
            this.rewardStats = new List<RewardStat>();
        }
    }

    [System.Serializable]
    public struct RewardStat
    {
        public StatName StatName { get; set; }
        public int Value { get; set; }

        public RewardStat(StatName name = StatName.FALSE, int Value = 0)
        {
            this.StatName = name;
            this.Value = 0;
        }
    }

    public static string GetIconString(StatName stat)
    {
        string temp = $"  <sprite={(int)stat}>";
        return temp;
    }

    public enum StatIcons { Game, Song, Draw, Strength, Mental, Luck, Sub, Gold, BigSuccess, Success, Fail, Heart, Star, Ruby }
    public static string GetIconString(StatIcons stat)
    {
        string temp = $"  <sprite={(int)stat}>";
        return temp;
    }

    [System.Serializable]
    public class PlayerData
    {
        public int NowWeek;
        public int nowSubCount;
        public int nowGoldAmount;
        public int RubiaKarma;
        public float NowHeart;
        public float NowStar;
        public float[] SixStat;
        public List<string> DoneEventNames;
        public List<string> BoughtItems;

        public PlayerData()
        {
            NowWeek = 1;
            nowSubCount = 0;
#if UNITY_EDITOR
            nowGoldAmount = 1000000;
#else
        nowGoldAmount = 0;
#endif
            NowHeart = 100;
            NowStar = 100;
            SixStat = new float[6];
            DoneEventNames = new List<string>();
            BoughtItems = new List<string>();

        }

        public void ChangeHeart(float value)
        {
            NowHeart += value;
            NowHeart = Mathf.Clamp(NowHeart, 0, 100);
        }


        public void ChangeStar(float value)
        {
            NowStar += value;
            NowStar = Mathf.Clamp(NowStar, 0, 100);
        }

        public Define.StatName GetHigestStatName()
        {
            Define.StatName temp = Define.StatName.Game;
            float temp2 = 0;

            for (int i = 0; i < 3; i++)
            {
                if (temp2 < SixStat[i])
                {
                    temp2 = SixStat[i];
                    temp = (Define.StatName)i;
                }
            }
            return temp;
        }

        public void ChangeStat(float[] stats)
        {
            for (int i = 0; i < 6; i++)
            {
                SixStat[i] += stats[i];
                SixStat[i] = Mathf.Clamp(SixStat[i], 0, 200);
                if (stats[i] != 0)
                {
                    UI_MainBackUI.instance.GlitterStat(i);
                }
            }
            UI_MainBackUI.instance.UpdateUItexts();
        }

        public void StatUpByDialogue(RewardStat rewardStat)
        {
            if(rewardStat.StatName == StatName.Karma)
            {
                Managers.Data._myPlayerData.RubiaKarma += rewardStat.Value;
            }
            else
            {
                ChangeStat(rewardStat.StatName, rewardStat.Value);
            }
        }

        public void ChangeStat(StatName statName, float value)
        {
            if ((int)statName >= 6) Debug.LogError("6½ºÅÝ ¾Æ´Ô!");
            int index = (int)statName; 

            SixStat[index] += value;
            SixStat[index] = Mathf.Clamp(SixStat[index], 0, 200);

            if (value != 0)
            {
                UI_MainBackUI.instance.GlitterStat(index);
            }
            UI_MainBackUI.instance.UpdateUItexts();
        }

        public bool MerchantAppearanceWeek()
        {
            if (NowWeek == 5 || NowWeek == 10 || NowWeek == 15) return true;
            return false;
        }

        public bool MainStoryApperanceWeek()
        {
            if (NowWeek == 4 || NowWeek == 8 || NowWeek == 12 || NowWeek == 16) return true;
            return false;
        }

        public void ResetData()
        {
            NowWeek = 1;
            nowSubCount = 0;
#if UNITY_EDITOR
            nowGoldAmount = 1000000;
#else
        nowGoldAmount = 0;
#endif
            NowHeart = 100;
            NowStar = 100;
            SixStat = new float[6];
            DoneEventNames = new List<string>();
            BoughtItems = new List<string>();
        }

    }
}
