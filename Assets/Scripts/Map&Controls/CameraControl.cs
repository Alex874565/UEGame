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

    [Header("World Bounds (Base Limit)")]
    public Vector2 baseWorldMin = new Vector2(-1000f, -1000f);
    public Vector2 baseWorldMax = new Vector2(1000f, 1000f);

    [Header("Zoom-In Pan Expansion")]
    public float panExpandMultiplier = 2.0f;

    private float targetZoom;
    private Camera cam;
    private Vector3 dragOrigin;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(baseWorldMin, baseWorldMax);
    }
    void Start()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            Debug.LogWarning("Camera is not orthographic. Switching to orthographic.");
            cam.orthographic = true;
        }

        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        HandleZoom();
        HandlePan();
        ClampCameraToZoomBounds();
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
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position += difference * panSpeed;
        }
    }

    void ClampCameraToZoomBounds()
    {
        float zoomPercent = Mathf.InverseLerp(maxZoom, minZoom, cam.orthographicSize);

        // Expand pan bounds outward as you zoom in
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

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}
