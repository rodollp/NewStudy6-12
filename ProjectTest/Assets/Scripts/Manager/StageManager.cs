using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class StageManager : MonoBehaviour
{
    [Header("스폰 매니저 연결")]
    public SpawnManager spawnManager;

    [Header("현재 스테이지")]
    public int stageIndex = 0;

    [Header("시작 위치")]
    [SerializeField] Transform startPoint;
    [SerializeField] Transform player;

    [Header("현재 살아있는 몬스터")]
    public List<Monster> aliveMonsters = new();


    bool canNextStage = false;
    void Start()
    {
        StartStage(stageIndex);
    }

    private void Update()
    {
        if (canNextStage == false) return;
        if (Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
        {
            canNextStage = false;
            Debug.Log("스테이지 시작!");
            StartStage(stageIndex + 1);
            
        }
    }

    
    public void StartStage(int index)
    {
        canNextStage = false ;

        stageIndex = index;

        aliveMonsters.Clear();

        Rigidbody rb = player.GetComponent<Rigidbody>();

        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.position = startPoint.position;
        rb.rotation = startPoint.rotation;

        PlayerStatus p = player.GetComponent<PlayerStatus>();
        
        p.FullHeal();

        // 스테이지 시작 시 스폰 요청
        spawnManager.Spawn(stageIndex);
    }


    public List<Monster> GetAliveMonsters()
    {
        aliveMonsters.RemoveAll(m => m == null);
        return aliveMonsters;
    }
    public void AddMonster(Monster monster)
    {
        aliveMonsters.Add(monster);
    }

    public Monster ShortMagnitude(Vector3 position)
    {
        Monster shortmag = null;
        float shortmagDist = float.MaxValue;

        foreach (Monster monster in aliveMonsters)
        {
            if (monster == null) continue;

            float dist = (monster.transform.position - position).sqrMagnitude;
            if (dist < shortmagDist)
            {
                shortmagDist = dist;
                shortmag = monster;
            }
        }
        return shortmag;

    }
    public void OnMonsterDead(Monster monster)
    {
        if (monster == null) return;
        if (monster.IsDead == false) return;

        aliveMonsters.Remove(monster);

        if (aliveMonsters.Count <= 0)
        {
            if (stageIndex >= spawnManager.stages.Count - 1)
            {
                Debug.Log("게임 클리어");
                return;
            }

            Debug.Log($"스테이지 클리어! 다음 스테이지 : {stageIndex + 1}");
            Debug.Log($"다음 스테이지로 넘어가시겠습니까? (N키를 눌러서 스테이지 이동) ");
            canNextStage = true;
        }
    }
}