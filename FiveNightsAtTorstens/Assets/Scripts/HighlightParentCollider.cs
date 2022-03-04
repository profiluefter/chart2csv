using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HighlightParentCollider : MonoBehaviour
{
    public bool StayAfterClick;

    private LineRenderer _line;
    private bool _stayActive;
    private static readonly List<HighlightParentCollider> _stayActiveHighlighters = new List<HighlightParentCollider>();
    private static bool _stayActiveLocked;

    void Start()
    {
        if (!transform.parent.TryGetComponent<BoxCollider2D>(out var boxCollider))
        {
            Debug.LogError("Parent doesn't have BoxCollider2D!", gameObject);
            enabled = false;
            return;
        }
        var size = boxCollider.size;
        var offset = boxCollider.offset;
        _line = GetComponent<LineRenderer>();
        _line.SetPositions(new[]
        {
            new Vector3(size.x / 2 + offset.x, size.y / 2 + offset.y, 0),
            new Vector3(size.x / 2 + offset.x, -size.y / 2 + offset.y, 0),
            new Vector3(-size.x / 2 + offset.x, -size.y / 2 + offset.y, 0),
            new Vector3(-size.x / 2 + offset.x, size.y / 2 + offset.y, 0)
        });
        if (StayAfterClick)
            AttachEventListener();
    }

    public static void ResetAllStayActive()
    {
        _stayActiveHighlighters.ToList().ForEach(x => x.ResetStayActive());
        _stayActiveLocked = true;

    }

    public void ResetStayActive()
    {
        _stayActive = false;
        _stayActiveHighlighters.Remove(this);
    }

    private void AttachEventListener()
    {
        if (!transform.parent.TryGetComponent<TriggerButton>(out var triggerButton))
        {
            Debug.LogError(
                "Please add a TriggerButton script to the object with the collider that should be highlighted",
                gameObject);
            return;
        }

        triggerButton.onClick.AddListener(() =>
        {
            if (_stayActiveLocked) return;
            _stayActive = true;
            _stayActiveHighlighters.Add(this);
        });
    }

    private void Update()
    {
        _stayActiveLocked = false;
        _line.enabled = _stayActive ||
                        Input.GetKey(KeyCode.LeftControl) ||
                        Input.GetKey(KeyCode.LeftAlt) ||
                        Input.GetKey(KeyCode.Space);
    }
}
