using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpriteManager
{
    public Sprite[] MiniStatIcons;
    public Sprite[] Days;
    public Sprite[] DaysPannel;

    public void Init()
    {
        MiniStatIcons = Resources.LoadAll<Sprite>("MiniStatIcons");
        Days = Resources.LoadAll<Sprite>("Days");
        DaysPannel = Resources.LoadAll<Sprite>("DaysPannel");
    }
}
