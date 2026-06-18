using UnityEngine;
using System.Collections.Generic;

public class MonsterTracker : MonoBehaviour
{
    private readonly List<Monster> aliveMonsters = new();

    public int AliveCount => aliveMonsters.Count;

    public void AddMonster(Monster monster)
    {
        if (monster == null) return;

        aliveMonsters.Add(monster);
    }

    public void RemoveMonster(Monster monster)
    {
        if (monster == null) return;

        aliveMonsters.Remove(monster);
    }

    public void Clear()
    {
        aliveMonsters.Clear();
    }

    public List<Monster> GetAliveMonsters()
    {
        aliveMonsters.RemoveAll(m => m == null);
        return aliveMonsters;
    }

    public Monster GetClosestMonster(Vector3 position)
    {
        aliveMonsters.RemoveAll(m => m == null);

        Monster closest = null;
        float closestDist = float.MaxValue;

        foreach (Monster monster in aliveMonsters)
        {
            float dist = (monster.transform.position - position).sqrMagnitude;

            if (dist < closestDist)
            {
                closestDist = dist;
                closest = monster;
            }
        }

        return closest;
    }
}