using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MM : MonoSingleton<MM>
{
    public float AniSpeed;
    public TextAsset SmallTalkTextAsset;
    public TMPro.TMP_Text MMTalkTMP;
    List<string> MMSmallTalkList = new List<string>();

    Coroutine ResetTalkCor;
    
    Animator animator;
    
    

    MMState _nowMMState = MMState.usual;

    public MMState NowMMState { get { return _nowMMState; } set { _nowMMState = value; } }

    private void Awake()
    {
        base.Awake();
        string[] talks = SmallTalkTextAsset.text.Split('\n');
        foreach(string talk in talks)
        {
            MMSmallTalkList.Add(talk);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = AniSpeed;
        TalkSomething(_nowMMState);
    }

    void TalkSomething(MMState mMState)
    {
        if(mMState == MMState.usual)
        {
            MMTalkTMP.text = MMSmallTalkList[Random.Range(0, MMSmallTalkList.Count)];
        }
        else if(mMState == MMState.OnSchedule)
        {
            MMTalkTMP.text = "일정은 내가 확실히 기억해주겠다뮹!\n절대 머리 위의 버튼을 누르지 말라뮹";
        }
    }

    public void SetState(MMState mmState)
    {
        if(mmState == MMState.usual)
        {
            animator.SetTrigger("Hat");
            _nowMMState = MMState.usual;
        }
        else if(mmState == MMState.OnSchedule)
        {
            animator.SetTrigger("BTN");
            _nowMMState = MMState.OnSchedule;
        }
        TalkSomething(mmState);
    }



    private void OnMouseDown()
    {
        if (_nowMMState == MMState.OnSchedule)
        {
            animator.SetTrigger("Push");
            Managers.Sound.Play("MM");
            UI_SchedulePopup.instance.ResetSchedule();
            if (ResetTalkCor != null) StopCoroutine(ResetTalkCor);
            ResetTalkCor = StartCoroutine(ForgetTalk());
        }
    }

    IEnumerator ForgetTalk()
    {
        MMTalkTMP.text = "<size=21.5>앗...</size> 까먹었다뮹...";
        yield return new WaitForSeconds(2f);
        TalkSomething(_nowMMState);
    }
}
