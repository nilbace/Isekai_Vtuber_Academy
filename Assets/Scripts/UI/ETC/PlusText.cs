using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using static Define;

public class PlusText : MonoBehaviour
{
    TMP_Text text;
    RectTransform rect;
    public float MoveTime;
    public float Move_Y_Distance;
    Vector2 InitPoz;
    
    void Start()
    {
        text = GetComponent<TMP_Text>();
        rect = GetComponent<RectTransform>();
        InitPoz = rect.anchoredPosition;
    }

    public Color PlusColor;
    public Color MinusColor;

    public void PlayAnimation(StatName statName, float value)
    {
        if (value > 0)
        {
            text.color = PlusColor;
            text.text = GetIconString((int)statName) + "+" + value.ToString("F0");
        }
        else
        {
            text.color = MinusColor;
            text.text = GetIconString((int)statName) + value.ToString("F0");
        }

        int intValue = (int)statName;
        Vector2 offset = Vector2.zero;

        if (intValue >= 0 && intValue <= 2)
        {
            offset.y = -39f * intValue ;
        }
        else if (intValue >= 3 && intValue <= 5)
        {
            offset.x = 152f;
            offset.y = -39f * (intValue - 3);
        }
        rect.anchoredPosition = InitPoz;
        rect.anchoredPosition += offset;
       
        StartCoroutine(AnimationCoroutine());
    }

    private IEnumerator AnimationCoroutine()
    {
        // 초기 위치 및 알파값 설정
        Vector2 initialPosition = rect.anchoredPosition;
        float initialAlpha = 1f;

        // 목표 위치 및 알파값 설정
        Vector2 targetPosition = initialPosition + new Vector2(0f, Move_Y_Distance);
        float targetAlpha = 0.6f;

        // 시간 변수 초기화
        float elapsedTime = 0f;

        // 애니메이션 진행
        while (elapsedTime < MoveTime)
        {
            // 시간 경과에 따른 보간값 계산
            float t = elapsedTime / MoveTime;

            // 위치 이동
            rect.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, t);

            // 알파값 변경
            text.alpha = Mathf.Lerp(initialAlpha, targetAlpha, t);

            // 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        text.alpha = 0;
        // 애니메이션 종료 후 초기 상태로 되돌리기
        rect.anchoredPosition = initialPosition;
    }
}
