using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class REventManager
{
    public List<EventData> EventDatasList = new List<EventData>();

    

    public void ProcessData(string data)
    {
        string[] lines = data.Substring(0, data.Length - 1).Split('\t');
        Queue<string> tempstrings = new Queue<string>();
        foreach (string temp in lines)
            tempstrings.Enqueue(temp);

        EventData tempEventData = new EventData();
        tempEventData.EventName = tempstrings.Dequeue();
        tempEventData.StatName = (StatName)Enum.Parse(typeof(StatName), tempstrings.Dequeue());
        tempEventData.ReqStat = int.Parse(tempstrings.Dequeue());
        tempEventData.EventDataType = (EventDataType)Enum.Parse(typeof(EventDataType), tempstrings.Dequeue());
        tempEventData.OccurableWeek = int.Parse(tempstrings.Dequeue());
        tempEventData.CutSceneName = tempstrings.Dequeue();

        int[] intArray = new int[10];
        for(int i =0;i<10;i++)
        {
            string item = tempstrings.Dequeue();
            int parsedInt;

            if (int.TryParse(item, out parsedInt))
            {
                intArray[i] = parsedInt;
            }
            else
            {
                // 변환 실패 시 예외 처리 또는 기본값 할당 등의 로직을 추가할 수 있습니다.
                // 예를 들면, intArray[i] = defaultValue; 등
                Debug.Log($"Failed to parse item at index {i}: {item}");
            }
        }
        tempEventData.Change = intArray;
        tempEventData.EventInfoString = tempstrings.Dequeue();

        EventDatasList.Add(tempEventData);
    }

    EventData tempConditionEvent;
    
    public EventData GetProperEvent()
    {
        EventData temp = new EventData();

        List<EventData> tempEventDatasList = new List<EventData>(EventDatasList);
        RemoveDoneEvent(tempEventDatasList);


        foreach(string name in Managers.Data._myPlayerData.DoneEventNames)
        {
            foreach(EventData each in tempEventDatasList)
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
        Managers.Data.SaveData();
        return temp;
    }

    EventData GetMainEvent(List<EventData> eventlist)
    {
        EventData temp = new EventData();
        
        switch (Managers.Data._myPlayerData.GetHigestStatName())
        {
            case StatName.Game:
                foreach(EventData temp2 in eventlist)
                {
                    if (temp2.OccurableWeek == Managers.Data._myPlayerData.NowWeek 
                        && temp2.ReqStat <= Managers.Data._myPlayerData.SixStat[0] 
                        && temp2.StatName == StatName.Game)
                        temp = temp2;
                }
                break;
            case StatName.Song:
                EventData foundEvent2 = eventlist.Find(eventData => eventData.StatName == StatName.Song && eventData.OccurableWeek == Managers.Data._myPlayerData.NowWeek);
                if (foundEvent2 != null)
                {
                    Debug.Log("코딩 망했다");
                }
                else
                {
                    temp = foundEvent2;
                }
                break;
            case StatName.Chat:
                EventData foundEvent3 = eventlist.Find(eventData => eventData.StatName == StatName.Chat && eventData.OccurableWeek == Managers.Data._myPlayerData.NowWeek);
                if (foundEvent3 != null)
                {
                    Debug.Log("코딩 망했다");
                }
                else
                {
                    temp = foundEvent3;
                }
                break;
            
        }
        return temp;
    }

    EventData GetRandEvent(List<EventData> eventlist)
    {
        List<EventData> tempEventDatasList = new List<EventData>();
        foreach (EventData even in eventlist)
        {
            if (even.EventDataType == EventDataType.Random)
                tempEventDatasList.Add(even);
        }

        int rand = UnityEngine.Random.Range(0, tempEventDatasList.Count);


        return tempEventDatasList[rand];
    }

    bool IsOnCondition(List<EventData> eventlist)
    {
        List<EventData> tempEventDatasList = new List<EventData>();
        foreach (EventData even in eventlist)
        {
            if (even.EventDataType == EventDataType.Conditioned)
                tempEventDatasList.Add(even);
        }

        foreach (EventData even in tempEventDatasList)
        {
            switch (even.StatName)
            {
                case StatName.Game:
                    break;
                case StatName.Song:
                    break;
                case StatName.Chat:
                    break;
                case StatName.Health:
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

    void RemoveDoneEvent(List<EventData> eventlist)
    {
        eventlist.RemoveAll(eventdata => Managers.Data._myPlayerData.DoneEventNames.Contains(eventdata.EventName));
    }


    //이벤트 하나의 정보
    public class EventData
    {
        public string EventName;
        public StatName StatName;
        public int ReqStat;
        public EventDataType EventDataType;
        public int OccurableWeek;
        public string CutSceneName;
        public int[] Change;
        public string EventInfoString;

        public EventData()
        {
            Change = new int[10];
        }

        public void PrintData()
        {
            Debug.Log($"이벤트 이름 : {EventName}");
            Debug.Log($"발생 가능 주차 : {this.OccurableWeek}");
            Debug.Log($"필요 스텟과 수치 : {StatName} {ReqStat}");
        }
    }

}
