using UnityEngine;

/**
 * Plays the animation component associated with the
 * object that this component is applied to.
 *
 * This is done because you can't directly trigger an
 * animation from an unity event handler so you have to
 * relay it through this component.
 */
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
