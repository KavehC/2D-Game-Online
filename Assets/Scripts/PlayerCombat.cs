using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class PlayerCombat : NetworkBehaviour
{
    private Animator animator;

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
            Debug.Log("Attack input detected");
            animator.SetTrigger("attack");
        }
    }
}