using UnityEngine;

public class Creature : MonoBehaviour
{
    [Header(" 이름")]
    [SerializeField] protected string _name;
    [Header(" 체력")]
    [SerializeField]
    protected int maxHp = 100;
    [Header(" 공격력")]
    [SerializeField] protected int atk = 10;



    protected int currentHp;
    public string Name => _name;
    public int MaxHp => maxHp;

    public int CurrentHp
    {
        get => currentHp;
        protected set
        {
            currentHp = Mathf.Clamp(value, 0, maxHp);
        }
    }
    public int Atk
    {
        get => atk;

        protected set
        {
            atk = Mathf.Clamp(value, 0, int.MaxValue);
        }
    }

    protected virtual void Awake()
    {
        maxHp = Mathf.Max(1, maxHp);

        CurrentHp = maxHp;
    }

    public virtual void TakeDamage(int damage)
    {
        CurrentHp -= damage;
        Debug.Log($"{Name}이(가) {damage}만큼 피해를 입었습니다");
        if (currentHp <= 0)
        {
            Die();
        }


    }

    protected virtual void Die()
    {
        CurrentHp = 0;
        Debug.Log($"{Name} 사망");
        Destroy(gameObject);

    }

}

