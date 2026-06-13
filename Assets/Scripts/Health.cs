using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    public int hp = 50;

    public void TakeDamage(int dmg)
    {
        if (!isServer) return;

        hp -= dmg;
        Debug.Log(name + " took damage: " + dmg);

        if (hp <= 0)
            Destroy(gameObject);
    }
}