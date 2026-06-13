using System.Collections.Generic;
using UnityEngine;

public class RoomNode : MonoBehaviour
{
    [Header("연결된 방")]
    public RoomNode north;
    public RoomNode south;
    public RoomNode west;
    public RoomNode east;

    // 컴포넌트에 넣어진 것들, 연결된 방만 neighbors에 추가한다
    public List<RoomNode> GetNeighbors()
    {
        List<RoomNode> neighbors = new List<RoomNode>();

        if (north != null) neighbors.Add(north);
        if (south != null) neighbors.Add(south);
        if (west != null) neighbors.Add(west);
        if (east != null) neighbors.Add(east);

        return neighbors;
    }

}