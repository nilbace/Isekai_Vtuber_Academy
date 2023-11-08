using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static Define;

/// <summary>
/// 데이터 읽어오기 및 가공 담당 스크립트
/// </summary>
public class DataManager
{ 
    public PlayerData _myPlayerData;
    float[] weekBounsMagnification = { 1.2f, 1f, 1f, 1f, 1f };
    int[] MonthlyExpense = {1000, 3000, 5000, 10000, 10000 };


    public void Init()
    {
        LoadData();
    }

    #region Part_DataSave&Load

    void LoadData()
    {
        _myPlayerData = new PlayerData();

        //기존 코드

        //string path;
        //if (Application.platform == RuntimePlatform.Android)
        //{
        //    path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        //}
        //else
        //{
        //    path = Path.Combine(Application.dataPath, "PlayerData.json");
        //}

        //if (!File.Exists(path))
        //{
        //    _myPlayerData = new PlayerData();
        //    SaveData();
        //}

        //FileStream fileStream = new FileStream(path, FileMode.Open);
        //byte[] data = new byte[fileStream.Length];
        //fileStream.Read(data, 0, data.Length);
        //fileStream.Close();
        //string jsonData = Encoding.UTF8.GetString(data);

        //_myPlayerData = JsonUtility.FromJson<PlayerData>(jsonData);
    }

    public void SaveData()
    {
        string path;
        if (Application.platform == RuntimePlatform.Android)
        {
            path = Path.Combine(Application.persistentDataPath, "PlayerData.json");
        }
        else
        {
            path = Path.Combine(Application.dataPath, "PlayerData.json");
        }
        string jsonData = JsonUtility.ToJson(_myPlayerData, true);

        FileStream fileStream = new FileStream(path, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public void SaveToCloud()
    {
        SaveData();
    }

    public void LoadFromCloud()
    {

    }

    //랜덤 이벤트 데이터 불러오기

    public IEnumerator RequestAndSetRandEventDatas(string www)
    {
        UnityWebRequest wwww = UnityWebRequest.Get(www);
        yield return wwww.SendWebRequest();

        string data = wwww.downloadHandler.text;
        string[] lines = data.Substring(0, data.Length - 1).Split('\n');

        foreach (string temp in lines)
        {
            Managers.RandEvent.ProcessData(temp);
        }
    }

    #endregion


    #region ScheduleData

    //Schedule Popup데이터 관리용
    public OneDayScheduleData[] _SevenDayScheduleDatas = new OneDayScheduleData[7];
    public float[] _SeveDayScrollVarValue = new float[7];


    public float GetNowWeekBonusMag()
    {
        int temp = _myPlayerData.NowWeek;
        int temp2 = (temp-1) / 4;
        return weekBounsMagnification[temp2];
    }

    public int GetNowMonthExpense()
    {
        int temp = _myPlayerData.NowWeek;
        int temp2 = ((temp) / 4) - 1;
        return MonthlyExpense[temp2];
    }

    List<OneDayScheduleData> oneDayDatasList = new List<OneDayScheduleData>();
   
    public OneDayScheduleData GetOneDayDataByName(RestType restType)
    {
        OneDayScheduleData temp = new OneDayScheduleData();
        foreach(OneDayScheduleData one in oneDayDatasList)
        {
            if (one.restType == restType) temp = one;
        }
        return temp;
    }

    public OneDayScheduleData GetOneDayDataByName(BroadCastType broadType)
    {
        OneDayScheduleData temp = new OneDayScheduleData();
        foreach (OneDayScheduleData one in oneDayDatasList)
        {
            if (one.broadcastType == broadType) temp = one;
        }
        return temp;
    }

    public OneDayScheduleData GetOneDayDataByName(GoOutType GooutType)
    {
        OneDayScheduleData temp = new OneDayScheduleData();
        foreach (OneDayScheduleData one in oneDayDatasList)
        {
            if (one.goOutType == GooutType) temp = one;
        }
        return temp;
    }

    public IEnumerator RequestAndSetDayDatas(string www)
    {
        UnityWebRequest wwww = UnityWebRequest.Get(www);        
        yield return wwww.SendWebRequest();

        string data = wwww.downloadHandler.text;
        string[] lines = data.Substring(0, data.Length).Split('\n');
        Queue<string> stringqueue = new Queue<string>();

        foreach(string line in lines)
        {
            stringqueue.Enqueue(line);
        }


        for(int i = 0;i<(int)BroadCastType.MaxCount_Name; i++)
        {
            ProcessStringToList(ScheduleType.BroadCast, i, stringqueue.Dequeue());
        }
        for (int i = 0; i <  (int)RestType.MaxCount; i++)
        {
            ProcessStringToList(ScheduleType.Rest, i, stringqueue.Dequeue());
        }
        for (int i = 0; i < (int)GoOutType.MaxCount; i++)
        {
            ProcessStringToList(ScheduleType.GoOut, i, stringqueue.Dequeue());
        }
    }

    void ProcessStringToList(ScheduleType scheduleType,int index,string data)
    {
        string[] lines = data.Substring(0, data.Length).Split('\t');
        Queue<string> tempstringQueue = new Queue<string>();

        foreach (string asdf in lines)
        {
            tempstringQueue.Enqueue(asdf);
        }

        OneDayScheduleData temp = new OneDayScheduleData();

        if(scheduleType == ScheduleType.BroadCast)
        {
            temp.scheduleType = ScheduleType.BroadCast;
            temp.broadcastType = (BroadCastType)index;
        }
        else if(scheduleType == ScheduleType.Rest)
        {
            temp.scheduleType = ScheduleType.Rest;
            temp.restType = (RestType)index;
        }
        else
        {
            temp.scheduleType = ScheduleType.GoOut;
            temp.goOutType = (GoOutType)index;
        }
        
        temp.KorName = tempstringQueue.Dequeue();
        temp.FisSubsUpValue = float.Parse(tempstringQueue.Dequeue());
        temp.PerSubsUpValue = float.Parse(tempstringQueue.Dequeue());
        temp.HeartVariance = float.Parse(tempstringQueue.Dequeue());
        temp.StarVariance = float.Parse(tempstringQueue.Dequeue());
        temp.InComeMag = float.Parse(tempstringQueue.Dequeue());
        temp.MoneyCost = int.Parse(tempstringQueue.Dequeue());
        for (int j = 0; j < 6; j++)
        {
            temp.Six_Stats[j] = float.Parse(tempstringQueue.Dequeue());
        }
        temp.infotext = tempstringQueue.Dequeue();
        oneDayDatasList.Add(temp);
    }

    #endregion

   

    #region Merchant
    public List<Item> ItemList = new List<Item>();
    public IEnumerator RequestAndSetItemDatas(string www)
    {
        UnityWebRequest wwww = UnityWebRequest.Get(www);
        yield return wwww.SendWebRequest();

        string data = wwww.downloadHandler.text;
        string[] lines = data.Substring(0, data.Length).Split('\n');

        foreach(string dattaa in lines)
        {
            SetItem(dattaa);
        }
    }

    void SetItem(string data)
    {
        Queue<string> tempstrings = new Queue<string>();

        string[] lines = data.Substring(0, data.Length).Split('\t');

        foreach (string date in lines)
        {
            tempstrings.Enqueue(date);
        }


        Item tempitem = new Item();

        tempitem.EntWeek = int.Parse(tempstrings.Dequeue()); 
        tempitem.ItemName = tempstrings.Dequeue();           
        tempitem.Cost = int.Parse(tempstrings.Dequeue());    
        tempitem.ItemImageName = tempstrings.Dequeue();      

        float[] tempint = new float[6];
        for (int j = 0; j < 6; j++)
        {
            tempint[j] = float.Parse(tempstrings.Dequeue());
        }

        tempitem.SixStats = tempint;
        ItemList.Add(tempitem);
    }


    #endregion


    #region StatProperty

    public Bonus GetProperty(StatName statName)
    {
        float highestStat = Managers.Data._myPlayerData.SixStat[(int)statName];
        Bonus bonus = new Bonus();

        if (highestStat >= 200)
        {
            bonus.SubBonus = 25;
            bonus.IncomeBonus = 25;
        }
        else if (highestStat >= 180)
        {
            bonus.SubBonus = 25;
            bonus.IncomeBonus = 20;
        }
        else if (highestStat >= 160)
        {
            bonus.SubBonus = 20;
            bonus.IncomeBonus = 20;
        }
        else if (highestStat >= 140)
        {
            bonus.SubBonus = 20;
            bonus.IncomeBonus = 15;
        }
        else if (highestStat >= 120)
        {
            bonus.SubBonus = 15;
            bonus.IncomeBonus = 15;
        }
        else if (highestStat >= 100)
        {
            bonus.SubBonus = 15;
            bonus.IncomeBonus = 10;
        }
        else if (highestStat >= 80)
        {
            bonus.SubBonus = 10;
            bonus.IncomeBonus = 10;
        }
        else if (highestStat >= 60)
        {
            bonus.SubBonus = 10;
            bonus.IncomeBonus = 5;
        }
        else if (highestStat >= 40)
        {
            bonus.SubBonus = 5;
            bonus.IncomeBonus = 5;
        }
        else if (highestStat >= 20)
        {
            bonus.SubBonus = 5;
            bonus.IncomeBonus = 0;
        }
        return bonus;
    }


    #endregion
}

[System.Serializable]
public class PlayerData
{
    public int NowWeek;
    public int nowSubCount;
    public int nowGoldAmount;
    public int NowHeart;
    public int NowStar;
    public float[] SixStat;
    public List<string> DoneEventNames;
    public List<string> BoughtItems;

    public PlayerData()
    {
        NowWeek = 1;
        nowSubCount = 0;
        nowGoldAmount = 0;
        NowHeart = 100;
        NowStar = 100;
        SixStat = new float[6];
        DoneEventNames = new List<string>();
        BoughtItems = new List<string>();
    }

    public float GetHighestStat()
    {
        float temp = 0;
        for(int i = 0;i<6;i++)
        {
            if(temp < SixStat[i])
            {
                temp = SixStat[i];
            }
        }
        return temp;
    }

    public Define.StatName GetHigestStatName()
    {
        Define.StatName temp = Define.StatName.Game;
        float temp2 = 0;

        for(int i = 0;i<6;i++)
        {
            if(temp2 < SixStat[i])
            {
                temp2 = SixStat[i];
                temp = (Define.StatName)i;
            }
        }
        return temp;
    }

    public void UpStat(float[] stats)
    {
        for(int i =0;i<6;i++)
        {
            SixStat[i] += stats[i];
            if (stats[i] != 0)
            {
                UI_MainBackUI.instance.GlitterStat(i);
            }
        }
    }
}


