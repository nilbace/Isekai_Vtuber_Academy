using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;
using static Define;

public class DataParser : MonoSingleton<DataParser>
{
    public TextAsset[] TextDatas;
    public List<NickName> NickNameList;
    private void Awake() => base.Awake();
    void Start()
    {
        DataDictionaryInit();
    }

    void DataDictionaryInit()
    {
        //ÄªÈ£ ºÎºÐ Parse
        DataDictionary.Add(typeof(NickName), TextDatas[0].text);
        NickNameList = MakeList<NickName>(DataDictionary[typeof(NickName)]);
    }


    #region ParseData
    Dictionary<Type, string> DataDictionary = new Dictionary<Type, string>();

    


    private T ParseDataTable<T>(string[] extractedData)
    {
        object data = Activator.CreateInstance(typeof(T));
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
       
        for (int i = 0; i < extractedData.Length; i++)
        {
            try
            {
                Type type = fields[i].FieldType;

                if (string.IsNullOrEmpty(extractedData[i])) continue;

                if (type == typeof(int))
                    fields[i].SetValue(data, int.Parse(extractedData[i]));
                else if (type == typeof(float))
                    fields[i].SetValue(data, float.Parse(extractedData[i]));
                else if (type == typeof(string))
                {
                    fields[i].SetValue(data, extractedData[i]);
                }
                else if (type == typeof(NickNameType))
                    fields[i].SetValue(data, (NickNameType)Enum.Parse(typeof(NickNameType), extractedData[i]));
            }
            catch (Exception e)
            {
                //Debug.LogError($"Data Parsing Error : {e.Message}");
            }
        }

        return (T)data;
    }

    private List<T> MakeList<T>(string parsedData)
    {
        List<T> list = new List<T>();
        string[] splitedData = parsedData.Split('\n');

        foreach (string s in splitedData)
        {
            string[] data = s.Split('\t');
            list.Add(ParseDataTable<T>(data));
        }

        return list;
    }

    #endregion
}
