using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] PlayerStatus player;

    public void MonsterDeadEvent(Monster monster)
    {
        TakeReward(monster);
        DropItem(monster);
    }

    void TakeReward(Monster monster)
    {
        player.AddExp(monster.Reward.ExpReward);
        player.AddGold(monster.Reward.Gold);
    }

    void DropItem(Monster monster)
    {
        if (monster.dropItem == null) return;
        if (monster.dropItem.Length == 0) return;

        int index = Random.Range(0, monster.dropItem.Length);

        GameObject itemPrefab = monster.dropItem[index];

        if (itemPrefab == null) return;
        Vector3 spawnPoint = monster.transform.position + Vector3.up * 1f;
        Instantiate(itemPrefab, spawnPoint, Quaternion.identity);
        Debug.Log("아이템 드랍!");
    }
}