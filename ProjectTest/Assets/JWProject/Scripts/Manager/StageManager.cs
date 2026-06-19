using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;

public class StageManager : MonoBehaviour
{
    [Header("스테이지 데이터")]
    [SerializeField] private List<StageData> stages;

    [Header("매니저 연결")]
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private MonsterTracker monsterTracker;
    [SerializeField] private PlayerRespawner playerRespawner;

    [Header("플레이어 입력")]
    [SerializeField] private PlayerInputHandler input;

    [Header("현재 스테이지")]
    [SerializeField] private int stageIndex = 0;

    private bool canNextStage = false;

    public int StageIndex => stageIndex;

    private void Awake()
    {
        if (input == null)
            input = FindAnyObjectByType<PlayerInputHandler>();
    }

    private void Start()
    {
        StartStage(stageIndex);
    }

    private void Update()
    {
        if (!canNextStage) return;
        if (input == null) return;

        if (input.NextStagePressed)
        {
            NextStage();
        }
    }

    public void StartStage(int index)
    {
        if (index < 0 || index >= stages.Count)
        {
            Debug.LogError($"존재하지 않는 Stage : {index}");
            return;
        }

        stageIndex = index;
        canNextStage = false;

        monsterTracker.Clear();
        playerRespawner.Respawn();

        StageData data = stages[stageIndex];
        spawnManager.Spawn(data);

        Debug.Log($"{stageIndex} 스테이지 시작");
    }

    public void ClearStage()
    {
        if (IsLastStage())
        {
            GameClear();
            return;
        }

        canNextStage = true;

        Debug.Log($"스테이지 클리어! 다음 스테이지 : {stageIndex + 1}");
        Debug.Log("N키를 누르면 다음 스테이지로 이동");
    }

    private void NextStage()
    {
        canNextStage = false;
        StartStage(stageIndex + 1);
    }

    private bool IsLastStage()
    {
        return stageIndex >= stages.Count - 1;
    }

    private void GameClear()
    {
        Debug.Log("게임 클리어!");
    }
}