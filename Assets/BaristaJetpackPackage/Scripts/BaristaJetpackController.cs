using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaristaJetpackController : MonoBehaviour
{
    [Header("Movement & Jetpack Settings")]
    public float forwardSpeed = 5f;
    public float jetpackThrust = 12f;
    public float maxFallSpeed = -10f;

    [Header("Sprite Animation")]
    public SpriteRenderer spriteRenderer;
    public Sprite idleSprite;
    public Sprite[] flyAnimationFrames;
    public float animationFps = 10f;

    [Header("Particle Effect")]
    public ParticleSystem creamParticleFx;

    private Rigidbody2D rb;
    private bool isThrusting = false;
    private float animTimer = 0f;
    private int currentFrame = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Nhấn giữ chuột trái hoặc phím Space để kích hoạt phản lực kem tươi
        isThrusting = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

        HandleAnimation();
    }

    void FixedUpdate()
    {
        if (isThrusting)
        {
            rb.velocity = new Vector2(forwardSpeed, jetpackThrust);
            if (creamParticleFx != null && !creamParticleFx.isPlaying)
            {
                creamParticleFx.Play();
            }
        }
        else
        {
            rb.velocity = new Vector2(forwardSpeed, Mathf.Max(rb.velocity.y, maxFallSpeed));
            if (creamParticleFx != null && creamParticleFx.isPlaying)
            {
                creamParticleFx.Stop();
            }
        }
    }

    private void HandleAnimation()
    {
        if (isThrusting && flyAnimationFrames != null && flyAnimationFrames.Length > 0)
        {
            animTimer += Time.deltaTime;
            if (animTimer >= 1f / animationFps)
            {
                animTimer = 0f;
                currentFrame = (currentFrame + 1) % flyAnimationFrames.Length;
                spriteRenderer.sprite = flyAnimationFrames[currentFrame];
            }
        }
        else
        {
            if (idleSprite != null)
            {
                spriteRenderer.sprite = idleSprite;
            }
        }
    }
}
