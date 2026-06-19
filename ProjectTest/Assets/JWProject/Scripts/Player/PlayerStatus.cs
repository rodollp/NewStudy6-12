using UnityEngine;

public class PlayerStatus : Creature
{
    [SerializeField] private int level = 1;
    [SerializeField] private int exp = 0;
    [SerializeField] private int expToNext = 10;
    [SerializeField] private int money = 0;
    [SerializeField] private int healAmount = 30;
    public int Level => level;
    public int Exp => exp;
    public int ExpToNext => expToNext;
    public int HealAmount => healAmount;    
    public int Money => money;

    protected override void Awake()
    {
        base.Awake();
    }

    public void AddExp(int amount)
    {
        exp += amount;

        while (exp >= expToNext)
        {
            LevelUp();
        }
    }

    public void AddGold(int amount)
    {
        money += amount;
    }

    public bool UseGold(int amount)
    {
        if (money < amount)
        {
            Debug.Log("돈이 부족합니다");
            return false;
        }

        money -= amount;
        return true;
    }
    void LevelUp()
    {
        level++;
        exp -= expToNext;
        expToNext += 5;

        maxHp += 3;
        CurrentHp += 3;
        atk += 3;

        Debug.Log("Level Up! " + level);
    }

    public void Heal(int amount)
    {
        CurrentHp += amount;
        Debug.Log($"{Name}은 {amount}만큼 회복했습니다. 현재 체력 : {CurrentHp}");
    }

    public void GrowForce(int amount)
    {
        Atk += amount;
        Debug.Log($"{Name}의 공격력이 {amount}만큼 올랐습니다 . 현재 공격력:  {Atk}");
    }

    public void Heal()
    {
        Heal(HealAmount);
    }

    public void FullHeal()
    {
        CurrentHp = maxHp;
        Debug.Log("스테이지 시작으로 인한 체력 회복!");
    }

    public override void TakeDamage(int damage)
    {
        CurrentHp -= damage;

        Debug.Log($"플레이어가 {damage} 피해를 받음 / 남은 HP: {CurrentHp}");

        if (CurrentHp <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        Debug.Log("게임 오버");
    }
}
