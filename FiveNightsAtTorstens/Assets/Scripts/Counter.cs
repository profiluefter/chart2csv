using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Counter : MonoBehaviour {
    public int countTo;
    public UnityEvent onFinish;

    private int _currentCount;
    private bool _finished;

    public void Count() {
        _currentCount++;
        if (!_finished && _currentCount >= countTo) {
            _finished = true;
            onFinish.Invoke();
        }
    }
}
