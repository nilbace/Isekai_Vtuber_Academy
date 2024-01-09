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

        return temp;
    }
}
