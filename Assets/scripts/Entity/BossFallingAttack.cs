using UnityEngine;

public class BossFallingAttack : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] LayerMask hitMask; // Player, Wall 체크

    float damage;
    EntityHealth attacker;
    bool hit;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
    public void SetDamage(float damage_, EntityHealth attacker_)
    {
        damage = damage_;
        attacker = attacker_;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hit) return;

        // 플레이어와 벽만 충돌 대상으로 처리
        if ((hitMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        hit = true;

        EntityHealth playerHealth = other.GetComponentInParent<EntityHealth>();

        if (playerHealth != null)
        {
            playerHealth.GetDamage(damage, attacker);
        }

        // 플레이어 또는 바닥에 닿으면 낙하물 삭제
        Destroy(gameObject);
    }
}