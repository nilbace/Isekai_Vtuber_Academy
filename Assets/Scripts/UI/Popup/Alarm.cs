using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//알람 팝업창
public class Alarm : MonoSingleton<Alarm>
{
    private static Coroutine fadeCoroutine;
    static bool isCoroutineRunning = false;
    public Image image;
    public TMP_Text text;
    public Queue<string> StringQueue;

    private void Awake()
    {
        base.Awake();
        Inst.StringQueue = new Queue<string>();
    }

    void Start()
    {
        image.color = Color.clear;
        text.color = new Color(1,1,1,0);
    }

    public static void ShowAlarm(string Text)
    {
        Inst.StringQueue.Enqueue(Text); // 받은 문자열을 Queue에 추가합니다.
        Inst.StartCoroutine(FadeCorController());
    }
    static IEnumerator FadeCorController()
    {
        if (isCoroutineRunning)
        {
            yield break;
        }

        isCoroutineRunning = true;

        while (Inst.StringQueue.Count > 0)
        {
            string text = Inst.StringQueue.Dequeue();
            yield return Inst.StartCoroutine(FadeCoroutine(text));
        }

        isCoroutineRunning = false;
    }

    private static IEnumerator FadeCoroutine(string Text)
    {
        // 텍스트 변경
        Inst.text.text = Text;
        float durationIn = 0.3f; // 페이드 인 시간 (0.3초)
        float durationOut = 0.7f; // 페이드 아웃 시간 (0.7초)

        float time = 0f;

        // 페이드 인
        while (time < durationIn)
        {
            float alpha = Mathf.Lerp(0f, (155f/255f), time / durationIn);
            float alpha2 = Mathf.Lerp(0f, 1f, time / durationIn);
            Color imageColor = Inst.image.color;
            Color textColor = Inst.text.color;
            imageColor.a = alpha;
            textColor.a = alpha2;
            Inst.image.color = imageColor;
            Inst.text.color = textColor;
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.7f);

        // 페이드 아웃
        time = 0f;
        while (time < durationOut)
        {
            float alpha = Mathf.Lerp((155f / 255f), 0f, time / durationOut);
            float alpha2 = Mathf.Lerp(1f, 0f, time / durationOut);
            Color imageColor = Inst.image.color;
            Color textColor = Inst.text.color;
            imageColor.a = alpha;
            textColor.a = alpha2;
            Inst.image.color = imageColor;
            Inst.text.color = textColor;
            time += Time.deltaTime;
            yield return null;
        }
    }
}