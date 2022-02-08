using UnityEngine;
using UnityEngine.Events;

public class TriggerButton : MonoBehaviour
{
    public UnityEvent onClick;
    
    private void OnMouseUpAsButton()
    {
        onClick.Invoke();
    }
}
