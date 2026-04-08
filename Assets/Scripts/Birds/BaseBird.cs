using UnityEngine;

public abstract class BaseBird : MonoBehaviour, IBirdAbility
{
    protected Rigidbody2D rb;
    protected bool abilityUsed = false;
    protected bool isBirdLaunched = false;

    [Header("Common Settings")]
    public float launchForce = 10f;

    [Header("Stop Detection")]
    public float stopThreshold = 0.2f;
    public float stopTime = 1f;

    private float stopTimer = 0f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isBirdLaunched) return;

        if (rb.linearVelocity.magnitude < stopThreshold)
        {
            stopTimer += Time.deltaTime;

            if (stopTimer >= stopTime)
            {
                isBirdLaunched = false;
                stopTimer = 0f;

                EventManager.RaiseEvent(new OnBirdLaunchFinished());
            }
        }
        else
        {
            stopTimer = 0f;
        }
    }

    protected abstract void BirdAbility();

    public void AbilityUse()
    {
        if (!abilityUsed)
        {
            abilityUsed = true;
            BirdAbility();
        }
    }

    public void OnBirdAim(Vector3 pos)
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
        transform.position = pos;
    }

    public void LaunchBird(Vector2 direction)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1;

        Vector2 launchDir = -direction;
        float power = direction.magnitude * launchForce;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.AddForce(launchDir * power, ForceMode2D.Impulse);

        isBirdLaunched = true;
        stopTimer = 0f;
    }

    public void ResetBird()
    {
        abilityUsed = false;
        isBirdLaunched = false;
        stopTimer = 0f;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
    }
}