using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Vector2 fireOffset = new Vector2(0.6f, 0.4f);
    public float bulletSpeed = 20f;

    PlayerAnimator animator;
    EntityStat stat;
    EntityHealth health;

    void Awake()
    {
        animator = GetComponent<PlayerAnimator>();
        stat = GetComponent<EntityStat>();
        health = GetComponent<EntityHealth>();
    }

    public void ShootBullet()
    {
        int direction = (int)animator.direction;

        Vector3 spawnPosition = transform.position +
            new Vector3(fireOffset.x * direction, fireOffset.y, 0);

        Quaternion rotation = Quaternion.Euler(
            0, 0,
            direction == 1 ? -90 : 90
        );

        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.right * direction * bulletSpeed;
        
        bullet.transform.position += new Vector3(0, 0.3f, 0);
        // 총알에 플레이어 공격력과 공격자 정보 전달
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetDamage(
            stat.GetResultValue("attackDamage"),
            health
        );


        Destroy(bullet, 1f);
    }
}