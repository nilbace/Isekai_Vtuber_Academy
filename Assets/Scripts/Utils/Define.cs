using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define : MonoBehaviour
{

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }
    public enum UIEvent
    {
        Click,
        Drag,
        
    }
    public enum MouseEvent{
        Press,
        Click,
    }
    public enum CameraMode{
        QuaterView,
    }

    public enum StatName {
        Game, Song, Chat, Health, Mental, Luck, FALSE, Subs, Week
    }

    public enum EventDataType {
        Main, Random, Conditioned
    }

    public struct Item
    {
        public string ItemName;
        public int Cost;
        public int[] SixStats;
        public string ItemImageName;
        public int EntWeek;

        public Item(string itemName = "", int cost = 0, string itemImageName = "", int entWeek = 0)
            : this(itemName, cost, itemImageName, entWeek, new int[6])
        {
        }

        public Item(string itemName, int cost, string itemImageName, int entWeek, int[] sixStats)
        {
            ItemName = itemName;
            Cost = cost;
            SixStats = sixStats;
            ItemImageName = itemImageName;
            EntWeek = entWeek;
        }
    }

    public struct Bonus
    {
        public int SubBonus;
        public int IncomeBonus;
    }


}
