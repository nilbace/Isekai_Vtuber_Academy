using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MainStoryParser : MonoSingleton<MainStoryParser>
{
    public TextAsset[] Stories;

    public List<Dialogue> Dialogues = new List<Dialogue>();
    private void Awake()
    {
        base.Awake();
    }
 
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

    Dialogue DebugSetence(string asdf)
    {
        Dialogue temp = new Define.Dialogue();
        string[] lines = asdf.Split('\t');

        temp.name = lines[0];


        if (lines[1] == "왼쪽")
            temp.isLeft = true;
        else
            temp.isLeft = false;

        string tempSentence = lines[2];
        string newSentence = "";

        for (int i = 0; i < tempSentence.Length; i++)
        {
            newSentence += tempSentence[i];

            if ((i + 1) % 14 == 0)
            {
                newSentence += "\n";
            }
        }

        temp.sentence = newSentence;
        if (!(lines[0] == "루비아" || lines[0] == "유저" || lines[0] == "뮹뮹" || lines[0] == "나레이션"))
            temp.sentence = lines[2];

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
