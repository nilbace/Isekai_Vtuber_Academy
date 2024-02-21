using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaLerp : MonoBehaviour
{
    public float minAlpha = 1/255f; // 최소 알파값
    public float maxAlpha = 50f/255f; // 최대 알파값
    public float lerpSpeed = 1f; // lerp 속도

    private float currentAlpha; // 현재 알파값
    private bool isIncreasing = true; // 알파값이 증가 중인지 여부
    Image _image;

    private void Start()
    {
        // 초기 알파값 설정
        currentAlpha = minAlpha;
        _image = GetComponent<Image>();

        // 알파값을 lerp하는 메서드 호출
        StartAlphaLerp();
    }

    private void StartAlphaLerp()
    {
        // 알파값을 lerp하여 설정하는 메서드
        currentAlpha = minAlpha;
        isIncreasing = true;
        LerpAlpha();
    }

    private void LerpAlpha()
    {
        // 알파값을 lerp하여 설정
        float t = Mathf.PingPong(Time.time * lerpSpeed, 1f);
        currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, t);

        // 알파값이 최대값에 도달한 경우
        if (currentAlpha >= maxAlpha)
        {
            // 알파값을 최대값으로 설정하고 감소하는 방향으로 변경
            currentAlpha = maxAlpha;
            isIncreasing = false;
        }
        // 알파값이 최소값에 도달한 경우
        else if (currentAlpha <= minAlpha)
        {
            // 알파값을 최소값으로 설정하고 증가하는 방향으로 변경
            currentAlpha = minAlpha;
            isIncreasing = true;
        }

        // 이미지 컴포넌트의 알파값 설정
        _image.color = new Color(1f, 1f, 1f, currentAlpha);
    }

    private void Update()
    {
        // 알파값을 lerp하는 메서드 호출
        LerpAlpha();
    }
}
