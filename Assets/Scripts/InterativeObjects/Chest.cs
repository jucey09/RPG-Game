using UnityEngine;

public class Chest : MonoBehaviour, IDamgable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();

    [Header("Open Details")]
    [SerializeField] private Vector2 kockback = new Vector2(0, 3);
    public bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        anim.SetBool("chestOpen", true);
        rb.linearVelocity = kockback;

        return true;
    }
}
