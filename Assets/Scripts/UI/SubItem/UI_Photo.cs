using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class UI_Photo : UI_Base
{
    public Button CoverBTN;
    public Image BaseImage;
    public Sprite[] BroadCastAnis;
    public Sprite[] RestAnis;
    public Sprite[] GoOutAnis;
    public Sprite[] EndingImgs;

    public override void Init() { }


    void Start()
    {
        Init();
        Set(EndingName.DrawFail);
    }

    public void Set(object TaskEnum)
    {
        if (TaskEnum is EndingName)
        {
            Debug.Log("yes");
        }
        else if(TaskEnum is BroadCastType)
        {

        }
        else if(TaskEnum is RestType)
        {

        }
        else if(TaskEnum is GoOutType)
        {

        }
    }
}
