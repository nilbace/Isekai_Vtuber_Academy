using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnChildUpdown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float offset = 1.0f;  // 이동할 Offset 값

    public void OnPointerDown(PointerEventData eventData)
    {

        // 자식 오브젝트들의 위치를 Offset만큼 내리기
        foreach (Transform child in transform)
        {
            child.position -= new Vector3(0, offset, 0);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        // 자식 오브젝트들의 위치를 Offset만큼 올리기
        foreach (Transform child in transform)
        {
            child.position += new Vector3(0, offset, 0);
        }
    }
}