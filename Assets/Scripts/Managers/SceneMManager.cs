using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SceneMManager
{
    public void Init()
    {
        Managers.Sound.Play("bgm1", Sound.Bgm);
#if UNITY_EDITOR
        //
#else
        Managers.Resource.Instantiate("LoginTitle");
#endif
    }
}
