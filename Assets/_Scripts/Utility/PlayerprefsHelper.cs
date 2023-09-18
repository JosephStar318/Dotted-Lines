using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerprefsHelper
{
    public static readonly string SoundVolume = "SoundVolume";
    public static readonly string HighScore = "HighScore";

    public static int GetSoundState()
    {
        return PlayerPrefs.GetInt(SoundVolume, 100);
    }
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(HighScore, 0);
    }


    public static void SetSoundState(int value)
    {
        PlayerPrefs.SetInt(SoundVolume, value);
    }
    public static void SetHighScore(int value)
    {
        PlayerPrefs.SetInt(HighScore, value);
    }
}
