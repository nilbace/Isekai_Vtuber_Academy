using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Ending : UI_Popup
{
    public float MoveTime;
    public float TargetY;
    public float ElasticScale;
    public float PeriodScale;

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

        transform.DOLocalMoveY(TargetY, MoveTime).SetEase(Ease.OutElastic, ElasticScale, PeriodScale);
    }
}
