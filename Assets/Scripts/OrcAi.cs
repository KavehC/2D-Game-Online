using Mirror;
using UnityEngine;

public class OrcAI : NetworkBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 1f;
    public int damage = 10;
    public float attackCooldown = 1f;

    private float lastAttack;
    private Transform target;

    void Update()
    {
        if (!isServer) return;

        FindTarget();

        if (target == null) return;

        float dist = Vector2.Distance(transform.position, target.position);

        if (dist > attackRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            TryAttack();
        }
    }

void FindTarget()
{
    if (target != null) return;

    GameObject playerObj = GameObject.FindWithTag("Player");
    if (playerObj != null)
    {
        target = playerObj.transform;
        Debug.Log($"[OrcAI] Target set to: {target.name} at {target.position}");
    }
    else
    {
        Debug.Log("[OrcAI] No player with tag 'Player' found.");
    }
}

    void TryAttack()
    {
        if (Time.time - lastAttack < attackCooldown) return;

        lastAttack = Time.time;

        Health hp = target.GetComponent<Health>();
        if (hp != null)
            hp.TakeDamage(damage);
    }
}
