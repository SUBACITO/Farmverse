using UnityEngine;
using UnityEngine.UI;

public class AutoScrollToBottom : MonoBehaviour
{
    public ScrollRect scrollRect;

    // This method should be called whenever a new child is added
    public void OnChildAdded()
    {
        // Ensure the scrollRect is assigned
        if (scrollRect != null)
        {
            // Force the content to update its size
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

            // Scroll to the bottom
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
