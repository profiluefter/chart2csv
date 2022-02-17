using UnityEngine;
using UnityEngine.Events;

/**
 * Displays a corresponding subtitle object while an audio
 * source is playing and triggers an event after the audio
 * source finished playing.
 */
[RequireComponent(typeof(AudioSource))]
public class VoiceLine : MonoBehaviour
{
    /**
     * GameObject to display while the audio source is playing
     */
    public GameObject subtitle;
    /**
     * Event that fires after the audio source finished playing
     */
    public UnityEvent lineFinished;
    private AudioSource _audioSource;
    private bool _playing;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        var previouslyPlaying = _playing;
        var nowPlaying = _audioSource.time > 0;

        if(previouslyPlaying && !nowPlaying)
            lineFinished.Invoke();
        
        subtitle.SetActive(nowPlaying);
        _playing = nowPlaying;
    }
}
