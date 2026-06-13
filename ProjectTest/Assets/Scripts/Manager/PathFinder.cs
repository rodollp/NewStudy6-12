using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    //RoomNode에 있는 것을 리스트화
    public List<RoomNode> rooms = new();

    // 플레이어가 있는 룸 노드
    RoomNode playerRoom;
    // 몬스터가 있는 룸 노드
    RoomNode targetRoom;

    // 플레이어 위치
    [SerializeField] Transform player;
    //스테이지 매니져로 살아있는 몬스터 추적
    [SerializeField] StageManager stageManager;

    // playerRoom에서 targetRoom까지 이어지는 경로 저장 리스트
    List<RoomNode> path = new();

    [Header("실수로 무한 검색에 빠지게 하지않는 검색 반복 횟수")]
    [SerializeField] private int maxIterations = 100;

    private bool stoppedByLimit;

    private void Awake()
    {
        //현재 씬에 존재하는 RoomNode 컴포넌트를 찾아서 배열로 가져옴
        RoomNode[] found = FindObjectsByType<RoomNode>(FindObjectsSortMode.None);
        //rooms에 모든 룸 노드를 넣기
        rooms.AddRange(found);

    }

    private void Update()
    {
        UpdateRooms();
        FindPath();
    }

    // 플레이어랑 몬스터 위치에서 가장 가까운 RoomNode를 갱신하는 메소드
    void UpdateRooms()
    {
        // 플레이어 위치 기준으로 현재 방 찾기
        playerRoom = FindClosestRoom(player.position);

        // 현재 살아있는 몬스터 중 가장 가까운 몬스터 찾기
        Monster target = stageManager.ShortMagnitude(player.position);

        if (target == null)
        {
            targetRoom = null;
            path.Clear();
            return;
        }

        // 몬스터 위치 기준으로 목표 방 찾기
        targetRoom = FindClosestRoom(target.transform.position);
    }

    // 가까운 룸 찾기
    RoomNode FindClosestRoom(Vector3 position)
    {
        //가장 가까운 룸 노드를 저장할 공간
        RoomNode closest = null;
        // 무조건 첫번째 방이 closest으로 저장되게 하려고 최대값을 사용
        float closestDist = float.MaxValue;

        // List<RoomNode> rooms에서 하나씩 꺼내며 검사
        foreach (RoomNode room in rooms)
        {
            // room의 위치에서 전달받은 position 사이의 거리를 제곱하여 나타낸 dist
            float dist =(room.transform.position - position).sqrMagnitude;

            // 현재 방이 지금까지 찾은 방보다 더 가까우면
            // closestDist를 그 거리로 갱신하고 closest를 현재 room으로 바꾼다
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = room;
            }
        }

        return closest;
    }

    void FindPath()
    {
        path.Clear();
        stoppedByLimit = false;

        if (playerRoom == null || targetRoom == null)
            return;
        //검사를 진행할  후보 노드
        List<RoomNode> openList = new List<RoomNode>();
        //검사가 끝난 노드
        HashSet<RoomNode> closedList = new HashSet<RoomNode>();
        // 현재 방에 오기 전까지 어디 방에서 왔는지 저장
        Dictionary<RoomNode, RoomNode> cameFrom = new Dictionary<RoomNode, RoomNode>();
        //시작방에서 현재까지의 이동 비용
        Dictionary<RoomNode, float> gCost = new Dictionary<RoomNode, float>();
        //Gcost + 목표까지의 예상 비용
        Dictionary<RoomNode, float> fCost = new Dictionary<RoomNode, float>();

        //플레이어 룸 노드를 첫번째로
        openList.Add(playerRoom);
        //Gcost도 첫번째로
        gCost[playerRoom] = 0;
        //첫번째 넣은 곳에서 Fcost 비용 검사 // Heuristic으로 비용 검사
        fCost[playerRoom] = Hcost(playerRoom, targetRoom);
        //검색 횟수
        int currentCountByRoom = 0;
        // 후보가 남았으면 반복
        while (openList.Count > 0)
        {
            //반복문 돌때마다 검색 횟수 증가
            currentCountByRoom++;
            //만약 최대 한도에 초과하여 검사시작시 검색취소
            if(currentCountByRoom > maxIterations)
            {
                stoppedByLimit=true;
                return;
            }
            // current에 제일 낮은 fcost 방 넣기
            RoomNode current = GetLowestFCostNode(openList, fCost);

            //current가 타겟룸이면 경로를 봐야하니 BuildPath 후 리턴
            if (current == targetRoom)
            {
                BuildPath(cameFrom, current);
                return;
            }

            //current에 들렸으니까 후보에서 제거
            openList.Remove(current);
            //검사 끝난 곳에 넣기
            closedList.Add(current);

            //current에 연결된 방에서 next 방을 하나씩 꺼내서 검사.
            foreach (RoomNode next in current.GetNeighbors())
            {
                //방이 없으면 건뛰
                if (next == null) continue;
                //검색한 방이 포함되어있으면 건너뛰기
                if (closedList.Contains(next))
                    continue;
                //새로운 Gcost 비용 계산 gcost는 첫 시작점부터 지금까지의 비용이니 current의 Gcost부터 next까지 가는데 든 비용 계산을 newGcost에 대입
                float newGCost = gCost[current] + Vector3.Distance(current.transform.position,next.transform.position);

                //Gcost에 next 방이 없거나 newGcost가 더 작으면 
                if (!gCost.ContainsKey(next) || newGCost < gCost[next])
                {
                    //오기직전의 방이 current였다고 저장
                    cameFrom[next] = current;
                    //더 싼 비용이니 이것을 newGcost로 지정
                    gCost[next] = newGCost;
                    //그러므로 fcost도 바꾸기
                    fCost[next] = newGCost + Hcost(next, targetRoom);

                    //후보에 없었다면 추가
                    if (!openList.Contains(next))
                        openList.Add(next);
                }
            }
        }
    }

    // 룸 A에서부터 룸 B까지의 거리비용 계산, 즉 H Cost
    float Hcost(RoomNode a, RoomNode b)
    {
        return Vector3.Distance(a.transform.position,b.transform.position
        );
    }

    //가장 싼 비용의 fcost 룸을 확인하는 메소드 / 후보에서 Dictionary를 이용하여 계산
    RoomNode GetLowestFCostNode(List<RoomNode> openList,Dictionary<RoomNode, float> fCost)
    {
        // 제일 첫번째 후보를 가장 싼 비용의 룸으로 두기
        RoomNode bestNode = openList[0];
        //그러므로 bestCost는 bestNode를 첫번째로 둔다
        float bestCost = fCost[bestNode];

        //후보에 있는 노드 검사
        foreach (RoomNode node in openList)
        {
            //후보에 있는 방이 더 비용이 싸면 그 방이 bestNode이자 bestcost
            if (fCost[node] < bestCost)
            {
                bestNode = node;
                bestCost = fCost[node];
            }
        }

        return bestNode;
    }
    //지나온 경로를 다시 돌아가서 경로 보여주기
    void BuildPath(Dictionary<RoomNode, RoomNode> cameFrom,RoomNode current)
    {
        // 그전의 경로 제거
        path.Clear();
        //경로 저장에 방 넣기
        path.Add(current);

        //지나온 경로에 그 방이 있으면 대입하여 넣기 
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        //묙표점 >> 시작점 순서였으니 방향을 돌려서 시작점 >> 목표점으로 변환
        path.Reverse();
    }
    private void OnDrawGizmos()
    {
        //경로가 없거나 크기가 안되면 리턴
        if (path == null || path.Count <= 1)
            return;

        Gizmos.color = stoppedByLimit ? Color.yellow : Color.red;

        for (int i = 0; i < path.Count - 1; i++)
        {
            Gizmos.DrawLine(path[i].transform.position + Vector3.up * 2f,path[i + 1].transform.position + Vector3.up * 2f);
        }
    }
}