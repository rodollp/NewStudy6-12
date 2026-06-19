using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct Reward
{
    public int _gold;
    public int _expReward;

    public int Gold => _gold;
    public int ExpReward => _expReward;
}



public class Monster : Creature
{

    [Header("몬스터 처치 시 드랍")]
    [SerializeField] Reward reward;
    [SerializeField] public GameObject[] dropItem;
    
    
    
    public Reward Reward => reward;

    public event Action<Monster> OnDead;

    public bool IsDead = false;
    public override void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp < 0)
            currentHp = 0;

        Debug.Log($"{Name}이(가) {damage} 피해를 받음 / 남은 HP: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        if (IsDead) return;
        IsDead = true;
        currentHp = 0;
        Debug.Log($"{Name} 사망");

        
        OnDead?.Invoke( this );
        Destroy(gameObject);

    }

}
