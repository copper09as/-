using UnityEngine;
using UnityEngine.Events;

public class MapLocation : MonoBehaviour
{
    public MapLocation[] connectedLocations; // ��õص�ֱ�������ĵص�
    public UnityEvent onArrivedEvent;        // ����õص�ʱ�������¼�
    public Vector2 playerIconPosition;       // ���ͼ���ڴ˵ص����ʾλ��

    private void OnMouseDown()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TryMoveToLocation(this);
        }
    }

    private void OnDrawGizmos()
    {
        // �� Scene ��ͼ�л���������
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