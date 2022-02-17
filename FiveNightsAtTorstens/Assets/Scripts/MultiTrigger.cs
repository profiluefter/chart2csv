using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/**
 * Triggers an event after multiple other events
 * happened at least once.
 *
 * So you can configure each method call to have a different
 * triggerName and configure all required triggerNames in the
 * Trigger property and after all configured triggerNames have
 * been triggered at least once the Event is fired.
 */
public class MultiTrigger : MonoBehaviour
{
    public List<string> Triggers;
    public UnityEvent Event;

    private List<string> _remainingTriggers;

    private void Awake()
    {
        _remainingTriggers = new List<string>(Triggers);
    }

    public void Trigger(string triggerName)
    {
        if (!_remainingTriggers.Contains(triggerName))
            return;
        _remainingTriggers.Remove(triggerName);
        
        if(_remainingTriggers.Count == 0)
            Event.Invoke();
    }
}
