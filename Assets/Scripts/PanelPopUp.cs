using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelPopUp : MonoBehaviour
{
    public GameObject shopPanel;  // Reference to the shop panel
    public float scaleDuration = 0.5f;  // Duration of the scale effect

    private Vector3 initialScale = Vector3.zero;
    private Vector3 targetScale = Vector3.one;

    void Start()
    {
        // Initialize the shop panel scale to zero
        shopPanel.transform.localScale = initialScale;
        shopPanel.SetActive(false);  // Start with the shop panel hidden
    }

    // Function to open the shop
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        StartCoroutine(ScaleUI(shopPanel, initialScale, targetScale, scaleDuration));
    }

    // Function to close the shop
    public void CloseShop()
    {
        StartCoroutine(ScaleUI(shopPanel, targetScale, initialScale, scaleDuration, () => shopPanel.SetActive(false)));
    }

    // Coroutine for scaling the UI
    private System.Collections.IEnumerator ScaleUI(GameObject target, Vector3 fromScale, Vector3 toScale, float duration, System.Action onComplete = null)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            target.transform.localScale = Vector3.Lerp(fromScale, toScale, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        target.transform.localScale = toScale;

        // Call the onComplete action if provided
        onComplete?.Invoke();
    }
}
