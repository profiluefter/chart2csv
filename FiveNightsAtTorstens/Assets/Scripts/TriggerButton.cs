using UnityEngine;
using UnityEngine.Events;

/**
 * Executes an event when a trigger of the object
 * that this component applies to is clicked.
 */
public class TriggerButton : MonoBehaviour
{
    public UnityEvent onClick;
    
    private void OnMouseUpAsButton()
    {
        onClick.Invoke();
    }
}
