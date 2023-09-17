using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerprefsHelper
{
    public static readonly string SoundVolume = "SoundVolume";

    public static int GetSoundState()
    {
        return PlayerPrefs.GetInt(SoundVolume, 100);
    }

    public static void SetSoundState(int value)
    {
        PlayerPrefs.SetInt(SoundVolume, value);
    }
}
