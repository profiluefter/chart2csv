using System;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class PlayDefaultAnimation : MonoBehaviour
{
    private Animation _animation;

    private void Start()
    {
        _animation = GetComponent<Animation>();
    }

    public void PlayAnimation()
    {
        _animation.Play();
    }
}
