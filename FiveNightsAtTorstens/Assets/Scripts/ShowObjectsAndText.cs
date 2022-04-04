using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Scene = UnityEngine.SceneManagement.Scene;

public class ShowObjectsAndText : MonoBehaviour {
    public string objectTag;
    private List<GameObject> _initialObjects;
    private List<GameObject> _objects;
    private int _objectsPerText;
    public Color objectColorOnFinish;
    
    public List<string> texts;
    public string lastText;
    public TextMeshPro text;
    public GameObject gameObject;
    
    // Start is called before the first frame update
    void Start() {
        _initialObjects = GameObject.FindGameObjectsWithTag(objectTag).ToList();
        _objects = new List<GameObject>(_initialObjects);
        
        _objectsPerText = _objects.Count / (texts.Count+1);
    }

    public void NextObjectsAndText() {
        if (_objects.Count == 0) return;
        
        var objectsPerText = _objectsPerText;
        
        //display text
        if (texts.Count > 0) {
            var textIndex = Random.Range(0, texts.Count);
            text.text = texts[textIndex];
            texts.RemoveAt(textIndex);
        }
        else {
            text.text = lastText;
            
            //set it to show the rest of the objects
            objectsPerText = _objects.Count;
        }
        
        //show objects
        for (var i = 0; i < objectsPerText; i++) {
            var objectIndex = Random.Range(0, _objects.Count);
            _objects[objectIndex].GetComponent<SpriteRenderer>().enabled = true;
            _objects.RemoveAt(objectIndex);
        }

        //finished
        if (_objects.Count == 0) {
            foreach (var o in _initialObjects) {
                o.GetComponent<SpriteRenderer>().color = objectColorOnFinish;
            }
            gameObject.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
