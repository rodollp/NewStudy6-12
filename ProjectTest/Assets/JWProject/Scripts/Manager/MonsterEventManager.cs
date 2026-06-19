using UnityEngine;

public class MonsterEventManager : MonoBehaviour
{
    [SerializeField] private MonsterTracker monsterTracker;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private StageManager stageManager;

    public void RegisterMonster(Monster monster)
    {
        if (monster == null) return;

        monsterTracker.AddMonster(monster);

        monster.OnDead += battleManager.MonsterDeadEvent;
        monster.OnDead += OnMonsterDead;
    }

    private void OnMonsterDead(Monster monster)
    {
        monsterTracker.RemoveMonster(monster);

        if (monsterTracker.AliveCount <= 0)
        {
            stageManager.ClearStage();
        }
    }
}