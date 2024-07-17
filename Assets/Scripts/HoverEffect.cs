using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 normalSize = new Vector3(1f, 1f, 1f);
    public Vector3 enlargedSize = new Vector3(1.1f, 1.1f, 1.1f); // Increased size by 20%
    public float smoothTime = 0.1f; // Time in seconds for the transition

    private Vector3 targetScale;
    private Vector3 velocity = Vector3.zero; // Needed for smooth damping

    void Start()
    {
        targetScale = normalSize; // Initial target scale is the normal size
        transform.localScale = normalSize; // Set initial scale
    }

    void Update()
    {
        // Smoothly scale the transform to the target scale using smooth damping
        transform.localScale = Vector3.SmoothDamp(transform.localScale, targetScale, ref velocity, smoothTime);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = enlargedSize; // Change target scale to enlarged size on hover
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = normalSize; // Change target scale to normal size when not hovered
    }
}
