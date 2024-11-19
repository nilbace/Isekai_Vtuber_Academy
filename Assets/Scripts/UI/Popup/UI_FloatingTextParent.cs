using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using System.Linq;
using System;

public class UI_FloatingTextParent : MonoSingleton<UI_FloatingTextParent>
{
    private PlusText[] _texts;
    public static StatName[] statOrders = new StatName[] { StatName.Heart, StatName.Star, StatName.Gold, StatName.Sub, StatName.Game, StatName.Song, StatName.Draw, StatName.Strength, StatName.Mental, StatName.Luck };

    private void Start()
    {
        _texts = GetComponentsInChildren<PlusText>();
    }
    public PlusText GetText(StatName name)
    {
        // Find the index of the StatName in statOrders
        int index = Array.FindIndex(statOrders, x => x == name);

        // Validate the index
        if (index >= 0 && index < _texts.Length)
        {
            return _texts[index];
        }

        // Return null or handle the case where the StatName is not found or index is invalid
        Debug.LogWarning($"StatName {name} not found or out of bounds!");
        return null;
    }

    [ContextMenu("테스트")]
    private void Test()
    {
        // 테스트용 데이터 생성
        List<(StatName stat, float value)> testStats = new List<(StatName stat, float value)>();

        // statOrders를 순회하며 각 스탯과 값을 추가
        foreach (var stat in statOrders)
        {
            testStats.Add((stat, UnityEngine.Random.Range(-20,20))); // 값은 +10
        }

        // ChangeStatAndPlayUIAnimation 호출
        Managers.Data.PlayerData.ChangeStatAndPlayUIAnimation(testStats);
    }
}
