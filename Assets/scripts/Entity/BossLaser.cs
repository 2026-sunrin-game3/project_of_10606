using UnityEngine;

public class BossLaser : MonoBehaviour
{
    [SerializeField] LayerMask playerMask;

    float damage;
    EntityHealth attacker;
    bool hit;

    public void SetDamage(float damage_, EntityHealth attacker_)
    {
        damage = damage_;
        attacker = attacker_;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) return;

        if ((playerMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        EntityHealth playerHealth =
            other.GetComponentInParent<EntityHealth>();

        if (playerHealth == null)
            return;

        hit = true;

        playerHealth.GetDamage(damage, attacker);
    }
}