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
        // �ʱ� ��ġ �� ���İ� ����
        Vector2 initialPosition = rect.anchoredPosition;
        float initialAlpha = 1f;

        // ��ǥ ��ġ �� ���İ� ����
        Vector2 targetPosition = initialPosition + new Vector2(0f, Move_Y_Distance);
        float targetAlpha = 0.6f;

        // �ð� ���� �ʱ�ȭ
        float elapsedTime = 0f;

        // �ִϸ��̼� ����
        while (elapsedTime < MoveTime)
        {
            // �ð� ����� ���� ������ ���
            float t = elapsedTime / MoveTime;

            // ��ġ �̵�
            rect.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, t);

            // ���İ� ����
            text.alpha = Mathf.Lerp(initialAlpha, targetAlpha, t);

            // �ð� ������Ʈ
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        text.alpha = 0;
        // �ִϸ��̼� ���� �� �ʱ� ���·� �ǵ�����
        rect.anchoredPosition = initialPosition;
    }
}
