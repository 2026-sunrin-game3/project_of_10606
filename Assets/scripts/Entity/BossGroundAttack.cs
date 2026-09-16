using UnityEngine;

public class BossGroundAttack : MonoBehaviour
{
    [SerializeField] LayerMask hitMask; // Player, Wall 체크

    float damage;
    EntityHealth attacker;
    bool hit;
    AudioSource audioSource;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void SetDamage(
        float damage_,
        EntityHealth attacker_,
        Vector2 velocity
    )
    {
        damage = damage_;
        attacker = attacker_;

        rigid.linearVelocity = velocity;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) return;

        // Player 또는 Wall에 닿을 때만 처리
        if ((hitMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        hit = true;

        EntityHealth playerHealth = other.GetComponentInParent<EntityHealth>();

        if (playerHealth != null)
        {
            playerHealth.GetDamage(damage, attacker);
        }

        Destroy(gameObject);
    }
}