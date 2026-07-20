using System.Collections;
using UnityEngine;

[System.Serializable]
    public struct AttackRange
    {
        public Vector2 offset, size;

        public bool drawGizmos;
    }
public class PlayerBattle : MonoBehaviour
{
    public PlayerMovement movement;
    
    public EntityHealth health;
    
    public EntityStat stat;

    public float atkCool;
    public float Skill_1_Cool;


    public AttackRange defaultAttack;

    [SerializeField] LayerMask enemyMask;
    [SerializeField] float dashPower, dashTime;//<--------------------니얼굴 윤겔라
    [SerializeField] DamageIndicator indicator;
    public bool inDash;
    void Start()
    {
        health = GetComponent<EntityHealth>();
        stat = GetComponent<EntityStat>();
        movement = GetComponent<PlayerMovement>();

        health.OnDamage(OnHurt);
    }

    void OnHurt(EntityHealth.Context ctx)
    {
        if (inDash)
        {
            ctx.canceled = true;
        }
        if (ctx.canceled)
        {
            return;
        }
        indicator.IndicateDamage(ctx.damage, transform.position + new Vector3(0, 1), Color.red);
    }

    void Update()
    {
        if (atkCool > 0) atkCool -= Time.deltaTime + (1 + stat.GetResultValue("atkSpeed")/100);
    }

    public void Dash(int direction)
    {
        StartCoroutine(dash_(direction));
    }

    IEnumerator dash_(int direction)
    {
        movement.SetVelocity(Vector2.right * direction * dashPower);

        yield return new WaitForSeconds(dashTime);

        movement.SetVelocity(Vector2.zero);
        inDash = false;
    }

    // Update is called once per frame
    public void Attack()
    {
        if (atkCool > 0)
            return;
        atkCool = 0.5f;

        var col = Physics2D.OverlapBoxAll((Vector2)transform.position + defaultAttack.offset,defaultAttack.size,0,enemyMask);

        foreach (var target in col)
        {
            EntityHealth hp = target.GetComponent<EntityHealth>();
            if (hp != null)
            {
                hp.GetDamage(stat.GetResultValue("attackDamage"), health);
            }
        }
    }
    
    public void Skill_1()
    {
        if (Skill_1_Cool > 0)
            return;
        Skill_1_Cool = 10f;
        StartCoroutine(skill_1_());
    }

    IEnumerator skill_1_()
    {
        var atkBuf = new EntityStat.Buf
        {
            Key = "attackDamage",
            mathType = MathType.Increase,
            Value = 2
        };

        var atkspeedBuf = new EntityStat.Buf
        {
            Key = "atkSpeed",
            mathType = MathType.Increase,
            Value = 160
        };
        stat.bufs.Add(atkBuf);
        stat.bufs.Add(atkspeedBuf);

        stat.Calc("attackDamage");
        stat.Calc("atkSpeed");

        yield return new WaitForSeconds(5);

        stat.bufs.Remove(atkBuf);
        stat.bufs.Remove(atkspeedBuf);

        stat.Calc("attackDamage");
        stat.Calc("atkSpeed");

        

    }
    void Draw(AttackRange range)
    {
        if (!range.drawGizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position + range.offset, range.size);
    }
    void OnDrawGizmos() 
    {
        Draw(defaultAttack);
    }
}
