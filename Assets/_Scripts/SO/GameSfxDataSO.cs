using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Game SFX Data")]
public class GameSfxDataSO : ScriptableObject
{
    public List<AudioClip> popClips = new List<AudioClip>();
    public AudioClip scoreClip;
    public AudioClip failClip;
}
