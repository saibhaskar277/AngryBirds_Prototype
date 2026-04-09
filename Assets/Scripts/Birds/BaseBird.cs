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
    public float angularStopThreshold = 10f;

    [Header("Ground Settle Control")]
    [SerializeField] private string groundTag = "Ground";
    [SerializeField] private float postGroundLinearDamping = 4f;
    [SerializeField] private float postGroundAngularDamping = 4f;
    [SerializeField] private float maxGroundMoveTime = 3f;

    private float stopTimer = 0f;
    private float defaultLinearDamping;
    private float defaultAngularDamping;
    private bool hasHitGround;
    private float groundMoveTimer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultLinearDamping = rb.linearDamping;
        defaultAngularDamping = rb.angularDamping;
    }

    private void Update()
    {
        if (!isBirdLaunched) return;

        bool isSleeping = rb.IsSleeping();
        bool isSlowLinear = rb.linearVelocity.sqrMagnitude < (stopThreshold * stopThreshold);
        bool isSlowAngular = Mathf.Abs(rb.angularVelocity) < angularStopThreshold;

        if (hasHitGround)
        {
            groundMoveTimer += Time.deltaTime;
            if (groundMoveTimer >= maxGroundMoveTime)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.Sleep();
            }
        }

        if (isSleeping || (isSlowLinear && isSlowAngular))
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
        rb.linearDamping = defaultLinearDamping;
        rb.angularDamping = defaultAngularDamping;

        Vector2 launchDir = -direction;
        float power = direction.magnitude * launchForce;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.AddForce(launchDir * power, ForceMode2D.Impulse);

        isBirdLaunched = true;
        stopTimer = 0f;
        hasHitGround = false;
        groundMoveTimer = 0f;
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
        rb.linearDamping = defaultLinearDamping;
        rb.angularDamping = defaultAngularDamping;
        hasHitGround = false;
        groundMoveTimer = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBirdLaunched || hasHitGround)
            return;

        if (!collision.collider.CompareTag(groundTag))
            return;

        hasHitGround = true;
        groundMoveTimer = 0f;
        rb.linearDamping = postGroundLinearDamping;
        rb.angularDamping = postGroundAngularDamping;
    }
}