using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RandEvent
{
    public class Item
    {
        public string EventName;
        public string InfoText;
    }
}

namespace MerchantItem
{
    public class Item
    {
        public string ItemName;
        public int ItemIndex;
        public string ItemInfo;
    }
}


public class Define
{
    public const float ScreenAniSpeed = 0.05555556f;

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
        ScheduleBTN, BigBTN, SmallBTN,
        ReturnBTN, TaskBTN, SubcontentBTN,
        NextWeekBTN,
        BigSuccess,
        Success,
        Fail,
        Buy,
        Receipt,
        Skip,
        knock,
        Chat1, Chat2, Chat3,

        MaxCount,
    }
    public enum UIEvent
    {
        Click,
        Drag,

    }
    public enum MouseEvent
    {
        Press,
        Click,
    }
    public enum CameraMode
    {
        QuaterView,
    }

    #region StatName
    public enum StatName
    {
        Game, Song, Draw, Strength, Mental, Luck, FALSE, Subs, Week, Karma, Heart, Star
    }

    public enum StatNameKor
    {
        °ÔÀÓ, ³ë·¡, ±×¸², ±Ù·Â, ¸àÅ», Çà¿î
    }

    public static string GetStatKorName(StatName statName)
    {
        string temp = "";
        temp += ((StatNameKor)((int)statName)).ToString();
        return temp;
    }
    #endregion

    public enum EndingName
    {
        GameGood, GameEvil, GameFail,
        SongGood, SongEvil, SongFail,
        DrawGood, DrawEvil, DrawFail,
        MaxCount
    }

    public enum SubStoryName
    {
        ¹äÁà_¹÷¹÷,
        ¹æ¼Û,
        È²±Ý_¿Ã¸®ºê,
        ÀÎ»ç,
        ¿Ê,
        ¸»Åõ,
        Å»ºÎÂø,
        ¾ÆÄ§Á¡½ÉÀú³á,
        ¿Üµ¿,
        »¡°£ºÒ,
        ±¸µ¶ÀÚ_¾ÖÄª,
        ²¿¸®,
        ÃâÃ³,
        »Ô,
        ±¸¸®ºû_ÇÇºÎ¿Í±Ý¹ß,
        ’À’À¾î,
        ºñÇÏÀÎµå,
        ÈÞ¹æ,
        ÀÌ°Ô_¹¹¾ß,
        ¹Ý·Á,
        °­¾ÆÁö,
        Äç,
        ¿ä¸®,
        °¡½ººñ,
        ¸¶¹ý,
        °Õ½Å_ÀÓÆÅ¶Ç,
        ¸®±×¿Àºê·¹ÀÜµµ,
        Àº¹ÐÇÑ_ÃëÇâ,
        °øÆ÷°ÔÀÓ,
        Æø¹ß_¾ÖÈ£°¡,
        Max
    }



    public enum EventDataType
    {
        Main, Random, Conditioned
    }

    public struct Item
    {
        public string ItemName;
        public int Cost;
        public float[] SixStats;
        public string ItemImageName;
        public int EntWeek;
        public int Karma;
        public string ItemInfoText;

        public Item(string itemName = "", int cost = 0, string itemImageName = "", int entWeek = 0, int Karma = 0, string IteminfoText = "")
            : this(itemName, cost, itemImageName, entWeek, new float[6], Karma, IteminfoText)
        {
        }

        public Item(string itemName, int cost, string itemImageName, int entWeek, float[] sixStats, int karma, string itemInfo)
        {
            ItemName = itemName;
            Cost = cost;
            SixStats = sixStats;
            ItemImageName = itemImageName;
            EntWeek = entWeek;
            Karma = karma;
            ItemInfoText = itemInfo;
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

    public enum DefaultPopupState
    {
        Normal, Merchant, RandomEvent, RandEventArchive,
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

    public enum MainStory { Game1, Game2, Game3, Game4, Song1, Song2, Song3, Song4, Draw1, Draw2, Draw3, Draw4 }

    public enum ContentType
    {
        Null, BroadCast, Rest, GoOut
    }

    public class OneDayScheduleData
    {
        public string KorName;
        public string infotext;
        public ContentType scheduleType;
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
        public string RubiaAni;
        public string ArchiveInfoText;

        public OneDayScheduleData()
        {
            KorName = "";
            this.scheduleType = ContentType.Null;
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
            RubiaAni = "";
            Six_Stats = new float[6];
        }

        public void CheckAndAddIfNotWatched()
        {
            switch (scheduleType)
            {
                case ContentType.BroadCast:
                    Managers.Data.PersistentUser.CheckAndAddIfNotWatched(broadcastType);
                    break;
                case ContentType.Rest:
                    Managers.Data.PersistentUser.CheckAndAddIfNotWatched(restType);
                    break;
                case ContentType.GoOut:
                    Managers.Data.PersistentUser.CheckAndAddIfNotWatched(goOutType);
                    break;
            }
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
            Subs = Managers.Data.PlayerData.nowSubCount;
            Gold = Managers.Data.PlayerData.nowGoldAmount;
            for (int i = 0; i < 7; i++)
            {
                Gold += Managers.Data._SevenDayScheduleDatas[i].MoneyCost;
            }


            SixStat = new float[Managers.Data.PlayerData.SixStat.Length];
            Array.Copy(Managers.Data.PlayerData.SixStat, SixStat, SixStat.Length);
        }
    }

    public static Color alpha0 = new Color(1, 1, 1, 0);
    public static Color alpha1 = new Color(1, 1, 1, 1);

    public enum MMState { usual, OnSchedule }

    public enum TutorialFocusPoint
    {
        CreateScheduleBTN,
        ScreenIMG,
        BroadcastBTN,
        RestBTN,
        GoOutBTN,
        Healing,
        LOL,
        Sketch,
        Category_Draw,
        TuesdayBTN,
        Category_Song,
        Song,
        MM,
        HealthBox,
        StatBox,
        MaxCount
    }

    public enum RandEventName
    { 
        Mandu,
        TS,
        Jaemmin,
        SilverButton,
        Gosamcha,
        Mukbang,
        Myungdiet,
        Chiropractor,
        skydiving,
        FanLetter,
        Mealworm,
        Drunk,
        danggeun,
        Sajyooo,
        BlackandWhite,
        Battle,
        Controversy,
        Idol,
        MaxCount
    }


    [System.Serializable]
    public class Dialogue
    {
        public string name;
        public string sentence;
        public bool isLeft;
        public int CostGold;
        public List<RewardStat> rewardStats;
        public TutorialFocusPoint tutorialFocus;
        public bool IsInteractable;

        public Dialogue(string name = "", string sentence = "", bool isLeft = false, int costGold = 0)
        {
            this.name = name;
            this.sentence = sentence;
            this.isLeft = isLeft;
            this.CostGold = costGold;
            this.rewardStats = new List<RewardStat>();
        }

        public bool UserHasEnoughGold()
        {
            if (Managers.Data.PlayerData.nowGoldAmount >= CostGold)
                return true;
            return false;
        }

        public bool IsNeedGold()
        {
            return (CostGold > 0);
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
        public List<int> SubStoryIndex;
        public PlayerData()
        {
            NowWeek = 1;
            nowSubCount = 10;
            nowGoldAmount = 1000;
            NowHeart = 100;
            NowStar = 100;
            SixStat = new float[6];
            DoneEventNames = new List<string>();
            BoughtItems = new List<string>();
            SubStoryIndex = new List<int>();
        }

        public void ChangeHeart(float value)
        {
            NowHeart += value;
            NowHeart = Mathf.Clamp(NowHeart, 0, 100);
            UI_MainBackUI.instance.UpdateUItexts();
        }


        public void ChangeStar(float value)
        {
            NowStar += value;
            NowStar = Mathf.Clamp(NowStar, 0, 100);
            UI_MainBackUI.instance.UpdateUItexts();
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

        public void ChangeStatAndPlayAnimation(float[] stats)
        {
            for (int i = 0; i < 6; i++)
            {
                SixStat[i] += stats[i];
                SixStat[i] = Mathf.Clamp(SixStat[i], 0, 200);
                if (stats[i] != 0)
                {
                    UI_MainBackUI.instance.GlitterStat(i);
                    PlusText.Inst.PlayAnimation((StatName)i, (int)stats[i]);
                }
            }
            UI_MainBackUI.instance.UpdateUItexts();
        }

        public void StatUpByDialogue(RewardStat rewardStat)
        {
            if (rewardStat.StatName == StatName.Karma)
            {
                Managers.Data.PlayerData.RubiaKarma += rewardStat.Value;
            }
            else if(rewardStat.StatName == StatName.Heart)
            {
                Managers.Data.PlayerData.ChangeHeart(rewardStat.Value);
                UI_MainBackUI.instance.UpdateUItexts();
            }
            else if(rewardStat.StatName == StatName.Star)
            {
                Managers.Data.PlayerData.ChangeStar(rewardStat.Value);
                UI_MainBackUI.instance.UpdateUItexts();
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
    }

    [Serializable]
    public class PersistentUserData
    {
        public bool BoughtAdPass;
        public bool WatchedTutorial;
        public bool WatchedCaught;
        public bool WatchedRunAway;
        public List<BroadCastType> WatchedBroadCast;
        public List<RestType> WatchedRest;
        public List<GoOutType> WatchedGoOut;
        public List<RandEventName> WatchedRandEvent;
        public List<EndingName> WatchedEndingName;
        public List<bool> OwnedNickNameBoolList;

        public bool CheckAndAddIfNotWatched<T>(T enumValue)
        {
            List<T> watchedList;
            if (typeof(T) == typeof(BroadCastType))
            {
                watchedList = WatchedBroadCast as List<T>;
            }
            else if (typeof(T) == typeof(RestType))
            {
                watchedList = WatchedRest as List<T>;
            }
            else if (typeof(T) == typeof(GoOutType))
            {
                watchedList = WatchedGoOut as List<T>;
            }
            else if(typeof(T)== typeof(RandEventName))
            {
                watchedList = WatchedRandEvent as List<T>;
            }
            else if (typeof(T) == typeof(EndingName))
            {
                watchedList = WatchedEndingName as List<T>;
            }
            else
            {
                throw new ArgumentException("Invalid type");
            }

            bool isWatched = watchedList.Exists(item => item.Equals(enumValue));
            if (!isWatched)
            {
                watchedList.Add(enumValue);
            }
            return isWatched;
        }
    }

    public enum NickNameType
    { prefix, suffix}

    [System.Serializable]
    public class NickName
    {
        public int NicknameIndex;
        public NickNameType NicknameType;
        public string NicknameString;
        public string ConditionString;
        public int GameStat;
        public int SongStat;
        public int DrawStat;
        public int StrStat;
        public int MenStat;
        public int LuckStat;
        public int SubCount;
        public int MoneyValue;

        public int[] GetSixStat()
        {
            int[] statArray = new int[6];
            statArray[0] = GameStat;
            statArray[1] = SongStat;
            statArray[2] = DrawStat;
            statArray[3] = StrStat;
            statArray[4] = MenStat;
            statArray[5] = LuckStat;

            return statArray;
        }
    }

}