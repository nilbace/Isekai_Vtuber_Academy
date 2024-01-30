using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Photo : UI_Base
{
    public Button CoverBTN;
    public Image BaseImage;
    public Sprite[] BroadCastImgs;
    public Sprite[] RestImgs;
    public Sprite[] GoOutImgs;
    public Sprite[] EndingImgs;

    


    void Start()
    {
        Init();
    }

    public override void Init() {
        
    }

    public void Set(object TaskEnum)
    {
        if (TaskEnum is EndingName)
        {
        }
        else if(TaskEnum is BroadCastType)
        {
            BaseImage.sprite = BroadCastImgs[(int)TaskEnum];

            if (Managers.Data.PersistentUser.WatchedBroadCast.Contains((BroadCastType)TaskEnum))
            {
                CoverBTN.gameObject.SetActive(false);
            }

            BaseImage.GetComponent<Button>().onClick.AddListener(()=> ShowBcPopup((BroadCastType)TaskEnum));
        }
        else if(TaskEnum is RestType)
        {
            BaseImage.sprite = RestImgs[(int)TaskEnum -9];
            if (Managers.Data.PersistentUser.WatchedRest.Contains((RestType)(int)TaskEnum - 9))
            {
                CoverBTN.gameObject.SetActive(false);
            }

            BaseImage.GetComponent<Button>().onClick.AddListener(() => ShowBcPopup((RestType)(int)TaskEnum - 9));
        }
        else if(TaskEnum is GoOutType)
        {
            BaseImage.sprite = GoOutImgs[(int)TaskEnum -15];
            if (Managers.Data.PersistentUser.WatchedGoOut.Contains((GoOutType)(int)TaskEnum - 15))
            {
                CoverBTN.gameObject.SetActive(false);
            }

            BaseImage.GetComponent<Button>().onClick.AddListener(() => ShowBcPopup((GoOutType)(int)TaskEnum - 15));
        }
    }

    void ShowBcPopup(object broadCastType)
    {
        Managers.Sound.Play(Sound.SmallBTN);
        UI_Ar_BC_Popup.tasktype = broadCastType;
        Managers.UI_Manager.ShowPopupUI<UI_Ar_BC_Popup>();
    }
}
