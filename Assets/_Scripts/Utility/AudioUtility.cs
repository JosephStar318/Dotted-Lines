using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioUtility
{
    static AudioManager s_AudioManager;

    static AudioUtility()
    {
        s_AudioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    public enum AudioGroups
    {
        Master,
        Music,
        Ambient,
        SFX,
        UI
    }
    public static void ChangeAmbience(AudioClip clip)
    {
        if (s_AudioManager == null) s_AudioManager = GameObject.FindObjectOfType<AudioManager>();

        s_AudioManager.SwapTrack(clip);
    }
    public static void CreateSFX(AudioClip clip, Vector3 position, AudioGroups audioGroup, float spatialBlend,
        float duration = 0, float rolloffDistanceMin = 10f, float rollofDistanceMax = 15f)
    {
        GameObject impactSfxInstance = new GameObject();
        impactSfxInstance.transform.position = position;
        AudioSource source = impactSfxInstance.AddComponent<AudioSource>();
        source.clip = clip;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.spatialBlend = spatialBlend;
        source.minDistance = rolloffDistanceMin;
        source.maxDistance = rollofDistanceMax;
        if(duration == 0)
        {
            source.pitch = 1;
        }
        else
        {
            source.pitch = clip.length / duration;
        }
        source.Play();

        source.outputAudioMixerGroup = GetAudioGroup(audioGroup);

        if (audioGroup == AudioGroups.UI || audioGroup == AudioGroups.Music || audioGroup == AudioGroups.Ambient)
        {
            source.ignoreListenerPause = true;
        }

        TimedSelfDestruct timedSelfDestruct = impactSfxInstance.AddComponent<TimedSelfDestruct>();
        timedSelfDestruct.LifeTime = duration != 0 ? duration : clip.length;
    }
    public static void CreateSFXLoop(AudioClip clip, Vector3 position, AudioGroups audioGroup, float spatialBlend, float period, ref float lastSFXTime,
        float rolloffDistanceMin = 1f)
    {
        if(Time.time - lastSFXTime > period)
        {
            lastSFXTime = Time.time;

            GameObject impactSfxInstance = new GameObject();
            impactSfxInstance.transform.position = position;
            AudioSource source = impactSfxInstance.AddComponent<AudioSource>();
            source.clip = clip;
            source.spatialBlend = spatialBlend;
            source.minDistance = rolloffDistanceMin;
            source.Play();

            source.outputAudioMixerGroup = GetAudioGroup(audioGroup);
            source.maxDistance = 10f;

            TimedSelfDestruct timedSelfDestruct = impactSfxInstance.AddComponent<TimedSelfDestruct>();
            timedSelfDestruct.LifeTime = clip.length;
        }
    }
    public static void CreateSFXLoop(List<AudioClip> clips, Vector3 position, AudioGroups audioGroup, float spatialBlend, float period, ref float lastSFXTime,
       float rolloffDistanceMin = 1f)
    {
        if (Time.time - lastSFXTime > period)
        {
            lastSFXTime = Time.time;

            GameObject impactSfxInstance = new GameObject();
            impactSfxInstance.transform.position = position;
            AudioSource source = impactSfxInstance.AddComponent<AudioSource>();

            source.clip = clips[UnityEngine.Random.Range(0, clips.Count)];
            source.spatialBlend = spatialBlend;
            source.minDistance = rolloffDistanceMin;
            source.Play();

            source.outputAudioMixerGroup = GetAudioGroup(audioGroup);
            source.maxDistance = 10f;

            TimedSelfDestruct timedSelfDestruct = impactSfxInstance.AddComponent<TimedSelfDestruct>();
            timedSelfDestruct.LifeTime = source.clip.length;
        }

    }
    public static void CreateSFXRandom(List<AudioClip> clips, Vector3 position, AudioGroups audioGroup, float spatialBlend,
        float rolloffDistanceMin = 1f)
    {
            GameObject impactSfxInstance = new GameObject();
            impactSfxInstance.transform.position = position;
            AudioSource source = impactSfxInstance.AddComponent<AudioSource>();
            
            source.clip = clips[UnityEngine.Random.Range(0, clips.Count)];
            source.spatialBlend = spatialBlend;
            source.minDistance = rolloffDistanceMin;
            source.Play();

            source.outputAudioMixerGroup = GetAudioGroup(audioGroup);
            source.maxDistance = 10f;

            TimedSelfDestruct timedSelfDestruct = impactSfxInstance.AddComponent<TimedSelfDestruct>();
            timedSelfDestruct.LifeTime = source.clip.length;
    }
    public static AudioMixerGroup GetAudioGroup(AudioGroups group)
    {
        if(s_AudioManager == null) s_AudioManager = GameObject.FindObjectOfType<AudioManager>();

        var groups = s_AudioManager.FindMatchingGroups(group.ToString());

        if (groups.Length > 0)
            return groups[0];

        Debug.LogWarning("Didn't find audio group for " + group.ToString());
        return null;
    }
    public static void SetMasterVolume(float value)
    {
        if (s_AudioManager == null) s_AudioManager = GameObject.FindObjectOfType<AudioManager>();

        float clampedVal = Mathf.InverseLerp(100, 0, value);
        clampedVal *= -80f;

        s_AudioManager.SetFloat("Master", clampedVal);
    }
    public static void SetVolume(AudioGroups audioGroup, float value)
    {
        if (s_AudioManager == null) s_AudioManager = GameObject.FindObjectOfType<AudioManager>();
        AudioMixerGroup group = GetAudioGroup(audioGroup);

        float clampedVal = Mathf.InverseLerp(100, 0, value);
        clampedVal *= -80f;

        s_AudioManager.SetFloat(group.ToString(), clampedVal);
    }
    public static float GetVolume(AudioGroups audioGroup)
    {
        if (s_AudioManager == null) s_AudioManager = GameObject.FindObjectOfType<AudioManager>();
        AudioMixerGroup group = GetAudioGroup(audioGroup);

        s_AudioManager.GetFloat(group.ToString(), out var val);
        float transformedVal = 100 - ((val / -80f) * 100f);
        return transformedVal;
    }
}
