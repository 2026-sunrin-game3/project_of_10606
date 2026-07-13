using System.Collections.Generic;
using UnityEngine;

public enum MathType
{
    Increase,
    Decrease,
    Add,
    Remove
}

public class EntityStat : MonoBehaviour
{
    Dictionary<string, float> baseValue = new();

    Dictionary<string, float> resultValue = new();

    public List<Buff> buffs = new();
    public struct Buff
    {
        public string key;
        public MathType mathType;
        public float Value;
    }
    [System.Serializable]
    struct StatValue
    {
        public string Key;
        public float Value;
    }
    
    [SerializeField]
    List<StatValue> defaultStat = new()
    {
        new StatValue{ Key = "attackDamage", Value=3 },
        new StatValue{ Key = "defense", Value=0 },
        new StatValue{ Key = "increaseDamage", Value=0 },
        new StatValue{ Key = "critPer", Value=0 },
        new StatValue{ Key = "critMul", Value = 3},
        new StatValue{ Key = "hurtDamage", Value=0 },
        new StatValue{ Key = "atkSpeed", Value = 0 },
        new StatValue{ Key = "moveSpeed", Value = 0 },
        new StatValue{ Key = "atkCool", Value = 4},
    };
    //공격력, 방어력, 치명타 피해, 치명타 확률, 받는 피해 증가, 공격 속도, 이동 속도
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public float attackDamage,defense, increaseDamage;
    //public float critPer, critMul, hurtDamage, atkSpeed, moveSpeed;

    void Start()
    { 

        foreach (StatValue val in defaultStat)
        {
            baseValue[val.Key] = val.Value;
            Calc(val.Key);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float GetResultValue(string key)
    {
        return resultValue[key];
    }

    public float Calc(string key)
    {
        float value = baseValue[key];
        float increase = 100;

        foreach (Buff buff in buffs)
        {
            switch (buff.mathType)
            {
                case MathType.Increase : increase -= buff.Value; break;
                case MathType.Decrease: increase -= buff.Value; break;
                case MathType.Add: value += buff.Value; break;
                case MathType.Remove: value -= buff.Value; break;
            }
        }

        return resultValue[key] = value * increase * 0.01f;
    }

}
