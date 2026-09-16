using UnityEngine;

public class Bullet : MonoBehaviour
{
    float damage;
    EntityHealth attacker;
    bool hit;

    public void SetDamage(float damage_, EntityHealth attacker_)
    {
        damage = damage_;
        attacker = attacker_;
    }

    void Awake()
    {
        Debug.Log("총알 발싸~~");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("총알 충돌: " + other.name);
        if (hit) return;

        EntityHealth bossHealth = other.GetComponent<EntityHealth>();

        if (bossHealth == null)
            Debug.Log("이 오브젝트에는 EntityHealth가 없음");
        bossHealth = other.GetComponentInParent<EntityHealth>();

        if (bossHealth == null)
            return;

        hit = true;

        // 기존 PlayerBattle의 피해 처리 방식과 동일
        bossHealth.GetDamage(damage, attacker);

        Debug.Log("보스에게 피해: " + damage);
        Destroy(gameObject);
    }
}