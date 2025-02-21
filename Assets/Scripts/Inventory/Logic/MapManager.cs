using UnityEngine;
using UnityEngine.Events;

public class MapLocation : MonoBehaviour
{
    public MapLocation[] connectedLocations; // 与该地点直接相连的地点
    public UnityEvent onArrivedEvent;        // 到达该地点时触发的事件
    public Vector2 playerIconPosition;       // 玩家图标在此地点的显示位置

    private void OnMouseDown()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TryMoveToLocation(this);
        }
    }

    private void OnDrawGizmos()
    {
        // 在 Scene 视图中绘制连接线
        Gizmos.color = Color.blue;
        foreach (var location in connectedLocations)
        {
            if (location != null)
            {
                Gizmos.DrawLine(transform.position, location.transform.position);
            }
        }
    }
}