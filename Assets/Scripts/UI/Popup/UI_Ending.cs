using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static Define;

public class UI_Ending : UI_Popup
{
    public EasyTransition.TransitionSettings transition;
    public Sprite[] EndingIMGs;
    public EndingName EndingName;

    enum Buttons
    {
        NextBTN,
        FinishBTN
    }
    enum Texts
    {
        EndingNameTMP,
        EndingIndexTMP,
        EndingTextTMP
    }

    enum Images {
        EndingIMG
    }

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TMPro.TMP_Text>(typeof(Texts));

        StartCoroutine(StartTransition());

        
    }

    IEnumerator StartTransition()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        EasyTransition.TransitionManager.Instance().Transition(transition, 0);

        yield return new WaitForSeconds(1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        GetImage((int)Images.EndingIMG).sprite = EndingIMGs[(int)EndingName];
    }

    public void Setting(EndingName endingName)
    {
        EndingName = endingName;
    }
}
