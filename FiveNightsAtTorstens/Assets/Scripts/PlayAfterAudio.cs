using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayAfterAudio : MonoBehaviour
{
    public AudioSource playAfter;
    public double delay;

    private void Start()
    {
        if (playAfter != null)
            playAfter.PlayScheduled(AudioSettings.dspTime + GetComponent<AudioSource>().clip.length + delay);
    }
}
