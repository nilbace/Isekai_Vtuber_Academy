using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowHealthPopup : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Managers.UI_Manager.ShowPopupUI<UI_HeartStarProperty>();
    }
}
