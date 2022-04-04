using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObject : MonoBehaviour {
    public GameObject gameObject;

    public void Enable() {
        gameObject.SetActive(true);
    }
}
