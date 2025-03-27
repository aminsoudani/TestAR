using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Color Settings")]
    public Color[] colors;              // Set three colors in the Inspector
    private int currentColorIndex = 0;
    public Renderer objectRenderer;     // Assign the Renderer whose material color you want to change

    [Header("Rotation Settings")]
    public float rotationSpeed = 50f;   // Rotation speed (degrees per second)
    private bool isRotating = false;

    [Header("Reverse Object Settings")]
    public GameObject objectA;          // The currently active object
    public GameObject objectB;          // The object to activate when reversing

    void Update()
    {
        // If rotation is enabled (via holding the button), rotate the object on the Y-axis.
        if (isRotating)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    // Called by your "Change Color" button's OnClick event.
    public void ChangeColor()
    {
        if (colors.Length > 0 && objectRenderer != null)
        {
            // Cycle through the color array.
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
            objectRenderer.material.color = colors[currentColorIndex];
        }
    }

    // These two functions are used for the rotate button via an Event Trigger.
    public void OnPointerDown(PointerEventData eventData)
    {
        isRotating = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isRotating = false;
    }

    // Called by your "Reverse" button's OnClick event.
    // This function toggles the active state of two objects.
    public void ReverseObjects()
    {
        if (objectA != null && objectB != null)
        {
            // If objectA is active, deactivate it and activate objectB; if not, do the reverse.
            bool isAActive = objectA.activeSelf;
            objectA.SetActive(!isAActive);
            objectB.SetActive(isAActive);
        }
    }
}
