using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;

public class MerChantItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public TMPro.TMP_Text NameTmp;
    public TMPro.TMP_Text CostStatTMP;
    public TMPro.TMP_Text FlavorTMP;
    public Item _thisItem;
    public static Item BuyUIItem;

    public int UpDownOffset;

    public void Setting(Item item)
    {
        _thisItem = item;

        if(_thisItem.Cost <= Managers.Data.PlayerData.nowGoldAmount && !IsBought(_thisItem))
        {
            NameTmp.text = _thisItem.ItemName;
            CostStatTMP.text = GetCostStatString();
            FlavorTMP.text = _thisItem.ItemInfoText;
            GetComponent<Button>().interactable = true;
        }
        else if(IsBought(_thisItem))
        {
            NameTmp.text = _thisItem.ItemName + " 구매 완료";
            CostStatTMP.text = "";
            FlavorTMP.text = _thisItem.ItemInfoText;
            GetComponent<Button>().interactable = false;
        }
        else
        {
            NameTmp.text = _thisItem.ItemName;
            CostStatTMP.text = $" <sprite=7><color=red>{_thisItem.Cost}</color>" + " 구매 불가";
            FlavorTMP.text = _thisItem.ItemInfoText;
            GetComponent<Button>().interactable = false;
            GetComponent<Image>().sprite = GetComponent<Button>().spriteState.pressedSprite;
            DownChildren();
        }

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(ShowBuyUI);
    }

    string GetCostStatString()
    {
        string temp = "";
        for (int i = 0; i < 6; i++)
        {
            if (_thisItem.SixStats[i] != 0)
            {
                return "<sprite=7> " + _thisItem.Cost + $"   <sprite={i}>+" + _thisItem.SixStats[i].ToString();
            }
        }
        return temp;
    }

    bool IsBought(Item item)
    {
        foreach(string temp in Managers.Data.PlayerData.BoughtItems)
        {
            if (temp == item.ItemName) return true;
        }
        return false;
    }

    void ShowBuyUI()
    {
        BuyUIItem = _thisItem;
        Managers.UI_Manager.ShowPopupUI<UI_Buy>();
        Managers.Sound.Play(Sound.SmallBTN);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DownChildren();
    }

    void DownChildren()
    {
        foreach(Transform tr in GetComponentsInChildren<Transform>())
        {
            if (tr.name == "TMP SubMeshUI [TextMeshPro/Sprite]") continue;
            tr.position -= UpDownOffset * Vector3.up;
        }
    }

    void UpChildren()
    {
        foreach (Transform tr in GetComponentsInChildren<Transform>())
        {
            if (tr.name == "TMP SubMeshUI [TextMeshPro/Sprite]") continue;
            tr.position += UpDownOffset * Vector3.up;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpChildren();
    }
}
