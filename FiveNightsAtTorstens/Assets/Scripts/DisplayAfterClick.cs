using UnityEngine;

/**
 * When the collider of the object that has this
 * component is clicked then the state of the
 * object in `target` is toggled.
 * Additionally the object in `after` is displayed
 * after the collider was clicked at least once.
 *
 * Very specific and probably not very reusable.
 */
public class DisplayAfterClick : MonoBehaviour
{
    public GameObject target;
    public GameObject after;

    private void Awake()
    {
        target.SetActive(false);
        after.SetActive(false);
    }

    private void OnMouseUpAsButton()
    {
        if(target.activeSelf)
            after.SetActive(target.activeSelf);
        target.SetActive(!target.activeSelf);
    }
}
