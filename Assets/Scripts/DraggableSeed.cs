using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableSeed : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Plant information")]

    public TreeInformation treeInformation;
    public TreeInformation TreeInformation => treeInformation;
    public Text AmountText;
    public GameObject TreePrefabs;
    private Vector3 startPosition;
    private Transform startParent;
    private Canvas canvas;
    [SerializeField] private RectTransform parentRectTransform;
    private bool isOutsideParentRect = false;

    private Renderer groundRenderer;
    private Material originalMaterial;
    public Material hoverMaterial; // Assign this in the Inspector
    public Material cantPlantMaterial;

    public AudioSource PlantSound;

    private void Start()
    {
        if (parentRectTransform == null)
        {
            Debug.LogError("Parent RectTransform not found.");
        }

        canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in the scene. Please add a Canvas.");
        }
    }

   
    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out worldPoint))
        {
            transform.position = worldPoint;

            if (!RectTransformUtility.RectangleContainsScreenPoint(parentRectTransform, eventData.position, eventData.pressEventCamera))
            {
                isOutsideParentRect = true;
            }
            else
            {
                isOutsideParentRect = false;
            }
        }

        if (isOutsideParentRect)
        {
            parentRectTransform.gameObject.SetActive(false);
            isOutsideParentRect = false;
            return;
        }

        if (Camera.main == null)
        {
            Debug.LogError("Main Camera not found.");
            transform.position = startPosition;
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                if (groundRenderer == null)
                {
                    groundRenderer = hit.collider.GetComponent<Renderer>();
                    if (groundRenderer != null)
                    {
                        if(hit.collider.transform.childCount != 0)
                        {
                            groundRenderer.material = cantPlantMaterial;
                        }
                        else{
                             originalMaterial = groundRenderer.material;
                            groundRenderer.material = hoverMaterial;
                        }   
                       
                    }
                }
            }
            else
            {
                ResetGroundMaterial();
            }
        }
        else
        {
            ResetGroundMaterial();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(startParent, true);
        ResetGroundMaterial();

        if (Camera.main == null)
        {
            Debug.LogError("Main Camera not found.");
            transform.position = startPosition;
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                if(hit.collider.transform.childCount == 0 && treeInformation.Amount > 0)
                {
                    Debug.Log("Can plant");
                    GameObject plant = Instantiate(TreePrefabs, TreePrefabs.transform.position, Quaternion.identity);
                    
                    plant.transform.parent = hit.collider.transform;
                    plant.transform.localPosition = Vector3.zero;
                    treeInformation.Amount--;
                    PlantSound.Play();
                }
                
            }
            else
            {
                transform.position = startPosition;
            }
        }
        else
        {
            transform.position = startPosition;
        }
    }

    private void ResetGroundMaterial()
    {
        if (groundRenderer != null)
        {
            groundRenderer.material = originalMaterial;
            groundRenderer = null;
        }
    }

    public void Update()
    {
        AmountText.text = treeInformation.Amount.ToString();
    }
}
