using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class TutorialParser : MonoSingleton<TutorialParser>
{
    public TextAsset TutorialAsset;

    public List<Dialogue> Dialogues = new List<Dialogue>();

   

    private void Start()
    {
        string[] lines = TutorialAsset.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            Dialogues.Add(DebugSetence(lines[i]));
        }

        UI_Tutorial.instance.StartDiagloue(Dialogues);
    }


    Dialogue DebugSetence(string dialogue)
    {
        Dialogue temp = new Dialogue();
        string[] lines = dialogue.Split('\t');

        //이름
        temp.name = lines[0];

        if (temp.name == "루비아") temp.Apperance = Apperance.Rubia_Default;
        else if (temp.name == "유저") temp.Apperance = Apperance.User_Default;
        else if (temp.name == "뮹뮹") temp.Apperance = Apperance.MM_Default;
        else temp.Apperance = Apperance.Narration;

        //왼쪽 오른쪽
        if (lines[1] == "왼쪽")
            temp.isLeft = true;
        else
            temp.isLeft = false;

        //배경 집중 위치
        if (lines[2] == "") temp.tutorialFocus = TutorialFocusPoint.MaxCount;
        else Enum.TryParse(lines[2], out temp.tutorialFocus);

        if (lines[3] == "TRUE") temp.IsInteractable = true;

        //대화 하나
        temp.sentence = lines[4].ConvertEuroToNewline();

        temp.Ypoz = lines[5].Trim();
        
        return temp;
    }
}
