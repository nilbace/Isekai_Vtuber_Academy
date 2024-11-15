using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }
    public static void AddUIEvent(this GameObject go, Action<UnityEngine.EventSystems.PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    public static string ConvertEuroToNewline(this string text)
    {
        return text.Replace("��", "\n");
    }

    public static string RoundToString(this float value)
    {
        return Mathf.Round(value).ToString();
    }
}
