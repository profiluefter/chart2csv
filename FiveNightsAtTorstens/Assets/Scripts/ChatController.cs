using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utils;

/**
 * Only used for the Chat prefab
 *
 * Modifies the text box and plays the sliding animation
 */
[RequireComponent(typeof(AudioSource))]
public class ChatController : MonoBehaviour
{
    /**
     * Object containing the TextMeshPro component
     * Shouldn't be changed
     */
    public GameObject textObject;
    /**
     * Object containing the sliding Animation
     * Shouldn't be changed
     */
    public GameObject sliderObject;
    /**
     * List of all Text Lines in the order they appear
     */
    public List<string> textLines;
    /**
     * List of Events that correspond to a text line
     * Not each text line needs to have an event but you have
     * to insert all previous events leading up to the event you
     * actually want because the index has to match
     */
    public List<UnityEvent> events;
    /**
     * If this is checked then the number of lines specified in
     * initialLines is played back after the object is loaded
     */
    public bool playInitialLinesOnAwake = false;
    /**
     * Number of lines to play after the object is loaded
     * if playInitialLinesOnAwake is enabled
     */
    public int initialLines;
    /**
     * Delay before the first message in seconds
     */
    public float initialDelay = 1;
    /**
     * Delay between each message in seconds
     */
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
