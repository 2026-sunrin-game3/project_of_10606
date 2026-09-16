using UnityEngine;

public class HomingBossBullet : MonoBehaviour
{
    [SerializeField] LayerMask hitMask; // Player, Wall 체크
    [SerializeField] float turnSpeed = 5f;

    float damage;
    float speed;
    EntityHealth attacker;
    Transform target;
    bool hit;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void SetDamage(
        float damage_,
        EntityHealth attacker_,
        Transform target_,
        float speed_
    )
    {
        damage = damage_;
        attacker = attacker_;
        target = target_;
        speed = speed_;

        Vector2 direction =
            (target.position - transform.position).normalized;

        rigid.linearVelocity = direction * speed;
        transform.right = direction;
    }

    void FixedUpdate()
    {
        if (target == null || hit)
            return;

        Vector2 direction =
            (target.position - transform.position).normalized;

        Vector2 targetVelocity = direction * speed;

        // 현재 방향에서 플레이어 방향으로 부드럽게 회전
        rigid.linearVelocity = Vector2.Lerp(
            rigid.linearVelocity,
            targetVelocity,
            turnSpeed * Time.fixedDeltaTime
        );

        if (rigid.linearVelocity.sqrMagnitude > 0.01f)
        {
            transform.right = rigid.linearVelocity.normalized;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) return;

        if ((hitMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        hit = true;

        EntityHealth playerHealth =
            other.GetComponentInParent<EntityHealth>();

        if (playerHealth != null)
        {
            playerHealth.GetDamage(damage, attacker);
        }

        Destroy(gameObject);
    }
}