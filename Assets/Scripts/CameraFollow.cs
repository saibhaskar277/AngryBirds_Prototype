using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;       // The bird to follow
    public float followSpeed = 5f; // Camera smooth speed
    public float xMin = -10f;      // Left boundary
    public float xMax = 50f;       // Right boundary
    public float yOffset = 2f;     // Optional vertical offset

    [Header("Pan Settings")]
    public float panSpeed = 5f;    // Speed when manually panning
    private bool isDragging = false;

    private Camera cam;

    bool canFollow = false;

    private void Start()
    {
        cam = Camera.main;

        EventManager.AddListner<OnBirdLaunched>(Instance_OnBirdLaunch);

        EventManager.AddListner<OnBirdLaunchFinished>(Instance_OnBirdLaunchCompleted);
    }

    private void Instance_OnBirdLaunchCompleted(OnBirdLaunchFinished e)
    {
        canFollow = false;
    }

    private void Instance_OnBirdLaunch(OnBirdLaunched e)
    {
        target = e.Bird;
        canFollow = true;
    }


    private void LateUpdate()
    {
        if (target == null) return;

        // ✅ Check if bird is moving significantly
        if (canFollow)
        {
            // Follow the bird smoothly
            Vector3 desiredPos = new Vector3(target.position.x, target.position.y + yOffset, transform.position.z);
            desiredPos.x = Mathf.Clamp(desiredPos.x, xMin, xMax);
            transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
      
        if(!canFollow)
        {
            // Bird stopped → allow manual horizontal panning
            float horizontalInput = Input.GetAxis("Horizontal"); // Arrow keys or A/D
            if (Mathf.Abs(horizontalInput) > 0.01f)
            {
                Vector3 pos = transform.position;
                pos.x += horizontalInput * panSpeed * Time.deltaTime;
                pos.x = Mathf.Clamp(pos.x, xMin, xMax);
                transform.position = pos;
            }
        }
    }
}