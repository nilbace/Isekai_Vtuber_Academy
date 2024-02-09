using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RabbitAppear : MonoBehaviour
{
    public RectTransform targetRectTransform;  // 이동시킬 UI 컴포넌트의 RectTransform
    public float offset = 100f;  // 아래로 이동할 거리
    public float moveTime = 1f;  // 이동 시간
    public float shakeStrength = 20f;  // 떨림의 세기
    bool startShake;
    bool canMove = true;
    public float cooldown;
    float cooldownTimer;

    public Image ChatBubble;
    public TMPro.TMP_Text ChatTMP;
    public float duration;
    public string[] Dialogues;

    private Vector2 initialPosition;  // 초기 위치

    private void Start()
    {
        // 초기 위치 저장
        initialPosition = targetRectTransform.anchoredPosition;
        ChatBubble.color = Define.alpha0;
        ChatTMP.color = new Color(0, 0, 0, 0);
        ChatTMP.text = Dialogues[Random.Range(0, 5)];

        // 아래로 즉시 이동
        targetRectTransform.anchoredPosition -= new Vector2(0f, offset);

        // 올라오는 Tweener 생성
        Tween moveUpTween = targetRectTransform.DOAnchorPosY(initialPosition.y, moveTime);
        startShake = true;
        // 올라오는 Tweener의 완료 시점에 진동 멈춤
        moveUpTween.OnComplete(() =>
        {
            startShake = false;
            targetRectTransform.DOAnchorPos(initialPosition, 0.1f);
            StartCoroutine(ShowChatBubble());
        });


    }
    private void Update()
    {
        if (canMove && startShake)
        {
            // 랜덤한 이동량 계산
            float randomMovement = Random.Range(-shakeStrength, shakeStrength);
            targetRectTransform.anchoredPosition += new Vector2(randomMovement, 0f);

            // 쿨다운 시작
            canMove = false;
            cooldownTimer = cooldown;
        }
        else
        {
            // 쿨다운 중이면 타이머 감소
            cooldownTimer -= Time.deltaTime;

            // 쿨다운이 끝나면 이동 가능 상태로 변경
            if (cooldownTimer <= 0f)
            {
                canMove = true;
            }
        }
    }

    IEnumerator ShowChatBubble()
    {
        float elapsedTime = 0f;
        Color startColor = ChatBubble.color;
        Color startColor2 = ChatTMP.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float currentAlpha = Mathf.Lerp(0, 1, t);

            Color newColor = startColor;
            newColor.a = currentAlpha;
            ChatBubble.color = newColor;

            Color newColor2 = startColor2;
            newColor2.a = currentAlpha;
            ChatTMP.color = newColor2;

            yield return null;
        }
    }
}
