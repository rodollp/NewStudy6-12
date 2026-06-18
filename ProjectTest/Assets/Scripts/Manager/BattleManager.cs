using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] PlayerStatus player;
    [SerializeField] StageManager stageManager;

    public void RegisterMonster(Monster monster)
    {
        stageManager.AddMonster(monster);

        monster.OnDead += MonsterDeadEvent;
    }

    public void MonsterDeadEvent(Monster monster)
    {
        TakeReward(monster);
        stageManager.OnMonsterDead(monster);
    }

    void TakeReward(Monster monster)
    {
        player.AddExp(monster.Reward.ExpReward);
        player.AddGold(monster.Reward.Gold);
    }
}