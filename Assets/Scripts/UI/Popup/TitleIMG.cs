using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TitleIMG : UI_Popup, IPointerClickHandler
{
    public Image Title;
    public Image TouchToStart;

    public float duration = 1f; // Lerp에 걸리는 시간
    private float startAlpha = 0.3f; // 시작 알파 값
    private float targetAlpha = 1f; // 목표 알파 값.

    private void Start()
    {
        base.Init();
        StartCoroutine(LerpAlphaRepeat());
        StartCoroutine(LerpAlpha());
        Title.rectTransform.DOAnchorPosY(Title.rectTransform.anchoredPosition.y + 30f, duration);
    }

    private IEnumerator LerpAlphaRepeat()
    {
        float elapsedTime = 0f;
        Color startColor = TouchToStart.color;
        float startAlpha = 0.3f; // 시작 알파 값
        float targetAlpha = 1f; // 목표 알파 값
        bool increasing = true; // 알파 값이 증가 중인지 여부를 나타내는 플래그

        while (true)
        {
            if (increasing)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= duration)
                {
                    elapsedTime = duration;
                    increasing = false;
                }
            }
            else
            {
                elapsedTime -= Time.deltaTime;
                if (elapsedTime <= 0f)
                {
                    elapsedTime = 0f;
                    increasing = true;
                }
            }

            float t = elapsedTime / duration;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            Color newColor = startColor;
            newColor.a = currentAlpha;

            TouchToStart.color = newColor;

            yield return null;
        }
    }

    private IEnumerator LerpAlpha()
    {
        float elapsedTime = 0f;
        Color startColor = Title.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            Color newColor = startColor;
            newColor.a = currentAlpha;

            Title.color = newColor;

            yield return null;
        }

        // 알파 값이 최종 목표치로 정확히 도달하도록 보정
        Color finalColor = startColor;
        finalColor.a = targetAlpha;
        Title.color = finalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UI_MainBackUI.instance.SetScreenAniSpeed(1);
        UI_MainBackUI.instance.StartScreenAnimation("WaitingArea");
        Managers.instance.CloseTitle();
    }
}