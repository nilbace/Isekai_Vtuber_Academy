using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Inst { get; private set; }
    protected virtual void Awake() => Inst = FindObjectOfType(typeof(T)) as T;
}