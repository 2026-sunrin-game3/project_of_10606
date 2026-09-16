using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    [SerializeField] PlayerController player;
    [SerializeField] Slider bossbar;

    [Header("1번: 조준 3연발")]
    [SerializeField] GameObject bossBulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletSpeed = 8f;
    [SerializeField] float shotDelay = 0.25f;

    [Header("2번: 낙하 공격")]
    [SerializeField] GameObject warningPrefab;
    [SerializeField] GameObject fallingPrefab;
    [SerializeField] float warningTime = 0.8f;
    [SerializeField] float fallHeight = 6f;
    [SerializeField] Vector2 warningOffset = new Vector2(0, -0.7f);
    [SerializeField] int fallingCount = 3;
    [SerializeField] float fallingInterval = 0.35f;

    [Header("3번: 바닥 돌진체")]
    [SerializeField] GameObject groundAttackPrefab;
    [SerializeField] Transform leftSpawnPoint;
    [SerializeField] Transform rightSpawnPoint;
    [SerializeField] float groundAttackSpeed = 10f;
    [SerializeField] float groundAttackDelay = 0.45f;

    [SerializeField] float patternRestTime = 1.5f;

    [Header("4번: 부채꼴 탄막")]
    [SerializeField] int fanBulletCount = 5;
    [SerializeField] float fanAngle = 50f;

    [Header("5번: 원형 탄막")]
    [SerializeField] int circleBulletCount = 8;
    [SerializeField] int circleWaveCount = 3;
    [SerializeField] float circleWaveDelay = 0.4f;
    [SerializeField] float circleRotateAngle = 20f;

    [Header("6번: 추적 구체")]
    [SerializeField] GameObject homingBulletPrefab;
    [SerializeField] int homingBulletCount = 3;
    [SerializeField] float homingBulletDelay = 0.35f;
    [SerializeField] float homingBulletSpeed = 5f;

    [Header("7번: 하늘 탄환비")]
    [SerializeField] int rainBulletCount = 12;
    [SerializeField] float rainBulletDelay = 0.12f;
    [SerializeField] float rainWidth = 7f;
    [SerializeField] float rainHeight = 6f;
    [SerializeField] float rainBulletSpeed = 9f;

    [Header("8번: 양쪽 교차 사격")]
    [SerializeField] Transform[] crossFirePoints;
    [SerializeField] int crossFireWaveCount = 3;
    [SerializeField] float crossFireDelay = 0.4f;
    [SerializeField] float crossFireBulletSpeed = 10f;

    [Header("2페이즈")]
    [SerializeField, Range(0.1f, 0.9f)]
    float phase2StartPercent = 0.5f;

    [SerializeField] float phase2DelayMultiplier = 0.6f;
    [SerializeField] float phase2SpeedMultiplier = 1.25f;

    /*[Header("10번: 가로 레이저")]
    [SerializeField] GameObject laserWarningPrefab;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] Transform laserCenterPoint;
    [SerializeField] float laserWarningTime = 0.8f;
    [SerializeField] float laserDuration = 0.5f;
    [SerializeField] float laserHeightOffset = 0.5f;*/

    int phase = 1;

    bool inPattern;
    int patternIndex;

    bool phase2Started = false;

    bool phaseTransition = false;

    float start = 0f;

    //Animator animator;
    
    //void Start()
    //{
    //    animator = GetComponent<Animator>();
    //}

    protected override void MobUpdate()
    {
        bossbar.value = health.health / health.maxHealth;

        start += Time.deltaTime;

        if (start < 5f)
        {
            if (start < 3f)
            transform.position += new Vector3(0, 1.2f * Time.deltaTime, 0);
            return;
        }
             // 5초가 되기 전까지는 아래 코드 실행 안 함

        if (!inPattern)
        {
            StartCoroutine(NextPattern());
        }

        if (!phase2Started &&
    health.health <= health.maxHealth * phase2StartPercent)
        {
            phase2Started = true;
            StartCoroutine(Phase2Animation());
        }

    }
    IEnumerator Phase2Animation()
    {
        boxCollider.enabled = false;

        // 오른쪽으로 이동
        float timer = 0f;
        while (timer < 5f)
        {
            timer += Time.deltaTime;
            transform.position += Vector3.right * 2f * Time.deltaTime;
            yield return null;
        }

        // 잠시 대기
        yield return new WaitForSeconds(2f);

        // 페이즈 변경
        animator.SetInteger("phase", 2);
        phase = 2;

        // 왼쪽으로 이동
        timer = 0f;
        while (timer < 5f)
        {
            timer += Time.deltaTime;
            transform.position += Vector3.left * 2f * Time.deltaTime;
            yield return null;
        }

        boxCollider.enabled = true;
        phase = 2;
        phaseTransition = false;
    }

    IEnumerator NextPattern()
    {
        
        inPattern = true;
        if (phase == 1)
        {
            patternIndex = Random.Range(0, 3);
        }
        else
        {
            patternIndex = Random.Range(3, 16);
        }

        // 0: 3연발 / 1: 낙하 공격
        switch (patternIndex)
        {
            case 0:
                yield return StartCoroutine(ActiveRoutineFallingPattern1());
                //yield return StartCoroutine(ActiveFallingPattern());
                break;

            case 1:
                yield return StartCoroutine(ComboPattern(1));
                //yield return StartCoroutine(GroundRushPattern());
                break;

            case 2:
                yield return StartCoroutine(ComboPattern(2));
                //yield return StartCoroutine(AimBurstPattern());
                break;
            case 3:
                yield return StartCoroutine(ComboPattern(3));
                break;

            case 4:
                yield return StartCoroutine(ComboPattern(4));
                break;
            case 5:
                yield return StartCoroutine(ComboPattern(6));
                break;
            case 6:
                yield return StartCoroutine(SkyRainPattern());
                break;
            case 7:
                yield return StartCoroutine(CrossFirePattern());
                break;
            case 8:
                yield return StartCoroutine(ActiveRoutineFallingPattern1());
                break;
            case 9:
                yield return StartCoroutine(ComboPattern(1));
                break;
            case 10:
                yield return StartCoroutine(ComboPattern(6));
                break;
            case 11:
                yield return StartCoroutine(ComboPattern(7));
                break;
            case 12:
                yield return StartCoroutine(ComboPattern(8));
                break;
            case 13:
                yield return StartCoroutine(ComboPattern(9));
                break;
            case 14:
                yield return StartCoroutine(ComboPattern(10));
                break;
            case 15:
                yield return StartCoroutine(ComboPattern(11));
                break;
            case 16:
                yield return StartCoroutine(ComboPattern(12));
                break;
        }
        //patternIndex = Random.Range(0, 9);



        
        yield return new WaitForSeconds(patternRestTime);
        inPattern = false;
    }
    IEnumerator ComboPattern(int C)
    {
        switch (C)
        {
            case 1 :
                StartCoroutine(AimBurstPattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(ActiveRoutineFallingPattern2());
                break;
            case 2:
                StartCoroutine(GroundRushPattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(ActiveRoutineFallingPattern1());
                break;
            case 3:
                StartCoroutine(GroundRushPattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(ActiveFallingPattern());
                break;
            case 4:
                StartCoroutine(SkyRainPattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(ActiveRoutineFallingPattern2());
                break;
            case 5:
                StartCoroutine(HomingPattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(AimBurstPattern());

                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(AimBurstPattern());
                break;
            case 6:
                StartCoroutine(CirclePattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(AimBurstPattern());
                break;
            case 7:
                StartCoroutine(GroundRushPattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(SkyRainPattern());
                break;
            case 8:
                StartCoroutine(GroundRushPattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(1f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(CrossFirePattern());
                break;
            case 9:
                StartCoroutine(GroundRushPattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(AimBurstPattern());
                break;
            case 10:
                StartCoroutine(CirclePattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(CirclePattern());
                break;
            case 11:
                StartCoroutine(FanPattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(1f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(FanPattern());

                yield return new WaitForSeconds(1f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(FanPattern());
                yield return new WaitForSeconds(1f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(FanPattern());
                break;
            case 12:
                StartCoroutine(AimBurstPattern());

                // 0.4초 뒤 탄환비도 시작
                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(GroundRushPattern());

                yield return new WaitForSeconds(0.4f);

                // 탄환비가 끝날 때까지 대기
                yield return StartCoroutine(CrossFirePattern());
                break;
        }
        
    }

    IEnumerator AimBurstPattern()
    {
        animator.SetInteger("attackType", 1);
        for (int i = 0; i < 3; i++)
        {
            ShootAtPlayer();
            yield return new WaitForSeconds(shotDelay);
        }
        yield return new WaitForSeconds(shotDelay);
        yield return new WaitForSeconds(shotDelay);
        yield return new WaitForSeconds(shotDelay);
        for (int i = 0; i < 3; i++)
        {
            ShootAtPlayer();
            yield return new WaitForSeconds(shotDelay);
        }
        yield return new WaitForSeconds(shotDelay);
        yield return new WaitForSeconds(shotDelay);
        yield return new WaitForSeconds(shotDelay);
        for (int i = 0; i < 3; i++)
        {
            ShootAtPlayer();
            yield return new WaitForSeconds(shotDelay);
        }
    }

    void ShootAtPlayer()
    {
        Vector2 direction =
            (player.transform.position - firePoint.position).normalized;

        ShootInDirection(direction);
    }

    void ShootInDirection(Vector2 direction)
    {
        GameObject bullet = Instantiate(
            bossBulletPrefab,
            firePoint.position,
            Quaternion.identity
        );

        bullet.transform.right = direction;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;

        BossBullet bossBullet = bullet.GetComponent<BossBullet>();

        bossBullet.SetDamage(
            stat.GetResultValue("attackDamage"),
            health
        );

        Destroy(bullet, 5f);
    }

    IEnumerator DashPattern()
    {
        float speed = 12f;

        Vector3 center = transform.position;
        Vector3 left = new Vector3(-7f, center.y, 0);
        Vector3 right = new Vector3(7f, center.y, 0);

        yield return DashTo(left, speed);

        yield return StartCoroutine(CirclePattern());

        yield return DashTo(right, speed);

        yield return StartCoroutine(CirclePattern());

        yield return DashTo(center, speed);
    }

    IEnumerator DashTo(Vector3 target, float speed)
    {
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                speed * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator ActiveFallingPattern()
    {
        for (int i = 0; i < fallingCount; i++)
        {
            // 경고가 여러 개 동시에/겹쳐서 진행됨
            StartCoroutine(FallingPattern());

            yield return new WaitForSeconds(fallingInterval);
        }


        // 마지막 경고가 끝나고 낙하물이 생성될 시간까지 대기
        yield return new WaitForSeconds(warningTime);

        for (int i = 0; i < fallingCount+5; i++)
        {
            // 경고가 여러 개 동시에/겹쳐서 진행됨
            StartCoroutine(FallingPattern());

            yield return new WaitForSeconds(fallingInterval/10*7);
        }


        // 마지막 경고가 끝나고 낙하물이 생성될 시간까지 대기
        yield return new WaitForSeconds(warningTime);

        for (int i = 0; i < fallingCount+10; i++)
        {
            // 경고가 여러 개 동시에/겹쳐서 진행됨
            StartCoroutine(FallingPattern());

            yield return new WaitForSeconds(fallingInterval/10*3);
        }


        // 마지막 경고가 끝나고 낙하물이 생성될 시간까지 대기
        yield return new WaitForSeconds(warningTime);
    }
    IEnumerator FallingPattern()
    {
        // 경고가 나타날 위치를 먼저 기억
        Vector3 targetPosition = player.transform.position;
        targetPosition += (Vector3)warningOffset;
        targetPosition.y = 1f;

        // 바닥 경고 표시
        GameObject warning = Instantiate(
            warningPrefab,
            targetPosition,
            Quaternion.identity
        );

        yield return new WaitForSeconds(warningTime);

        Destroy(warning);

        // 경고 표시 위에서 낙하물 생성
        Vector3 fallingPosition = targetPosition + Vector3.up * fallHeight;

        GameObject falling = Instantiate(
            fallingPrefab,
            fallingPosition,
            Quaternion.identity
        );

        BossFallingAttack fallingAttack =
            falling.GetComponent<BossFallingAttack>();

        fallingAttack.SetDamage(
            stat.GetResultValue("attackDamage"),
            health
        );

        Destroy(falling, 5f);
    }

    IEnumerator GroundRushPattern()
    {
        // 왼쪽과 오른쪽에서 번갈아 총 4번 돌진체 생성
        for (int i = 0; i < 10; i++)
        {
            bool fromLeft = i % 2 == 0;

            Transform spawnPoint =
                fromLeft ? leftSpawnPoint : rightSpawnPoint;

            Vector2 direction =
                fromLeft ? Vector2.right : Vector2.left;

            SpawnGroundAttack(spawnPoint, direction);

            yield return new WaitForSeconds(groundAttackDelay);
        }
    }

    void SpawnGroundAttack(Transform spawnPoint, Vector2 direction)
    {
        GameObject groundAttack = Instantiate(
            groundAttackPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        // 스프라이트가 오른쪽을 향한 상태를 기준으로 회전
        groundAttack.transform.right = direction;

        BossGroundAttack attack =
            groundAttack.GetComponent<BossGroundAttack>();

        attack.SetDamage(
            stat.GetResultValue("attackDamage"),
            health,
            direction * groundAttackSpeed
        );

        Destroy(groundAttack, 5f);
    }

    IEnumerator FanPattern()
    {
        Vector2 baseDirection =
            (player.transform.position - firePoint.position).normalized;

        float startAngle = -fanAngle / 2f;
        float angleStep = fanAngle / (fanBulletCount - 1);

        for (int i = 0; i < fanBulletCount; i++)
        {
            float angle = startAngle + angleStep * i;

            Vector2 direction =
                Quaternion.Euler(0, 0, angle) * baseDirection;

            ShootInDirection(direction);
        }

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator CirclePattern()
    {
        for (int wave = 0; wave < circleWaveCount; wave++)
        {
            for (int i = 0; i < circleBulletCount; i++)
            {
                float angle =
                    (360f / circleBulletCount) * i +
                    circleRotateAngle * wave;

                Vector2 direction = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad)
                );

                ShootInDirection(direction);
            }

            yield return new WaitForSeconds(circleWaveDelay);
        }
    }
    IEnumerator ActiveRoutineFallingPattern1()
    {
        for (int i = 0; i < 10; i++)
        {
            // 경고가 여러 개 동시에/겹쳐서 진행됨
            StartCoroutine(RoutineFallingPattern(0 - i / 1));
            StartCoroutine(RoutineFallingPattern(0 + i / 1));
            yield return new WaitForSeconds(fallingInterval);
        }
        for (int i = 0; i < 10; i++)
        {
            // 경고가 여러 개 동시에/겹쳐서 진행됨
            StartCoroutine(RoutineFallingPattern(-7 + i / 1));
            StartCoroutine(RoutineFallingPattern(7 - i / 1));
            yield return new WaitForSeconds(fallingInterval);
        }
        for (int i = 0; i < 10; i++)
        {
            // 경고가 여러 개 동시에/겹쳐서 진행됨
            StartCoroutine(RoutineFallingPattern(-7 + i * 2));
            yield return new WaitForSeconds(fallingInterval/4);
        }
        // 마지막 경고가 끝나고 낙하물이 생성될 시간까지 대기
        yield return new WaitForSeconds(warningTime);
    }

    IEnumerator ActiveRoutineFallingPattern2()
    {
        
        for (int i = 0; i < 10; i++)
        {
            // 경고가 여러 개 동시에/겹쳐서 진행됨
            StartCoroutine(RoutineFallingPattern(-7 + i * 2));
            yield return new WaitForSeconds(fallingInterval / 10);
        }

        yield return new WaitForSeconds(fallingInterval * 2);
        
        for (int i = 0; i < 10; i++)
        {
            // 경고가 여러 개 동시에/겹쳐서 진행됨
            StartCoroutine(RoutineFallingPattern(-6 + i * 2));
            yield return new WaitForSeconds(fallingInterval / 10);
        }

        yield return new WaitForSeconds(fallingInterval * 2);

        for (int i = 0; i < 10; i++)
        {
            // 경고가 여러 개 동시에/겹쳐서 진행됨
            StartCoroutine(RoutineFallingPattern(-7 + i * 2));
            yield return new WaitForSeconds(fallingInterval / 10);
        }

        yield return new WaitForSeconds(fallingInterval * 2);

        for (int i = 0; i < 10; i++)
        {
            // 경고가 여러 개 동시에/겹쳐서 진행됨
            StartCoroutine(RoutineFallingPattern(-6 + i * 2));
            yield return new WaitForSeconds(fallingInterval / 10);
        }
        // 마지막 경고가 끝나고 낙하물이 생성될 시간까지 대기
        yield return new WaitForSeconds(warningTime);
    }
    IEnumerator RoutineFallingPattern(float x)
    {
        // 경고가 나타날 위치를 먼저 기억
        Vector3 targetPosition = player.transform.position;
        targetPosition.x = x;
        targetPosition.y = 1f;

        // 바닥 경고 표시
        GameObject warning = Instantiate(
            warningPrefab,
            targetPosition,
            Quaternion.identity
        );

        yield return new WaitForSeconds(warningTime);

        Destroy(warning);

        // 경고 표시 위에서 낙하물 생성
        Vector3 fallingPosition = targetPosition + Vector3.up * fallHeight;

        GameObject falling = Instantiate(
            fallingPrefab,
            fallingPosition,
            Quaternion.identity
        );

        BossFallingAttack fallingAttack =
            falling.GetComponent<BossFallingAttack>();

        fallingAttack.SetDamage(
            stat.GetResultValue("attackDamage"),
            health
        );

        Destroy(falling, 5f);
    }
    IEnumerator HomingPattern()
    {
        for (int i = 0; i < homingBulletCount; i++)
        {
            GameObject bullet = Instantiate(
                homingBulletPrefab,
                firePoint.position,
                Quaternion.identity
            );

            HomingBossBullet homingBullet =
                bullet.GetComponent<HomingBossBullet>();

            homingBullet.SetDamage(
                stat.GetResultValue("attackDamage"),
                health,
                player.transform,
                homingBulletSpeed
            );

            Destroy(bullet, 5f);

            yield return new WaitForSeconds(homingBulletDelay);
        }
    }
    IEnumerator SkyRainPattern()
    {
        for (int i = 0; i < rainBulletCount; i++)
        {
            // 플레이어 주변의 랜덤한 X 위치, 위쪽에서 생성
            float randomX = Random.Range(
                -rainWidth / 2f,
                rainWidth / 2f
            );

            Vector3 spawnPosition = player.transform.position +
                new Vector3(randomX, rainHeight+2f, 0);

            GameObject bullet = Instantiate(
                bossBulletPrefab,
                spawnPosition,
                Quaternion.identity
            );

            // 총알 그림이 아래를 향하게
            bullet.transform.right = Vector2.down;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.down * rainBulletSpeed;

            BossBullet bossBullet = bullet.GetComponent<BossBullet>();

            bossBullet.SetDamage(
                stat.GetResultValue("attackDamage"),
                health
            );

            Destroy(bullet, 5f);

            yield return new WaitForSeconds(rainBulletDelay);
        }
    }
    IEnumerator CrossFirePattern()
    {
        for (int wave = 0; wave < crossFireWaveCount; wave++)
        {
            foreach (Transform spawnPoint in crossFirePoints)
            {
                if (spawnPoint == null)
                    continue;

                Vector2 direction =
                    (player.transform.position - spawnPoint.position).normalized;

                GameObject bullet = Instantiate(
                    bossBulletPrefab,
                    spawnPoint.position,
                    Quaternion.identity
                );

                bullet.transform.right = direction;

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.linearVelocity = direction * crossFireBulletSpeed;

                BossBullet bossBullet = bullet.GetComponent<BossBullet>();

                bossBullet.SetDamage(
                    stat.GetResultValue("attackDamage"),
                    health
                );

                Destroy(bullet, 5f);
            }

            yield return new WaitForSeconds(crossFireDelay);
        }
    }
    float GetDelay(float normalDelay)
    {
        return phase == 2
            ? normalDelay * phase2DelayMultiplier
            : normalDelay;
    }

    float GetSpeed(float normalSpeed)
    {
        return phase == 2
            ? normalSpeed * phase2SpeedMultiplier
            : normalSpeed;
    }
    /*IEnumerator LaserPattern()
    {
        // 플레이어의 현재 높이를 기억
        Vector3 laserPosition = new Vector3(
            laserCenterPoint.position.x,
            player.transform.position.y + laserHeightOffset,
            0
        );
        laserPosition.y = 2f;

        // 먼저 경고선 표시
        GameObject warning = Instantiate(
            laserWarningPrefab,
            laserPosition,
            Quaternion.identity
        );

        yield return new WaitForSeconds(laserWarningTime);

        Destroy(warning);

        // 실제 레이저 생성
        GameObject laser = Instantiate(
            laserPrefab,
            laserPosition,
            Quaternion.identity
        );

        BossLaser bossLaser = laser.GetComponent<BossLaser>();

        bossLaser.SetDamage(
            stat.GetResultValue("attackDamage"),
            health
        );

        Destroy(laser, laserDuration);

        yield return new WaitForSeconds(laserDuration);
    }*/
}