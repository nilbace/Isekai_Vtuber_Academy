  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util{

    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if(component == null)
            component = go.AddComponent<T>();
        return component;
    }


    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if(go == null)
            return null;

        if(recursive == false)
        {
            for(int i = 0; i<go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if(string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if(component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                if(string.IsNullOrEmpty(name) || component.name == name)
                {
                    return component;
                }
            }
        }

        return null;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if(transform==null)
            return null;

        return transform.gameObject;
    }
 
    public static string FormatMoney(int amount)
    {
        // 만 단위로 나누기
        int won = amount / 10000;

        // 만 단위 이하의 금액
        int rest = amount % 10000;

        // 결과 문자열 생성
        string result = rest.ToString();

        if (won > 0)
        {
            result = won.ToString() + "만 " + rest.ToString();
        }

        return result + "원";
    }

    public static string FormatSubs(int amount)
    {
        // 만 단위로 나누기
        int won = amount / 10000;

        // 만 단위 이하의 금액
        int rest = amount % 10000;

        // 결과 문자열 생성
        string result = rest.ToString();

        if (won > 0)
        {
            result = won.ToString() + "만 " + rest.ToString() ;
        }

        return result + "명";
    }
}
