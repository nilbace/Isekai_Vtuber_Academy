using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CouponPopup : MonoBehaviour
{
    public TMP_InputField inputField;

    private const int maxInputLength = 12;
    private const int insertDashAt1 = 3;
    private const int insertDashAt2 = 7;

    private void Start()
    {
        inputField.characterLimit = maxInputLength;
    }

  
}
