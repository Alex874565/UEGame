using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    public Canvas canvas; // Reference to the canvas for cursor positioning
    public Image cursorImage;
    [Tooltip("Hotspot is where the actual click happens in the cursor image")]
    public Vector2 cursorHotspot = Vector2.zero;
    public Texture2D cursorHotspotTexture; // Optional hotspot texture for the cursor

    private float targetZoom;
    private Vector3 targetPosition;
    private Vector3 dragOrigin;
    private Camera cam;
    private bool isDragging = false;

    void Start()
    {
        Cursor.visible = false;

        cursorImage.raycastTarget = false;

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
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            null,
            out pos
        );
        cursorImage.rectTransform.anchoredPosition = pos;

        // Convert pixel hotspot to canvas units (UI scale)
        Vector2 canvasScale = canvas.transform.lossyScale;
        Vector2 adjustedHotspot = new Vector2(
            cursorHotspot.x / canvasScale.x,
            -cursorHotspot.y / canvasScale.y // Flip Y for UI space
        );

        cursorImage.rectTransform.anchoredPosition = pos - adjustedHotspot;

        // Update sprite
        Texture2D currentCursor = cursorNormal;
        if (Input.GetMouseButton(1))
            currentCursor = cursorHand;
        else if (Input.GetMouseButton(0))
            currentCursor = cursorClick;

        if (cursorImage.sprite == null || cursorImage.sprite.texture != currentCursor)
        {
            cursorImage.sprite = Sprite.Create(
                currentCursor,
                new Rect(0, 0, currentCursor.width, currentCursor.height),
                new Vector2(0.5f, 0.5f), // Pivot is ignored for rectTransform anchoring here
                100f
            );
        }
    }
}
