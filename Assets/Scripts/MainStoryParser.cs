using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainStoryParser : MonoSingleton<MainStoryParser>
{
    public TextAsset[] Stories;
    private void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        ParseStory(0);
    }
  
    void ParseStory(int n)
    {
        string[] lines = Stories[n].text.Split('\n');
        foreach (string line in lines)
        {
            DebugSetence(line);
        }
    }

    void DebugSetence(string asdf)
    {
        string[] lines = asdf.Split('\t');
        foreach (string line in lines)
        {
            if (line == "") continue;
            Debug.Log(line);
        }
    }
}
