using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;

public class SpawnManager : MonoBehaviour
{
    [Header("섹터")]
    [SerializeField] private Transform sector;

    [Header("몬스터 부모")]
    [SerializeField] private Transform monstersParent;

    [Header("이벤트 매니저")]
    [SerializeField] private MonsterEventManager monsterEventManager;

    private readonly List<Transform> rooms = new();

    private void Awake()
    {
        CollectRooms();
    }

    private void CollectRooms()
    {
        if (sector == null)
        {
            Debug.LogError("Sector가 연결되지 않음");
            return;
        }

        rooms.Clear();

        foreach (Transform room in sector)
        {
            if (room.Find("SpawnPoints") == null)
                continue;

            rooms.Add(room);
        }
    }

    public void Spawn(StageData data)
    {
        if (data == null)
        {
            Debug.LogError("StageData가 없음");
            return;
        }

        if (rooms.Count == 0)
        {
            Debug.LogError("생성 가능한 방이 없음");
            return;
        }

        if (data.isBossStage)
        {
            SpawnOne(data.boss);
            return;
        }

        int spawnCount = Random.Range(data.minSpawnCount, data.maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject prefab = data.monsters[Random.Range(0, data.monsters.Count)];
            SpawnOne(prefab);
        }
    }

    private void SpawnOne(GameObject prefab)
    {
        if (prefab == null) return;

        Transform point = GetRandomSpawnPoint();

        GameObject obj = Instantiate(
            prefab,
            point.position,
            Quaternion.identity,
            monstersParent
        );

        Monster monster = obj.GetComponent<Monster>();

        monsterEventManager.RegisterMonster(monster);
    }

    private Transform GetRandomSpawnPoint()
    {
        Transform room = rooms[Random.Range(0, rooms.Count)];
        Transform pointsParent = room.Find("SpawnPoints");

        return pointsParent.GetChild(Random.Range(0, pointsParent.childCount));
    }
}