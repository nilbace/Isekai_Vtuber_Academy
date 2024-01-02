using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Ending : UI_Popup
{
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

        
    }
}
