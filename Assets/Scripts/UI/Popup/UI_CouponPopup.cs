using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CouponPopup : UI_Popup
{
    public TMP_InputField inputField;


    enum Buttons
    {
        CloseBTN, ResultBTN
    }

    private void Start()
    {
        base.Init();
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.CloseBTN).onClick.AddListener(CloseBTN);

    }

  
}
