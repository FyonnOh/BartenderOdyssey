using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundEffects
{
    public static string BgmNormal { get{return "chilled-to-zero-intro-loop";} }
    public static string BgmHappy { get{return "bensound-slowmotion";} }
    public static string BgmSad { get{return "bensound-tomorrow";} }

    public static string[] TypingClips
    {
        get
        {
            return new string[] 
            {
                "Keyboard-Typing-03",
                "Keyboard-Typing-05"
            };
        }
    }

    public static string[] TalkingClips
    {
        get
        {
            return new string[] 
            {
                "Talking-01",
                "Talking-02",
                "Talking-03",
                "Talking-04",
                "Talking-05",
                "Talking-06",
                "Talking-07",
                "Talking-08",
            };
        }
    }
}

public enum SoundEffectType
{
    Typing,
    Talking,
    Normal,
    Happy,
    Sad
}