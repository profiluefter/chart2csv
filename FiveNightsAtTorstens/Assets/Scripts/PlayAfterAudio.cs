using UnityEngine;

/**
 * Chains Audio Sources together
 *
 * Plays another `Audio Source` component after the one
 * on the object with this component has finished and an
 * additional delay.
 *
 * This component currently only works if the object with
 * this component immediately starts playing at the start
 * of the scene.
 */
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
