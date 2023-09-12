using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(PlayerInputHandler))]
public class RigidbodyPlayerController2D : Player
{
    public float AttackCooldown = 0.3f;

    [Header("Movement")]
    public float Speed = 3;

    [Header("Jump")]
    public float JumpForce = 5;
    public float MaxJumpDuration = 0.2f;
    public bool CanDoubleJump;
    public float DoubleJumpForce = 2;
    public float GravityForceDown = 20;

    [Header("Dash")]
    public bool CanDash;
    public float DashForce = 20;
    [Tooltip("The time for how long input is disabled after dashing.")]
    public float DashInputPreventionCooldown = 0.5f;

    [Header("Gravity")]
    [Tooltip("The maximum negative velocity force. Used to limit Y velocity from speeding up further.")]
    public float MaxVelocityForceDown = 50;
    [Tooltip("The acceleration after wall jumping.")]
    public float AirAcceleration = 20;

    [Header("Wall Actions")]
    [Tooltip("If enabled, will slow down while falling near a wall")]
    public bool CanWallSlide;
    public bool CanWallJump;
    public float WallJumpForce = 10;
    public float WallDistanceCheck = 0.3f;
    public float WallSlideSpeed = 5;
    public float WallJumpInputPreventionCooldown = 0.5f;

    [Space(10)]
    public ContactFilter2D ContactFilter2D;
    public float GroundDistanceCheck = 0.3f;
    public LayerMask CollisionLayer;

    Animator anim;
    PlayerInputHandler input;
    Rigidbody2D rb;
    Health health;
    BoxCollider2D box2D;
    Transform positionAfterHit;
    Vector2 velocity;
    Vector2 groundNormal;
    Vector2 wallNormal;
    Vector2 move;
    float lastTimeAttacked;
    float lastTimeWallJumped;
    float lastTimeDashed;
    float lastTimeOnGround;
    bool isGrounded;
    bool wasPreviouslyGrounded;
    bool jumped;
    bool doubleJumped;
    bool dashed;
    bool wallJumped;
    bool previouslyWallJumped;
    bool canWallJump;
    bool isWallSliding;
    bool canDash;
    bool canDoubleJump;
    bool isDead;
    bool isInvincible;


    readonly int k_horizontalMoveAnimationHash = Animator.StringToHash("MoveX");
    readonly int k_verticalMoveAnimationHash = Animator.StringToHash("MoveY");
    readonly int k_isGroundedAnimationHash = Animator.StringToHash("IsGrounded");

    private void OnEnable() {
        EventManager.AddListener<LevelTransitionEvent>(OnLevelTransition);
        EventManager.AddListener<PlayerWinEvent>(OnGameWin);
        EventManager.AddListener<GameQuitEvent>(OnGameQuit);
    }

    private void OnDisable() {
        EventManager.RemoveListener<LevelTransitionEvent>(OnLevelTransition);
        EventManager.RemoveListener<PlayerWinEvent>(OnGameWin);
        EventManager.RemoveListener<GameQuitEvent>(OnGameQuit);
    }

    // Start is called before the first frame update
    void Start() {
        box2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInputHandler>();
        health = GetComponent<Health>();
        health.OnDamaged += OnDamaged;
        health.OnDie += OnDie;

        ContactFilter2D.useLayerMask = true;
    }

    // Update is called once per frame
    void Update() {
        move = new(input.move.x, 0);

        if (CanDash) {
            if (canDash && Keyboard.current.lKey.wasPressedThisFrame && Time.time > lastTimeDashed + 0.7f) {
                dashed = true;
            }
        }

        if (isGrounded && input.jump) {
            jumped = true;
        }

        if (CanWallJump) {
            if (canWallJump && input.jump) {
                wallJumped = true;
            }
        }

        if (CanDoubleJump) {
            if (!isWallSliding && canDoubleJump && input.jump) {
                doubleJumped = true;
            }
        }

        if (Keyboard.current.fKey.wasPressedThisFrame && Time.time > lastTimeAttacked + AttackCooldown) {
            if (anim != null)
                anim.SetTrigger("Attack");
            lastTimeAttacked = Time.time;
        }
    }

    private void LateUpdate() {
        if (anim != null) {
            anim.SetBool(k_isGroundedAnimationHash, isGrounded);
            anim.SetFloat(k_horizontalMoveAnimationHash, Mathf.Abs(rb.velocity.x));
            anim.SetFloat(k_verticalMoveAnimationHash, rb.velocity.y);
        }

        if (!isDead && TookDamage && ScreenFade.Instance.NormalizedTime >= 1) {
            transform.position = positionAfterHit.position;
            ResetVelocity();
            ResetMove();
            TookDamage = false;
            positionAfterHit = null;
            anim.SetTrigger("Respawn");
        }
    }

    private void FixedUpdate() {

        velocity = new(move.x * Speed, 0);

        wasPreviouslyGrounded = isGrounded;

        isGrounded = false;
        groundNormal = Vector2.zero;

        var hit = Physics2D.BoxCast(box2D.bounds.center, box2D.bounds.size, 0, Vector2.down, GroundDistanceCheck, CollisionLayer);

        if (hit.collider != null) {
            //if (Vector2.Dot(transform.up, hit.normal) > 0) {
                isGrounded = true;
                groundNormal = hit.normal;
            //}
        }

        RaycastHit2D[] results = new RaycastHit2D[2];

        var count = rb.Cast(velocity.normalized, ContactFilter2D, results, velocity.magnitude * Time.deltaTime + 0.01f);

        for (int i = 0; i < count; i++) {
            var hitNormal = results[i].normal;

            // If we collide with something while grounded, slow down or stop
            // (ex: slowing down on hills or stop completely at a wall)
            if (isGrounded) {
                var projection = Vector2.Dot(velocity, hitNormal);

                if (projection < 0) {
                    velocity -= projection * hitNormal;
                }

            } else {
                // If we're in the air and hit something from the sides, stop x velocity
                if (Vector2.Dot(transform.right, hitNormal) == -1) {
                    velocity.x = 0;
                }

                //if (Vector2.Dot(transform.up, hitNormal) < 0) {
                    //var velocityY = Mathf.Min(rb.velocity.y, 0);
                    //rb.velocity = new Vector2(rb.velocity.x, velocityY);

                    //Vector2 verticalVelocity = new(0, rb.velocity.y);
                    //verticalVelocity.y = Mathf.Min(verticalVelocity.y, 0);
                    //rb.velocity = verticalVelocity + (rb.velocity.x * Vector2.right);
                //}
            }
        }

        if (!TookDamage) {
            rb.velocity = new(velocity.x, rb.velocity.y);

            if (isGrounded) {
                canDoubleJump = false;

                if (jumped) {
                    rb.velocity = new(rb.velocity.x, 0);
                    rb.AddForce(JumpForce / 3 * Vector2.up, ForceMode2D.Impulse);

                    isGrounded = false;
                    groundNormal = Vector2.zero;
                    canDoubleJump = true;
                }

                //fixme
                lastTimeOnGround = Time.time;

            } else {

                //if (wasPreviouslyGrounded && !isGrounded) {
                //    canDoubleJump = true;
                //}

                if (jumped && input.jumpHeld && rb.velocity.y > 0 && Time.time < lastTimeOnGround + MaxJumpDuration) {
                    rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);

                    // Clamp jump velocity to not go higher
                    Vector2 jumpVelocity = new(0, rb.velocity.y);
                    jumpVelocity = Vector2.ClampMagnitude(jumpVelocity, JumpForce);
                    rb.velocity = jumpVelocity + velocity;

                } else {
                    jumped = false;
                }

                if (doubleJumped) {
                    rb.velocity = new(rb.velocity.x, 0);
                    rb.AddForce(Vector2.up * DoubleJumpForce, ForceMode2D.Impulse);

                    doubleJumped = false;
                    canDoubleJump = false;
                }

                // Clamp vertical velocity magnitude while falling to prevent speeding up
                Vector2 verticalVelocity = new(0, rb.velocity.y);
                verticalVelocity = Vector2.ClampMagnitude(verticalVelocity, MaxVelocityForceDown);
                rb.velocity = verticalVelocity + velocity;
            }

            // Flip sprite after final velocity
            if (velocity.x > 0) {
                FlipSprite(0);
            } else if (velocity.x < 0) {
                FlipSprite(180);
            }
        }
    }

    /// <summary>
    /// Flips the sprite's Y rotation.
    /// </summary>
    /// <param name="degrees">The degrees of the Y rotation.</param>
    void FlipSprite(float degrees) {
        transform.rotation = Quaternion.Euler(0, degrees, 0);
    }

    /// <summary>
    /// Sets the velocity to (0, 0, 0).
    /// </summary>
    public override void ResetVelocity() {
        velocity = Vector2.zero;
    }

    public override void ResetMove() {
        input.ResetMove();
    }

    void OnLevelTransition(LevelTransitionEvent evt) {
        if (evt.isTransitioningIn) {
            isInvincible = true;
            ResetMove();
        }

        if (evt.isTransitioningOut) {
            ResetVelocity();
            ResetMove();
            transform.position = evt.newPosition;
            isInvincible = false;
        }
    }

    void OnDamaged(float dmg, GameObject gameObject, Transform positionAfterHit) {
        if (isInvincible) return;

        TookDamage = true;
        this.positionAfterHit = positionAfterHit;
        ScreenFade.Instance.FadeInAndOut(0.5f, 0.5f, 0.5f);
        anim.SetTrigger("Hit");
    }

    void OnDie() {
        isDead = true;

        EventManager.Broadcast(Events.PlayerDeathEvent);
    }

    void OnGameWin(PlayerWinEvent evt) {
        ResetMove();
    }

    void OnGameQuit(GameQuitEvent evt) {
        ResetMove();
    }

    private void OnDrawGizmos() {
        if (TryGetComponent(out BoxCollider2D box)) {

            if (isGrounded)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;

            RaycastHelper.DrawBoxCast(box.bounds.center, box.bounds.size, Vector2.down, GroundDistanceCheck);

            Gizmos.color = Color.yellow;
            // Wall checking box
            RaycastHelper.DrawBoxCast(box.bounds.center, box.bounds.size,
                transform.right, WallDistanceCheck);

            Vector2 normalizedVelocity = velocity.normalized;

            if (Mathf.Abs(normalizedVelocity.x) > 0.1f && Mathf.Abs(normalizedVelocity.y) > 0.1f) {
                Gizmos.color = Color.magenta;
            } else if (Mathf.Abs(normalizedVelocity.x) > 0.1f) {
                Gizmos.color = Color.red;
            } else if (Mathf.Abs(normalizedVelocity.y) > 0.1f) {
                Gizmos.color = Color.green;
            }

            // Velocity direction ray
            RaycastHelper.DrawRayCast(box.bounds.center, normalizedVelocity, 1);
        }
    }
}
