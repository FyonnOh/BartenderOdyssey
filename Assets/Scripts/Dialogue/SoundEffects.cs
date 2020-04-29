using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundEffects
{
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
}

public enum SoundEffectType
{
    Typing
}