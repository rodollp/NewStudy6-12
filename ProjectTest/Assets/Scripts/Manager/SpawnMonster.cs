using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;


public class SpawnManager : MonoBehaviour
{
    [Header("섹터 프리펩")]
    [SerializeField] Transform sector;

    [Header("몬스터 저장 오브젝트")]
    public Transform monstersParent;
    [Header("스테이지 매니져")]
    public StageManager stageManager;
    [Header("스테이지 데이터 넣기")]
    public List<StageData> stages;
    [Header("전투 이벤트 연결")]
    [SerializeField] BattleManager battleManager;

    //rooms의 위치
    private List<Transform> rooms = new List<Transform>();

    private void Awake()
    {
        CollectRooms();
    }

    //Sector에서 Room 추가
    void CollectRooms()
    {
        if(sector ==  null)
        {
            Debug.LogError("연결되지 않음");
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

    //Room에 있는 SpawnPoint를 랜덤으로 선택
    Transform GetRandomSpawnPoint()
    {
        Transform room = rooms[Random.Range(0, rooms.Count)];

        Transform pointsParent = room.Find("SpawnPoints");

        Transform point = pointsParent.GetChild(Random.Range(0, pointsParent.childCount));

        return point;
    }

    void RegisterMonsterEvent(Monster monster)
    {
        if (monster == null) return;
        stageManager.AddMonster(monster);
        monster.OnDead += battleManager.MonsterDeadEvent;
        monster.OnDead += stageManager.OnMonsterDead;
    }

    public void Spawn(int stage)
    {

        if(rooms.Count == 0)
        {
            Debug.LogError("생성된 방이 없음");
            return;
        }

        //인덱스 위치에서 벗어나면 리턴
        if (stage < 0 || stage >= stages.Count)
        {
            Debug.LogError($"존재하지 않는 Stage : {stage}");
            return;
        }

        StageData data = stages[stage];

        if (data.isBossStage)
        {
            Transform point = GetRandomSpawnPoint();

            GameObject boss = Instantiate(data.boss, point.position, Quaternion.identity, monstersParent);

            Monster bossMonster = boss.GetComponent<Monster>();
           
            RegisterMonsterEvent(bossMonster);


            return;
        }

        //Data 안에 설정해 놓은 최소값, 최대값에서 랜덤으로 스폰 수 설정
        int spawnCount = Random.Range(data.minSpawnCount, data.maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            Transform point = GetRandomSpawnPoint();

            GameObject monster = data.monsters[Random.Range(0, data.monsters.Count)];


            GameObject mon = Instantiate(monster, point.position, Quaternion.identity, monstersParent);
            Monster spawnMonster = mon.GetComponent<Monster>();

            RegisterMonsterEvent(spawnMonster);
        }


    }
}