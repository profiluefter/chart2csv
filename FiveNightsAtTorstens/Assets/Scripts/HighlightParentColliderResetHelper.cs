using UnityEngine;

[CreateAssetMenu(fileName = "HighlightParentColliderResetHelper", menuName = "HighlightParentColliderResetHelper", order = 0)]
public class HighlightParentColliderResetHelper : ScriptableObject
{
    public void ResetAllStayActive() => HighlightParentCollider.ResetAllStayActive();
}
