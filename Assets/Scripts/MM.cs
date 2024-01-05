using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using static Define;

public class MM : MonoSingleton<MM>, IPointerClickHandler
{
    public float AniSpeed;
    
    Animator animator;
    
    

    MMState _nowMMState = MMState.usual;

    public MMState NowMMState { get { return _nowMMState; } set { _nowMMState = value; } }

    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = AniSpeed;
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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_nowMMState == MMState.OnSchedule)
        {
            animator.SetTrigger("Push");
            UI_SchedulePopup.instance.ResetSchedule();
        }

        Managers.Sound.Play("MM");
    }
}
