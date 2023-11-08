using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MM : MonoBehaviour
{
    public static MM instance;

    
    Animator animator;
    
    public enum MMState { usual, OnSchedule, PushAni}

    MMState _nowMMState = MMState.usual;

    public MMState NowMMState { get { return _nowMMState; } set { _nowMMState = value; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(RequestAndSetDayDatas(MerchantURL));
    }

    private void OnMouseDown()
    {
        if(_nowMMState == MMState.OnSchedule)
        {
            animator.Play("push");
            UI_SchedulePopup.instance.ResetSchedule();
        }

        Managers.Sound.Play("power");
    }

    const string MerchantURL = "https://docs.google.com/spreadsheets/d/1WjIWPgya-w_QcNe6pWE_iug0bsF6uwTFDRY8j2MkO3o/export?format=tsv&gid=1725059436&range=A2:D4";
    public IEnumerator RequestAndSetDayDatas(string www)
    {
        UnityWebRequest wwww = UnityWebRequest.Get(www);
        yield return wwww.SendWebRequest();

        string data = wwww.downloadHandler.text;
        string[] lines = data.Substring(0, data.Length).Split('\n');

        foreach (string line in lines)
        {
            Debug.Log(line);
        }
    }
}
