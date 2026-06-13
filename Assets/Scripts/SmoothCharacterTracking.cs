using UnityEngine;
using UnityEngine.InputSystem;

public class SmoothCharacterTracking : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField, Tooltip("Camera orthographic size for zoom (smaller = closer)")]
    private float zoomSize = 5f;

    [SerializeField, Tooltip("Minimum orthographic size (closest)")]
    private float minZoom = 2f;

    [SerializeField, Tooltip("Maximum orthographic size (furthest)")]
    private float maxZoom = 20f;

    [SerializeField, Tooltip("How fast mouse wheel changes the zoom.")]
    private float zoomSpeed = 2f;

    [SerializeField, Tooltip("Zoom lerp speed (higher = faster)")]
    private float zoomLerpSpeed = 10f;

    Camera cam;

    [SerializeField, Tooltip("Transform to follow. If null, will try to find object tagged 'Player'.")]
    private Transform target;

    [SerializeField, Tooltip("Offset from the target. For 2D set Z to -10.")]
    private Vector3 offset = new Vector3(0f, 0f, -10f);

    [SerializeField, Tooltip("Time (seconds) the camera takes to catch up to the target. Lower = less lag. Typical: 0.05")]
    private float smoothTime = 0.05f;

    [SerializeField, Tooltip("If true, camera will only follow X and Y; Z stays at offset.z.")]
    private bool followXYOnly = true;

    private Vector3 velocity = Vector3.zero;
    [SerializeField, Tooltip("How often (seconds) to try finding the player when target is null.")]
    private float searchInterval = 0.5f;

    private float searchTimer = 0f;

    private void Reset()
    {
        offset = new Vector3(0f, 0f, -10f);
        smoothTime = 0.05f;
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
        if (cam != null && cam.orthographic)
        {
            zoomSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            cam.orthographicSize = zoomSize;
        }
        if (target == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            searchTimer += Time.deltaTime;
            if (searchTimer >= searchInterval)
            {
                searchTimer = 0f;
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                    Debug.Log("SmoothCharacterTracking: found Player and assigned target.");
                }
            }
            return;
        }

        // reset timer when we have a target
        searchTimer = 0f;

        // Read scroll input (Input System only)
        float scroll = 0f;
        if (Mouse.current != null)
        {
            scroll = Mouse.current.scroll.y.ReadValue();
        }

        if (cam != null && cam.orthographic)
        {
            // Directly increment/decrement zoomSize for constant speed
            if (Mathf.Abs(scroll) > 0.0001f)
            {
                zoomSize -= scroll * zoomSpeed;
                zoomSize = Mathf.Clamp(zoomSize, minZoom, maxZoom);
                cam.orthographicSize = zoomSize;
            }
        }

        Vector3 desiredPosition = target.position + offset;
        if (followXYOnly) desiredPosition.z = offset.z;

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, Mathf.Max(0.0001f, smoothTime));
    }

    /// <summary>
    /// Assign the follow target at runtime (call from your spawner/manager).
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(target.position, target.position + offset);
        Gizmos.DrawSphere(target.position + offset, 0.12f);
    }

    public Transform GetTarget()
    {
        return target;
    }
}

