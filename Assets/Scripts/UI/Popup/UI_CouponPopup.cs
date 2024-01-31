using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class GoogleData
{
    public string order, result, msg;
}

public class UI_CouponPopup : UI_Popup
{
    public TMP_InputField inputField;
    const string URL = "https://script.google.com/macros/s/AKfycbxPZ4qdF8K8ywrbbE-x6Ya3-lKfk3rXlKZNqp2yvlrxemDHJ2hLNYCEO_Uv8XfXHVLx/exec";
    public GoogleData GD;
    string number;

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

	bool SetCouponNumber()
	{
		number = inputField.text.Trim();

		if (number == "") return false;
		else return true;
	}


	public void UseCoupon()
	{
		if (!SetCouponNumber())
		{
			print("아이디 또는 비밀번호가 비어있습니다");
			return;
		}

		WWWForm form = new WWWForm();
		form.AddField("number", number);

		StartCoroutine(Post(form));
	}


	IEnumerator Post(WWWForm form)
	{
		using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) 
		{
			yield return www.SendWebRequest();

			if (www.isDone) Response(www.downloadHandler.text);
			else print("웹의 응답이 없습니다.");
		}
	}

	void Response(string json)
	{
		if (string.IsNullOrEmpty(json)) return;
		Debug.Log(json);
		GD = JsonUtility.FromJson<GoogleData>(json);

		if (GD.result == "ERROR")
		{
			print(GD.order + "을 실행할 수 없습니다. 에러 메시지 : " + GD.msg);
			return;
		}

		print(GD.order + "을 실행했습니다. 메시지 : " + GD.msg);

	}

}
