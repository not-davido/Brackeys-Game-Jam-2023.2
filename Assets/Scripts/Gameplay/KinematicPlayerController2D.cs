using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(PlayerInputHandler))]
public class KinematicPlayerController2D : Player
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

    Animator anim;
    PlayerInputHandler input;
    Rigidbody2D rb;
    Health health;
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
    bool tookDamaged;

    readonly int k_horizontalMoveAnimationHash = Animator.StringToHash("MoveX");
    readonly int k_verticalMoveAnimationHash = Animator.StringToHash("MoveY");
    readonly int k_isGroundedAnimationHash = Animator.StringToHash("IsGrounded");

    private void OnEnable()
    {
        EventManager.AddListener<LevelTransitionEvent>(OnLevelTransition);
    }

    private void OnDisable() {
        EventManager.RemoveListener<LevelTransitionEvent>(OnLevelTransition);
    }

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInputHandler>();
        health = GetComponent<Health>();
        health.OnDamaged += OnDamaged;

        rb.isKinematic = true;

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
            anim.SetFloat(k_horizontalMoveAnimationHash, Mathf.Abs(velocity.x));
            anim.SetFloat(k_verticalMoveAnimationHash, velocity.y);
        }

        if (tookDamaged && ScreenFade.Instance.NormalizedTime >= 1) {
            GameManager.Instance.SetPlayerPositionAfterDamage();
            ResetVelocity();
            ResetMove();
            tookDamaged = false;
        }
    }

    private void FixedUpdate() {
        // Apply gravity over time
        if (!jumped)
            velocity += GravityForceDown * Time.deltaTime * Vector2.down;

        // Clamp vertical magnitude to prevent it from speeding up
        Vector2 verticalVelocity = new(0, velocity.y);
        verticalVelocity = Vector2.ClampMagnitude(verticalVelocity, MaxVelocityForceDown);
        velocity = verticalVelocity + (velocity.x * Vector2.right);

        // Y velocity should be 0 so we won't be falling during dash
        if (Time.time < lastTimeDashed + DashInputPreventionCooldown)
            velocity.y = 0;

        // Reduce y velocity if we're against a wall
        if (velocity.y < 0 && isWallSliding) {
            velocity = WallSlideSpeed * Vector2.down;
        }

        // Prevent velocity from being updated by input after wall jump or dashing
        if (Time.time > lastTimeWallJumped + WallJumpInputPreventionCooldown &&
            Time.time > lastTimeDashed + DashInputPreventionCooldown &&
            !previouslyWallJumped) {
            velocity.x = move.x * Speed;
        }

        wasPreviouslyGrounded = isGrounded;

        // For collision checking
        isGrounded = false;
        groundNormal = Vector2.up;

        RaycastHit2D[] results = new RaycastHit2D[8];

        var count = rb.Cast(velocity.normalized, ContactFilter2D, results, velocity.magnitude * Time.deltaTime + 0.01f);

        float distance = 0;

        for (int i = 0; i < count; i++) {
            var hitNormal = results[i].normal;

            if (Vector3.Dot(transform.up, hitNormal) > 0) {
                groundNormal = hitNormal;
                isGrounded = true;
                distance = results[i].distance;

                // So sprite won't flip or slide slowly down
                if (Vector2.Angle(transform.up, groundNormal) <= 45f) {
                    velocity.y = 0;
                }
            }

            // If we collide with something while grounded, slow down or stop
            // (ex: slowing down on hills or stop completely at a wall)
            if (isGrounded) {
                var projection = Vector2.Dot(velocity, hitNormal);

                if (projection < 0) {
                    velocity -= projection * hitNormal;
                }

                //velocity = Vector3.ProjectOnPlane(velocity, groundNormal);
            } else {
                // If we're in the air and hit something from the sides, stop x velocity
                if (Vector2.Dot(transform.right, hitNormal) == -1) {
                    velocity.x = 0;
                }

                // If we jump and hit something from top, stop y velocity
                if (Vector2.Dot(transform.up, hitNormal) < 0) {
                    velocity.y = Mathf.Min(velocity.y, 0);
                }
            }
        }

        // For wall jumping and sliding
        canWallJump = false;
        isWallSliding = false;
        wallNormal = Vector2.zero;

        RaycastHit2D[] sideResults = new RaycastHit2D[5];

        var sideCount = rb.Cast(transform.right, ContactFilter2D, sideResults, WallDistanceCheck);

        for (int i = 0; i < sideCount; i++) {
            var sideNormal = sideResults[i].normal;

            if (!isGrounded) {
                if (Vector2.Dot(transform.right, sideNormal) <= -1) {
                    canWallJump = true;
                    isWallSliding = true;
                    wallNormal = sideNormal;
                }
            }
        }

        if (canWallJump) {
            previouslyWallJumped = false;
        }

        if (isGrounded) {
            canDash = true;
            previouslyWallJumped = false;
            canDoubleJump = false;

            if (jumped) {
                velocity = new(velocity.x, 0);
                // Jump 1/3 of maximum jump height if just tapped
                velocity += JumpForce / 2 * Vector2.up;

                canDoubleJump = true;
                isGrounded = false;
                groundNormal = Vector2.up;
            }

            lastTimeOnGround = Time.time;

        } else {

            if (wasPreviouslyGrounded && !isGrounded) {
                canDoubleJump = true;
            }

            //if (isWallSliding) {
            //    canDoubleJump = false;
            //}

            if (previouslyWallJumped) {
                canDash = true;
                velocity.x += Time.deltaTime * move.x * AirAcceleration;

                // Clamp horizontal speed
                Vector2 horizontalVelocity = new(velocity.x, 0);
                horizontalVelocity = Vector2.ClampMagnitude(horizontalVelocity, Speed);
                velocity = horizontalVelocity + (velocity.y * Vector2.up);
            }

            if (wallJumped) {
                // If we fall and haven't jumped yet and then wall slide, double jumping condition may be set true
                // which will not double jump so reset to false
                //if (canDoubleJump) {
                //    canDoubleJump = false;
                //}

                //canDoubleJump = false;

                velocity = Vector2.zero;
                velocity += wallNormal.x * WallJumpForce * Vector2.right + WallJumpForce * Vector2.up;

                lastTimeWallJumped = Time.time;
                wallJumped = false;
                previouslyWallJumped = true;
                canDoubleJump = true;
            }

            if (jumped && input.jumpHeld && velocity.y > 0 && Time.time < lastTimeOnGround + MaxJumpDuration) {
                velocity.y = JumpForce;
            } else {
                jumped = false;
            }

            if (doubleJumped) {
                velocity = new(velocity.x, 0);
                velocity += DoubleJumpForce * Vector2.up;

                doubleJumped = false;
                canDoubleJump = false;
            }
        }

        if (dashed) {
            if (!isGrounded)
                canDash = false;

            velocity.x = 0;
            velocity += DashForce * (Vector2)transform.right;
            lastTimeDashed = Time.time;
            dashed = false;
            previouslyWallJumped = false;
        }

        // Flip sprite after final velocity
        if (velocity.x > 0) {
            FlipSprite(0);
        } else if (velocity.x < 0) {
            FlipSprite(180);
        }

        // Update final position
        rb.MovePosition(rb.position + Time.fixedDeltaTime * velocity - new Vector2(0, distance));
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

    void OnDamaged(float dmg, GameObject gameObject) {
        ScreenFade.Instance.FadeInAndOut(0.5f, 0.5f);
        tookDamaged = true;
    }

    void OnLevelTransition(LevelTransitionEvent evt) {

        if (evt.isTransitioningOut) {
            ResetVelocity();
            ResetMove();
            transform.position = evt.newPosition;
        }
    }

    private void OnDrawGizmos() {
        if (TryGetComponent(out BoxCollider2D box)) {
            Gizmos.color = Color.green;

            // Collision checking box
            RaycastHelper.DrawBoxCast(box.bounds.center, box.bounds.size,
                velocity.normalized, velocity.magnitude * Time.deltaTime + 0.01f);

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
            RaycastHelper.DrawRayCast(box.bounds.center, velocity.normalized, 1);
        }
    }
}
