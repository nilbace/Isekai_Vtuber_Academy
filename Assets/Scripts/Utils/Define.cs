using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        게임, 노래, 그림, 근력, 멘탈, 행운
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
        밥줘_벅벅,
        방송,
        황금_올리브,
        인사,
        옷,
        말투,
        탈부착,
        아침점심저녁,
        외동,
        빨간불,
        구독자_애칭,
        꼬리,
        출처,
        뿔,
        구리빛_피부와금발,
        뮹뮹어,
        비하인드,
        휴방,
        이게_뭐야,
        반려,
        강아지,
        쾅,
        요리,
        가스비,
        마법,
        겐신_임팍또,
        리그오브레잔도,
        은밀한_취향,
        공포게임,
        폭발_애호가,
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
            if (Managers.Data.PersistentUser.WatchedBroadCast.Count >= 9) Managers.NickName.OpenNickname(NickNameKor.카멜레온);
            Managers.NickName.CheckPerfectNickName();
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
        BaseGame,
        BaseSong,
        BaseDraw,
        StartScheduleBTN,
        RecieptBackGroundIMG,
        FinishBTN,
        SettingBTN,
        StartResetBTN,
        ResetBTN,
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


    public enum StatIcons { Game, Song, Draw, Strength, Mental, Luck, Sub, Gold, BigSuccess, Success, Fail, Heart, Star, Ruby }
    public static string GetIconString(StatIcons stat)
    {
        string temp = $"<sprite={(int)stat}>";
        return temp;
    }

    public static string GetIconString(int index)
    {
        string temp = $"<sprite={index}>";
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
        public string NowNickName;
        public PlayerData()
        {
            NowWeek = 1;
            nowSubCount = 10;
            nowGoldAmount = 5000;
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
            UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
        }


        public void ChangeStar(float value)
        {
            NowStar += value;
            NowStar = Mathf.Clamp(NowStar, 0, 100);
            UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
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
            UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
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
                UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
            }
            else if(rewardStat.StatName == StatName.Star)
            {
                Managers.Data.PlayerData.ChangeStar(rewardStat.Value);
                UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
            }
            else
            {
                ChangeStat(rewardStat.StatName, rewardStat.Value);
            }
        }

        public void ChangeStat(StatName statName, float value)
        {
            if ((int)statName >= 6) Debug.LogError("6스텟 아님!");
            int index = (int)statName;

            SixStat[index] += value;
            SixStat[index] = Mathf.Clamp(SixStat[index], 0, 200);

            if (value != 0)
            {
                UI_MainBackUI.instance.GlitterStat(index);
            }
            UI_MainBackUI.instance.UpdateUItextsAndCheckNickname();
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
        public int ResetCount;
        public int BigSuccessCount;
        public int MMCount;
        public int BroadcastCount;
        public int ColdCount;

        public PersistentUserData()
        {
            BoughtAdPass = false;
            WatchedTutorial = false;
            WatchedCaught = false;
            WatchedRunAway = false;
            WatchedBroadCast = new List<BroadCastType>();
            WatchedRest = new List<RestType>();
            WatchedGoOut = new List<GoOutType>();
            WatchedRandEvent = new List<RandEventName>();
            WatchedEndingName = new List<EndingName>();
            OwnedNickNameBoolList = new List<bool>();
            for (int i = 0; i < 42; i++)
            {
                OwnedNickNameBoolList.Add(false);
            }
            ResetCount = 0;
            BigSuccessCount = 0;
            MMCount = 0;
        }

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

        public void InCreaseResetCount()
        {
            ResetCount++;
            if(ResetCount == 1)
            {
                Managers.NickName.OpenNickname(NickNameKor.익숙한);
            }
            if (ResetCount == 50)
            {
                Managers.NickName.OpenNickname(NickNameKor.시공간의);
            }
            if (ResetCount == 100)
            {
                Managers.NickName.OpenNickname(NickNameKor.회귀자);
            }
        }

        public void IncreaseBigSuccessCount()
        {
            BigSuccessCount++;
            if (BigSuccessCount == 50)
            {
                Managers.NickName.OpenNickname(NickNameKor.천재);
            }
        }

        public void IncreaseMMCount()
        {
            MMCount++;
            if (MMCount >= 100)
            {
                Managers.NickName.OpenNickname(NickNameKor.뮹뮹이의);
            }
        }

        public void IncreaseColdCount()
        {
            ColdCount++;
            if (ColdCount >= 10)
            {
                Managers.NickName.OpenNickname(NickNameKor.환자);
            }
        }

        public void IncreaseBCCount()
        {
            BroadcastCount++;
            if (BroadcastCount >= 200)
            {
                Managers.NickName.OpenNickname(NickNameKor.노예);
            }
        }
    }

    public enum NickNameType
    { prefix, suffix }

    public enum NickNameKor
    {
        대담한,
        불멸의,
        빛나는,
        파릇파릇한,
        발칙한,
        버릇_없는,
        농염한,
        사랑에_빠진,
        숨겨진,
        집요한,
        익숙한,
        시공간의,
        천재,
        작고_소중한,
        사랑스러운,
        이세계,
        떠오르는,
        행운의,
        단단한,
        평온한,
        뮹뮹이의,
        초보,
        탑라이너,
        아이돌,
        새내기,
        여왕,
        쓰레기,
        붉은_머리,
        소녀,
        보석,
        지배자,
        필멸자,
        노예,
        환자,
        부르주아,
        드래곤,
        버튜버,
        회귀자,
        완벽주의자,
        흑마법사,
        카멜레온,
        구미호,
        MaxCount
    }


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
        public int MoneyValue;
        public int SubCount;


        public NickName()
        {
            NicknameIndex = -1;
            NicknameType = NickNameType.suffix;
            NicknameString = "";
            ConditionString = "";
            GameStat = 0;
            SongStat = 0;
            DrawStat = 0;
            StrStat = 0;
            MenStat = 0;
            LuckStat = 0;
            SubCount = 0;
            MoneyValue = 0;
        }

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

