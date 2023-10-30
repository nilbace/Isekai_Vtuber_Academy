using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM : MonoBehaviour
{
    public static MM instance;

    
    Animator animator;
    
    public enum MMState { usual, OnSchedule, PushAni}

    MMState _nowMMState = MMState.usual;

    public MMState NowMMState { get { return _nowMMState; } set { _nowMMState = value; } }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        if(_nowMMState == MMState.OnSchedule)
        {
            animator.Play("push");
            UI_SchedulePopup.instance.ResetSchedule();
        }

        Managers.Sound.Play("power");
    }

    
}
