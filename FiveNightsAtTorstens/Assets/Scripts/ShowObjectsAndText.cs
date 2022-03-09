using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShowObjectsAndText : MonoBehaviour {
    private List<GameObject> _initialObjects;
    public string objectTag;
    private List<GameObject> _objects;
    private int _objectsPerText;
    public Color objectColorOnFinish;
    
    public List<string> texts;
    private List<string> _texts;
    public string lastText;
    public TextMeshPro text;
    
    // Start is called before the first frame update
    void Start() {
        _initialObjects = GameObject.FindGameObjectsWithTag(objectTag).ToList();
        _objects = new List<GameObject>(_initialObjects);

        _texts = new List<string>(texts);
        
        _objectsPerText = _objects.Count / (_texts.Count+1);
    }

    public void NextObjectsAndText() {
        var objectsPerText = _objectsPerText;
        
        if (_texts.Count == 0 || _objects.Count == 0) {
            text.text = lastText;
            objectsPerText = _objects.Count;
            
            foreach (var o in _initialObjects) {
                o.GetComponent<SpriteRenderer>().color = objectColorOnFinish;
            }
        }
        else {
            var textIndex = Random.Range(0, _texts.Count);
            text.text = _texts[textIndex];
            _texts.RemoveAt(textIndex);
        }
        
        for (var i = 0; i < objectsPerText; i++) {
            var objectIndex = Random.Range(0, _objects.Count);
            _objects[objectIndex].GetComponent<SpriteRenderer>().enabled = true;
            _objects.RemoveAt(objectIndex);
        }
    }
}
