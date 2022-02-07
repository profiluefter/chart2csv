using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utils;

[RequireComponent(typeof(AudioSource))]
public class ChatController : MonoBehaviour
{
    public GameObject textObject;
    public GameObject sliderObject;
    public List<string> textLines;
    public List<UnityEvent> events;
    public bool playInitialLinesOnAwake = false;
    public int initialLines;
    public float initialDelay = 1;
    public float delay;

    private Stack<TextMessage> _remainingLines;
    private TextMeshPro _textMeshPro;
    private AudioSource _notificationSound;

    private static readonly int UpAnimatorParameter = Animator.StringToHash("up");

    private void Awake()
    {
        _notificationSound = GetComponent<AudioSource>();

        _textMeshPro = textObject.GetComponent<TextMeshPro>();
        _textMeshPro.text = "";

        _remainingLines = new Stack<TextMessage>(
            textLines.Zip(
                events.Pad(textLines.Count),
                (text, @event) => new TextMessage(text, @event)
            ).Reverse()
        );

        if (playInitialLinesOnAwake)
            StartInitial();
    }

    public void StartInitial() => StartCoroutine(nameof(PlayLines));

    public IEnumerator PlayLines()
    {
        sliderObject.GetComponent<Animator>().SetBool(UpAnimatorParameter, true);
        yield return new WaitForSeconds(initialDelay);
        NextLine();
        for (var i = 1; i < initialLines; i++)
        {
            yield return new WaitForSeconds(delay);
            NextLine();
        }
    }

    public void NextLine()
    {
        var message = _remainingLines.Pop();
        _textMeshPro.text += $"{message.Text}\n";
        _notificationSound.Play();
        message.Event?.Invoke();
    }

    private class TextMessage
    {
        public string Text { get; }
        [CanBeNull] public UnityEvent Event { get; }

        public TextMessage(string text, UnityEvent @event)
        {
            Text = text;
            Event = @event;
        }
    }
}
