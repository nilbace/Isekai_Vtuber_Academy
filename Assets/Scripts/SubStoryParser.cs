using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SubStoryParser : MonoSingleton<SubStoryParser>
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
        int index = 0;
        string[] lines = LoadMainStory(mainStory).text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            Dialogues.Add(DebugSetence(lines[i]));
            index++;
        }

        UI_Communication.instance.StartDiagloue(Dialogues);
    }

    Dialogue DebugSetence(string asdf)
    {
        Dialogue temp = new Define.Dialogue();
        string[] lines = asdf.Split('\t');

        temp.name = lines[0];


        if (lines[1] == "¿ÞÂÊ")
            temp.isLeft = true;
        else
            temp.isLeft = false;

        temp.sentence = lines[2];

        return temp;
    }
}
