using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
    public GameObject target;

    public void MoveX() {
        target.transform.position = new Vector2(transform.position.x, target.transform.position.y);
    }
}
