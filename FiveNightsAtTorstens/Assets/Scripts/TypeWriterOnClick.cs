using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshPro))]
public class TypeWriterOnClick : MonoBehaviour
{
    public float delay = 0.125f;
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
