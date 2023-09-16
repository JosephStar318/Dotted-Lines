using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer[] AudioMixers;

    private AudioSource track1, track2;
    private bool isPlayingTrack1;

    private void Start()
    {
        track1 = gameObject.AddComponent<AudioSource>();    
        track2 = gameObject.AddComponent<AudioSource>();
        track1.loop = true;
        track2.loop = true;
        track1.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.Ambient);
        track2.outputAudioMixerGroup = AudioUtility.GetAudioGroup(AudioUtility.AudioGroups.Ambient);

        isPlayingTrack1 = true;
        AudioListener.pause = false;
    }

    public void SwapTrack(AudioClip newClip)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTrack(newClip));

        isPlayingTrack1 = !isPlayingTrack1;
    }

    private IEnumerator FadeTrack(AudioClip newClip)
    {
        float timeToFade = 5f;
        float timeElapsed = 0;

        if (isPlayingTrack1)
        {
            if (track1.clip == newClip) yield return null;
            track2.clip = newClip;
            track2.Play();

            while (timeElapsed < timeToFade)
            {
                track2.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track1.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            track1.Stop();
        }
        else
        {
            if (track2.clip == newClip) yield return null;
            track1.clip = newClip;
            track1.Play();

            while (timeElapsed < timeToFade)
            {
                track1.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track2.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            track2.Stop();
        }
    }
    public AudioMixerGroup[] FindMatchingGroups(string subPath)
    {
        for (int i = 0; i < AudioMixers.Length; i++)
        {
            AudioMixerGroup[] results = AudioMixers[i].FindMatchingGroups(subPath);
            if (results != null && results.Length != 0)
            {
                return results;
            }
        }

        return null;
    }

    public void SetFloat(string name, float value)
    {
        for (int i = 0; i < AudioMixers.Length; i++)
        {
            if (AudioMixers[i] != null)
            {
                AudioMixers[i].SetFloat(name, value);
            }
        }
    }

    public void GetFloat(string name, out float value)
    {
        value = 0f;
        for (int i = 0; i < AudioMixers.Length; i++)
        {
            if (AudioMixers[i] != null)
            {
                AudioMixers[i].GetFloat(name, out value);
                break;
            }
        }
    }
}
