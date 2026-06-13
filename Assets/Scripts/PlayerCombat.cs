using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class PlayerCombat : NetworkBehaviour
{
    private Animator animator;
    public float attackRange = 1f;
    public int damage = 20;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            animator.SetTrigger("attack");
            TryAttack();
        }
    }

    void TryAttack()
    {
        // Raycast in the direction the player last moved
        Vector2 dir = GetComponent<PlayerMovement>().lastMoveDir;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, attackRange);

        if (hit.collider != null)
        {
            Health hp = hit.collider.GetComponent<Health>();
            if (hp != null)
            {
                CmdDealDamage(hit.collider.gameObject, damage);
            }
        }
    }

    [Command]
    void CmdDealDamage(GameObject target, int dmg)
    {
        Health hp = target.GetComponent<Health>();
        if (hp != null)
            hp.TakeDamage(dmg);
    }
}
