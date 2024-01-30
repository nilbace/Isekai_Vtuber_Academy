using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SchedulePopup_Category : MonoSingleton<UI_SchedulePopup_Category>
{
    public Button targetButton;  // 체크할 대상 Button

    private bool isButtonDown = false;  // 버튼이 눌렸는지 여부

    private void Update()
    {
        // 마우스 왼쪽 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))
        {
            // targetButton이 null이 아니고 마우스 클릭된 위치가 targetButton의 RectTransform 안에 있는지 확인
            if (targetButton != null && RectTransformUtility.RectangleContainsScreenPoint(targetButton.GetComponent<RectTransform>(), Input.mousePosition))
            {
                isButtonDown = true;
                Debug.Log("버튼 눌림");
            }
        }

        // 마우스 왼쪽 버튼을 뗐을 때
        if (Input.GetMouseButtonUp(0))
        {
            // targetButton이 null이 아니고 마우스 클릭된 위치가 targetButton의 RectTransform 안에 있는지 확인하고 버튼이 눌린 상태인지 확인
            if (targetButton != null && RectTransformUtility.RectangleContainsScreenPoint(targetButton.GetComponent<RectTransform>(), Input.mousePosition) && isButtonDown)
            {
                isButtonDown = false;
                Debug.Log("버튼 뗌");
            }
        }
    }

    private void Start()
    {
        targetButton.onClick.AddListener(() => Debug.Log("버튼 실행"));
    }
}
