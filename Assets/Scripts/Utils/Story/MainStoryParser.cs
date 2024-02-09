using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MainStoryParser : MonoSingleton<MainStoryParser>
{
    public TextAsset[] Stories;

    public List<Dialogue> Dialogues = new List<Dialogue>();
  
     TextAsset LoadMainStory(MainStory main)
    {
        TextAsset text = Resources.Load<TextAsset>($"MainStory/{main}");
        return text;
    }
  
    public void StartStory(MainStory mainStory)
    {
        string[] lines = LoadMainStory(mainStory).text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            Dialogues.Add(DebugSetence(lines[i]));
        }

        UI_Communication.instance.StartDiagloue(Dialogues);
        UI_Communication.instance.HideSkipBTN();
    }

    Dialogue DebugSetence(string dialogue)
    {
        Dialogue temp = new Define.Dialogue();
        string[] lines = dialogue.Split('\t');

        temp.name = lines[0];


        if (lines[1] == "¿ÞÂÊ")
            temp.isLeft = true;
        else
            temp.isLeft = false;

        temp.sentence = lines[2].ConvertEuroToNewline();
        

        int tempGold;
        StatName tempStatName;
        for(int i = 3; i<lines.Length;i++)
        {
            if (lines[i] == "") continue;
            else if (int.TryParse(lines[i], out tempGold)) temp.CostGold = tempGold;
            else if (Enum.TryParse(lines[i], out tempStatName))
            {
                RewardStat tempRewardStat = new RewardStat();
                tempRewardStat.StatName = tempStatName;
                tempRewardStat.Value = int.Parse(lines[i + 1]);
                temp.rewardStats.Add(tempRewardStat);
                i++;
            }
        }

        return temp;
    }

}
