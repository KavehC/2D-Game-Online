using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Player_Behavior behavior;

    private Vector2 movement;
    public Vector2 lastMoveDir = Vector2.down; // default facing direction

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        behavior = GetComponent<Player_Behavior>();
    }

    void Update()
    {
        // Only animate LOCAL player
        if (!isLocalPlayer) return;

        // Get movement from Player_Behavior
        movement = behavior.Movement;

        float magnitude = movement.magnitude;
        bool isMoving = magnitude > 0.1f;

        // Remember last direction ONLY while moving
        if (isMoving)
            lastMoveDir = movement;

        // Direction parameters (used by 2D blend trees)
        animator.SetFloat(
            "moveX",
            isMoving ? movement.x : lastMoveDir.x,
            0.1f,
            Time.deltaTime
        );

        animator.SetFloat(
            "moveY",
            isMoving ? movement.y : lastMoveDir.y,
            0.1f,
            Time.deltaTime
        );

        // Idle ↔ Movement transition
        animator.SetBool("isMoving", isMoving);

        // Speed parameter for Walk/Run blend tree
        float speed = 0f;

        if (isMoving)
        {
            // 0.5 = walk threshold
            // 1.0 = run threshold
            speed = behavior.IsRunning ? 1f : 0.5f;
        }

        animator.SetFloat("speed", speed, 0.1f, Time.deltaTime);

        // Flip sprite left/right
        if (lastMoveDir.x < -0.01f)
            spriteRenderer.flipX = true;
        else if (lastMoveDir.x > 0.01f)
            spriteRenderer.flipX = false;
    }
}