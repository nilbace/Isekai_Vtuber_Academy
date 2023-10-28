using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MM : MonoBehaviour
{
    Animator animator;
    
    enum MMState { usual, OnSchedule, PushAni}

    MMState NowMMState = MMState.usual;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }

    private void OnMouseDown()
    {
        animator.Play("push");
    }
}
