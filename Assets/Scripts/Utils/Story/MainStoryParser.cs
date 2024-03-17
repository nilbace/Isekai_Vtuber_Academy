using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MainStoryParser : MonoSingleton<MainStoryParser>
{
    public TextAsset[] Stories;

    public List<Dialogue> Dialogues = new List<Dialogue>();
  
     TextAsset LoadMainStory(string Name)
    {
        Debug.Log(Name);
        TextAsset text = Resources.Load<TextAsset>($"MainStory/{Name}");
        return text;
    }
  
    public void StartStory(int mainStoryIndex)
    {
        string[] lines = LoadMainStory(mainStoryIndex.ToString()).text.Split('\n');
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

        if(lines[1] == "")
        {
            if (temp.name == "루비아") temp.Apperance = Apperance.Rubia_Default;
            else if (temp.name == "유저") temp.Apperance = Apperance.User_Default;
            else if (temp.name == "뮹뮹") temp.Apperance = Apperance.MM_Default;
            else temp.Apperance = Apperance.Narration;

        }
        else
            Enum.TryParse(lines[1], out temp.Apperance);

        if (lines[2] == "왼쪽")
            temp.isLeft = true;
        else
            temp.isLeft = false;

        temp.sentence = lines[3].ConvertEuroToNewline();
        

        int tempInt;
        StatName tempStatName;
        for(int i = 4; i<lines.Length;i++)
        {
            if (lines[i] == "") continue;
            else if (int.TryParse(lines[i], out tempInt))
            {
                //500원 보다 비싸면 필요한 골드
                if (tempInt > 500)
                    temp.CostGold = tempInt;
                //아니라면 다음 스토리 인덱스임
                else
                    temp.NextDialogueIndex = tempInt;
            }

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
