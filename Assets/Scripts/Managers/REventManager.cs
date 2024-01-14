using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class REventManager
{
    public List<WeekEventData> EventDatasList = new List<WeekEventData>();

    

    public void ProcessData(string data)
    {
        string[] lines = data.Substring(0, data.Length).Split('\t');
        Queue<string> tempstrings = new Queue<string>();
        foreach (string temp in lines)
        { tempstrings.Enqueue(temp);}

        WeekEventData tempEventData = new WeekEventData();
        tempEventData.EventName = tempstrings.Dequeue();
        tempEventData.StatName = (StatName)Enum.Parse(typeof(StatName), tempstrings.Dequeue());
        tempEventData.ReqStat = int.Parse(tempstrings.Dequeue());
        tempEventData.EventDataType = (EventDataType)Enum.Parse(typeof(EventDataType), tempstrings.Dequeue());
        tempEventData.OccurableWeek = int.Parse(tempstrings.Dequeue());
        tempEventData.CutSceneName = tempstrings.Dequeue();

        float[] floatArray = new float[8];
        for(int i =0;i<8;i++)
        {
            string item = tempstrings.Dequeue();
            float parsedInt;

            if (float.TryParse(item, out parsedInt))
            {
                floatArray[i] = parsedInt;
            }
            else
            {
                Debug.Log($"Failed to parse item at index {i}: {item}");
            }
        }
        tempEventData.Option1 = floatArray;

        float[] floatArray2 = new float[8];
        for (int i = 0; i < 8; i++)
        {
            string item = tempstrings.Dequeue();
            float parsedInt;

            if (float.TryParse(item, out parsedInt))
            {
                floatArray2[i] = parsedInt;
            }
            else
            {
                Debug.Log($"Failed to parse item at index {i}: {item}");
            }
        }
        tempEventData.Option2 = floatArray2;


        tempEventData.EventInfoString = tempstrings.Dequeue();

        tempEventData.BTN1text = tempstrings.Dequeue();
        tempEventData.BTN1ResultText = tempstrings.Dequeue();
        tempEventData.BTN2text = tempstrings.Dequeue();
        tempEventData.BTN2ResultText = tempstrings.Dequeue();

        EventDatasList.Add(tempEventData);
    }

    WeekEventData tempConditionEvent;
    
    /// <summary>
    /// 주차나 조건에 알맞은 이벤트를 호출
    /// </summary>
    /// <returns></returns>
    public WeekEventData GetProperEvent()
    {
        WeekEventData temp = new WeekEventData();

        List<WeekEventData> tempEventDatasList = new List<WeekEventData>(EventDatasList);
        RemoveDoneEvent(tempEventDatasList);


        foreach(string name in Managers.Data._myPlayerData.DoneEventNames)
        {
            foreach(WeekEventData each in tempEventDatasList)
            {
                if (each.EventName == name) tempEventDatasList.Remove(each);
            }
        }
        
        if(Managers.Data._myPlayerData.NowWeek%4 == 0)
        {
            temp = GetMainEvent(tempEventDatasList);
        }
        else if(IsOnCondition(tempEventDatasList))
        {
            temp = tempConditionEvent;
        }
        else
        {
            temp = GetRandEvent(tempEventDatasList);
        }

        Managers.Data._myPlayerData.DoneEventNames.Add(temp.EventName);
        return temp;
    }

    /// <summary>
    /// 4주차마다 메인 이벤트
    /// </summary>
    /// <param name="eventlist">넘겨받는 이벤트</param>
    /// <returns></returns>
    WeekEventData GetMainEvent(List<WeekEventData> eventlist)
    {
        WeekEventData temp = new WeekEventData();
        
        switch (Managers.Data._myPlayerData.GetHigestStatName())
        {
            case StatName.Game:
                foreach(WeekEventData temp2 in eventlist)
                {
                    if (temp2.OccurableWeek == Managers.Data._myPlayerData.NowWeek 
                        && temp2.ReqStat <= Managers.Data._myPlayerData.SixStat[0] 
                        && temp2.StatName == StatName.Game)
                        temp = temp2;
                }
                break;
            case StatName.Song:
                foreach (WeekEventData temp4 in eventlist)
                {
                    if (temp4.OccurableWeek == Managers.Data._myPlayerData.NowWeek
                        && temp4.ReqStat <= Managers.Data._myPlayerData.SixStat[1]
                        && temp4.StatName == StatName.Song)
                        temp = temp4;
                }
                break;
            case StatName.Draw:
                foreach (WeekEventData temp3 in eventlist)
                {
                    if (temp3.OccurableWeek == Managers.Data._myPlayerData.NowWeek
                        && temp3.ReqStat <= Managers.Data._myPlayerData.SixStat[2]
                        && temp3.StatName == StatName.Draw)
                        temp = temp3;
                }
                break;
            
        }
        return temp;
    }

    WeekEventData GetRandEvent(List<WeekEventData> eventlist)
    {
        List<WeekEventData> tempEventDatasList = new List<WeekEventData>();
        foreach (WeekEventData even in eventlist)
        {
            if (even.EventDataType == EventDataType.Random)
                tempEventDatasList.Add(even);
        }

        int rand = UnityEngine.Random.Range(0, tempEventDatasList.Count);


        return tempEventDatasList[rand];
    }

    bool IsOnCondition(List<WeekEventData> eventlist)
    {
        List<WeekEventData> tempEventDatasList = new List<WeekEventData>();
        foreach (WeekEventData even in eventlist)
        {
            if (even.EventDataType == EventDataType.Conditioned)
                tempEventDatasList.Add(even);
        }

        foreach (WeekEventData even in tempEventDatasList)
        {
            switch (even.StatName)
            {
                case StatName.Game:
                    break;
                case StatName.Song:
                    break;
                case StatName.Draw:
                    break;
                case StatName.Strength:
                    break;
                case StatName.Mental:
                    break;
                case StatName.Luck:
                    break;
                
                case StatName.Subs:
                    if (even.ReqStat <= Managers.Data._myPlayerData.nowSubCount)
                    {
                        
                        tempConditionEvent = even;
                        return true;
                    }
                    break;

                case StatName.Week:
                    if (even.ReqStat <= Managers.Data._myPlayerData.NowWeek)
                    {
                        tempConditionEvent = even;
                        return true;
                    }
                    break;

            }

        }
        return false;
    }

    void RemoveDoneEvent(List<WeekEventData> eventlist)
    {
        eventlist.RemoveAll(eventdata => Managers.Data._myPlayerData.DoneEventNames.Contains(eventdata.EventName));
    }


    //이벤트 하나의 정보
    public class WeekEventData
    {
        public string EventName;
        public StatName StatName;
        public int ReqStat;
        public EventDataType EventDataType;
        public int OccurableWeek;
        public string CutSceneName;
        public float[] Option1;
        public float[] Option2;
        public string EventInfoString;
        public string BTN1text;
        public string BTN1ResultText;
        public string BTN2text;
        public string BTN2ResultText;

        public WeekEventData()
        {
            Option1 = new float[8];
            Option2 = new float[8];
            EventInfoString = "빈 이벤트";
        }

        public void PrintData()
        {
            Debug.Log($"이벤트 이름 : {EventName}");
            Debug.Log($"발생 가능 주차 : {this.OccurableWeek}");
            Debug.Log($"필요 스텟과 수치 : {StatName} {ReqStat}");
        }
    }

}
