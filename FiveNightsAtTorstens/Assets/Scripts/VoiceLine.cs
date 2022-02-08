using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class VoiceLine : MonoBehaviour
{
    public GameObject subtitle;
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
