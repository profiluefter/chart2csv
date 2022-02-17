using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/**
 * Executes a typewriter-style animation after the collider
 * of the object that this component applies to is clicked.
 *
 * The object has to have a TextMeshPro component and the
 * text content of that component is used as text for the
 * effect.
 */
[RequireComponent(typeof(TextMeshPro))]
public class TypeWriterOnClick : MonoBehaviour
{
    /**
     * Delay between the different characters appearing
     */
    public float delay = 0.125f;
    /**
     * This event is executed after the whole text of this
     * component has been written
     */
    public UnityEvent finished;

    private string _text;
    private TextMeshPro _textMeshPro;

    private void Start()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
        _text = _textMeshPro.text;
        _textMeshPro.text = "";
    }

    private void OnMouseUpAsButton()
    {
        StartCoroutine(nameof(PlayText));
    }

    private IEnumerator PlayText()
    {
        foreach (var character in _text)
        {
            _textMeshPro.text += character;
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(delay * 2);
        finished.Invoke();
    }
}
