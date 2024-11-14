using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SubStoryParser : MonoSingleton<SubStoryParser>
{
    public TextAsset[] Stories;

    public List<Dialogue> Dialogues = new List<Dialogue>();
 

    TextAsset LoadSubStory(int storyindex)
    {
        TextAsset text = Resources.Load<TextAsset>($"SubStory/{storyindex}");
        return text;
    }

    public void StartStory(int storyindex)
    {
        string[] lines = LoadSubStory(storyindex).text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            Dialogues.Add(DebugSetence(lines[i]));
        }

        UI_Communication.instance.StartDiagloue(Dialogues);
        UI_Communication.instance.IsWeeklyCommunication = true;
    }

    Dialogue DebugSetence(string asdf)
    {
        Dialogue temp = new Define.Dialogue();
        string[] lines = asdf.Split('\t');

        temp.name = lines[0];

        if (lines[1] == "")
        {
            if (temp.name == "����") temp.Apperance = Apperance.Rubia_Default;
            else if (temp.name == "����") temp.Apperance = Apperance.User_Default;
            else if (temp.name == "����") temp.Apperance = Apperance.MM_Default;
            else temp.Apperance = Apperance.Narration;

        }
        else
            Enum.TryParse(lines[1], out temp.Apperance);


        if (lines[2] == "����")
            temp.isLeft = true;
        else
            temp.isLeft = false;

        temp.sentence = lines[3].ConvertEuroToNewline();

        return temp;
    }
}
