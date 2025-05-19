using UnityEngine;

public class SmoothZoomAndPan : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float minZoom = 400f;
    public float maxZoom = 540f;
    public float zoomSpeed = 100f;
    public float zoomSmoothSpeed = 10f;

    [Header("Pan Settings")]
    public float panSpeed = 1f;
    public float panSmoothSpeed = 10f;

    [Header("World Bounds (Base Limit)")]
    public Vector2 baseWorldMin = new Vector2(-1000f, -1000f);
    public Vector2 baseWorldMax = new Vector2(1000f, 1000f);

    [Header("Zoom-In Pan Expansion")]
    public float panExpandMultiplier = 2.0f;

    [Header("Cursor")]
    public Texture2D cursorNormal;
    public Texture2D cursorHand;
    public Texture2D cursorClick;
    [Tooltip("Hotspot is where the actual click happens in the cursor image")]
    public Vector2 cursorHotspot = Vector2.zero;


    private float targetZoom;
    private Vector3 targetPosition;
    private Vector3 dragOrigin;
    private Camera cam;
    private bool isDragging = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            Debug.LogWarning("Camera is not orthographic. Switching to orthographic.");
            cam.orthographic = true;
        }

        targetZoom = cam.orthographicSize;
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleZoom();
        HandlePan();
        ClampCameraToZoomBounds();

        // Smooth camera movement
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * panSmoothSpeed);

        UpdateCursor();
    }



    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSmoothSpeed);
    }

    void HandlePan()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - dragOrigin;
            if (delta.sqrMagnitude > 0.01f)
            {
                Vector3 worldDelta = cam.ScreenToWorldPoint(dragOrigin) - cam.ScreenToWorldPoint(dragOrigin + delta);
                targetPosition += worldDelta * panSpeed;

                dragOrigin = Input.mousePosition; // update to avoid compounding
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }
    }

    void ClampCameraToZoomBounds()
    {
        float zoomPercent = Mathf.InverseLerp(maxZoom, minZoom, cam.orthographicSize);
        Vector2 expansion = (baseWorldMax - baseWorldMin) * ((panExpandMultiplier - 1f) * zoomPercent);

        Vector2 expandedMin = baseWorldMin - expansion / 2f;
        Vector2 expandedMax = baseWorldMax + expansion / 2f;

        float centerX = (expandedMax.x + expandedMin.x) / 2f;
        float centerY = (expandedMax.y + expandedMin.y) / 2f;

        float xRange = (expandedMax.x - expandedMin.x) / 2f * zoomPercent;
        float yRange = (expandedMax.y - expandedMin.y) / 2f * zoomPercent;

        float minX = centerX - xRange;
        float maxX = centerX + xRange;
        float minY = centerY - yRange;
        float maxY = centerY + yRange;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
    }

    void UpdateCursor()
    {
        if (Input.GetMouseButton(1)) // Right-click held (dragging)
        {
            Cursor.SetCursor(cursorHand, cursorHotspot, CursorMode.Auto);
        }
        else if (Input.GetMouseButton(0)) // Left-click held
        {
            Cursor.SetCursor(cursorClick, cursorHotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursorNormal, cursorHotspot, CursorMode.Auto);
        }
    }

}
