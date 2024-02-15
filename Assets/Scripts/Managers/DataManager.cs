using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static Define;

//삭제 예정
public class DataManager
{ 
    public PlayerData PlayerData;
    public PersistentUserData PersistentUser;
    public void Init()
    {
        LoadData();
        
    }

    #region DataSave&Load

    void LoadData()
    {
        string path;
        string path2;
        if (Application.platform == RuntimePlatform.Android)
        {
            path = Path.Combine(Application.persistentDataPath, "PlayerData2.json");
            path2 = Path.Combine(Application.persistentDataPath, "Persistent2.json");
        }
        else
        {
            path = Path.Combine(Application.dataPath, "PlayerData2.json");
            path2 = Path.Combine(Application.dataPath, "Persistent2.json");
        }

        if (!File.Exists(path) || !File.Exists(path2))
        {
            PlayerData = new PlayerData();
            PersistentUser = new PersistentUserData();
            SaveData();
        }
    
        FileStream fileStream = new FileStream(path, FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        PlayerData = JsonUtility.FromJson<PlayerData>(jsonData);

        FileStream fileStream2 = new FileStream(path2, FileMode.Open);
        byte[] data2 = new byte[fileStream2.Length];
        fileStream2.Read(data2, 0, data2.Length);
        fileStream2.Close();
        string jsonData2 = Encoding.UTF8.GetString(data2);
        PersistentUser = JsonUtility.FromJson<PersistentUserData>(jsonData2);
    }

    public void SaveData()
    {
        string path;
        string path2;
        if (Application.platform == RuntimePlatform.Android)
        {
            path = Path.Combine(Application.persistentDataPath, "PlayerData2.json");
            path2 = Path.Combine(Application.persistentDataPath, "Persistent2.json");
        }
        else
        {
            path = Path.Combine(Application.dataPath, "PlayerData2.json");
            path2 = Path.Combine(Application.dataPath, "Persistent2.json");
        }
        string jsonData = JsonUtility.ToJson(PlayerData, true);
        FileStream fileStream = new FileStream(path, FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();

        string jsonData2 = JsonUtility.ToJson(PersistentUser, true);
        FileStream fileStream2 = new FileStream(path2, FileMode.Create);
        byte[] data2 = Encoding.UTF8.GetBytes(jsonData2);
        fileStream2.Write(data2, 0, data2.Length);
        fileStream2.Close();
    }

    public void SaveToCloud()
    {
        SaveData();
    }

    public void LoadFromCloud()
    {

    }


    public IEnumerator RequestAndSetRandEventDatas(string www)
    {
        UnityWebRequest wwww = UnityWebRequest.Get(www);
        yield return wwww.SendWebRequest();

        string data = wwww.downloadHandler.text;
        string[] lines = data.Substring(0, data.Length).Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            Managers.RandEvent.ProcessData(lines[i], i);
        }
    }

    #endregion

    #region ScheduleData

    //Schedule Popup데이터 관리용
    public OneDayScheduleData[] _SevenDayScheduleDatas = new OneDayScheduleData[7];
    public float[] _SeveDayScrollVarValue = new float[7];


    public List<OneDayScheduleData> oneDayDatasList = new List<OneDayScheduleData>();

    public OneDayScheduleData GetOneDayDataByObject(object type)
    {
        OneDayScheduleData oneDayData = null;

        if (type is RestType restType)
        {
            oneDayData = GetOneDayDataByName(restType);
        }
        else if (type is BroadCastType broadcastType)
        {
            oneDayData = GetOneDayDataByName(broadcastType);
        }
        else if (type is GoOutType gooutType)
        {
            oneDayData = GetOneDayDataByName(gooutType);
        }

        return oneDayData;
    }


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
            ProcessStringToList(ContentType.BroadCast, i, stringqueue.Dequeue());
        }
        for (int i = 0; i <  (int)RestType.MaxCount; i++)
        {
            ProcessStringToList(ContentType.Rest, i, stringqueue.Dequeue());
        }
        for (int i = 0; i < (int)GoOutType.MaxCount; i++)
        {
            ProcessStringToList(ContentType.GoOut, i, stringqueue.Dequeue());
        }
    }

    void ProcessStringToList(ContentType scheduleType,int index,string data)
    {
        string[] lines = data.Substring(0, data.Length).Split('\t');
        Queue<string> tempstringQueue = new Queue<string>();

        foreach (string asdf in lines)
        {
            tempstringQueue.Enqueue(asdf);
        }

        OneDayScheduleData temp = new OneDayScheduleData();

        if(scheduleType == ContentType.BroadCast)
        {
            temp.scheduleType = ContentType.BroadCast;
            temp.broadcastType = (BroadCastType)index;
        }
        else if(scheduleType == ContentType.Rest)
        {
            temp.scheduleType = ContentType.Rest;
            temp.restType = (RestType)index;
        }
        else
        {
            temp.scheduleType = ContentType.GoOut;
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
        temp.PathName = tempstringQueue.Dequeue();
        temp.RubiaAni = tempstringQueue.Dequeue();
        temp.infotext = tempstringQueue.Dequeue();
        temp.ArchiveInfoText = tempstringQueue.Dequeue();
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
            tempint[j] = string.IsNullOrEmpty(tempstrings.Peek()) ? 0 : int.Parse(tempstrings.Peek());
            tempstrings.Dequeue();
        }

        tempitem.SixStats = tempint;

        tempitem.Karma = string.IsNullOrEmpty(tempstrings.Peek()) ? 0 : int.Parse(tempstrings.Peek());
        tempstrings.Dequeue();

        tempitem.ItemInfoText = tempstrings.Dequeue();
        ItemList.Add(tempitem);
    }


    #endregion


    #region StatProperty


    public Bonus GetMainProperty(StatName statName)
    {
        float highestStat = Managers.Data.PlayerData.SixStat[(int)statName];
        return GetMainProperty(highestStat);
    }

    public Bonus GetMainProperty(float Value)
    {
        Bonus bonus = new Bonus();

        int bonusValue = Mathf.FloorToInt(Value) / 20;
        bonus.SubBonus = ((bonusValue + 1) / 2) * Managers.instance.MainStat_ValuePerLevel;
        bonus.IncomeBonus = ((bonusValue) / 2) * Managers.instance.MainStat_ValuePerLevel;

        return bonus;
    }

    #endregion
}




