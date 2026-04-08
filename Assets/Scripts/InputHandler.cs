using System;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;

    public event Action OnDragRelease;
    public event Action<Vector2> OnDragDirection;

    private Camera cam;

    public bool isDragging;
    public bool canDrag = true;

    public Vector2 startPoint;
    public Vector2 currentPoint;
    public Vector2 dragVector;
    public float maxDrag = 3f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        cam = Camera.main;
    }

    void Update()
    {
        if (!canDrag) return;

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startPoint = GetMouseWorld2D();
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            currentPoint = GetMouseWorld2D();

            dragVector = currentPoint - startPoint;
            dragVector = Vector2.ClampMagnitude(dragVector, maxDrag);

            OnDragDirection?.Invoke(dragVector);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            dragVector = Vector2.zero;

            OnDragRelease?.Invoke();
        }
    }

    Vector2 GetMouseWorld2D()
    {
        Vector3 mousePos = Input.mousePosition;


        mousePos.z = Mathf.Abs(cam.transform.position.z);

        Vector3 mouseWorld = cam.ScreenToWorldPoint(mousePos);
        return new Vector2(mouseWorld.x, mouseWorld.y);
    }
}