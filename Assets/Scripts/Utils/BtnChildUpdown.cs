using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BtnChildUpdown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool Interactable = true;
    public float offset = 1.0f;  // 이동할 Offset 값

    private void Awake()
    {
        Interactable = true;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Interactable)
        {

            // 자식 오브젝트들의 위치를 Offset만큼 내리기
            foreach (Transform child in transform)
            {
                child.position -= new Vector3(0, offset, 0);
            }
        }
    }

    public void SetUninteractable()
    {
        Interactable = false;
        foreach (Transform child in transform)
        {
            child.position -= new Vector3(0, offset, 0);
        }
    }

    public void NotEnoughMoney()
    {
        Interactable = false;
        foreach (Transform child in transform)
        {
            TMPro.TMP_Text textComponent = child.GetComponent<TMPro.TMP_Text>();
            if (textComponent != null)
            {
                Color textColor = textComponent.color;
                textColor.a = 0.5f;
                textComponent.color = textColor;
            }
        }
    }



public void OnPointerUp(PointerEventData eventData)
    {

        if(Interactable)
        {
            // 자식 오브젝트들의 위치를 Offset만큼 올리기
            foreach (Transform child in transform)
            {
                child.position += new Vector3(0, offset, 0);
            }
        }
    }
}