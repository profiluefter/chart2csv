using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShowObjectsWithTag : MonoBehaviour
{

    public string Tag;
    private List<GameObject> _gameObjects;
    public GameObject chat;
    

    // Start is called before the first frame update
    void Start()
    {
        _gameObjects = GameObject.FindGameObjectsWithTag(Tag).ToList();
    }

    public void ShowRandom()
    {
        for (int i = 0; i < 2; i++)
        {
            if(_gameObjects.Where(x => !x.GetComponent<SpriteRenderer>().enabled).ToList().Count > 0)
            {
                int index = Random.Range(0,
                    _gameObjects.Where(x => !x.GetComponent<SpriteRenderer>().enabled).ToList().Count - 1);
                _gameObjects.Where(x => !x.GetComponent<SpriteRenderer>().enabled)
                    .ToList()[index]
                    .GetComponent<SpriteRenderer>()
                    .enabled = true;
            }
            else
            {
                chat.GetComponent<ChatController>().StartInitial();
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            } 
        }
        
    }
}
