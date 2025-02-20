using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveDuration = 1f; // �ƶ�����ʱ��

    private MapLocation _currentLocation;
    private bool _canMove = true;

    private void Start()
    {
        GameManager.Instance.onDayUpdated.AddListener(ResetMovement);
    }

    public void TryMoveToLocation(MapLocation targetLocation)
    {
        if (!_canMove)
        {
            Debug.Log("�����Ѿ��ƶ����ˣ����������ɣ�");
            return;
        }

        // �״��ƶ�
        if (_currentLocation == null)
        {
            MoveToLocation(targetLocation);
            return;
        }

        // ����Ƿ������ڵص�
        if (IsLocationConnected(targetLocation))
        {
            MoveToLocation(targetLocation);
        }
        else
        {
            Debug.Log("�޷��ƶ��������ڵص㣡");
        }
    }

    private bool IsLocationConnected(MapLocation target)
    {
        foreach (var location in _currentLocation.connectedLocations)
        {
            if (location == target) return true;
        }
        return false;
    }

    private IEnumerator MoveToLocationCoroutine(MapLocation target)
    {
        _canMove = false;
        _currentLocation = target;

        float elapsedTime = 0;
        Vector2 startPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector2.Lerp(startPosition, target.playerIconPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target.playerIconPosition;
        target.onArrivedEvent.Invoke();
    }

    private void MoveToLocation(MapLocation target)
    {
        StartCoroutine(MoveToLocationCoroutine(target));
    }

    private void ResetMovement()
    {
        _canMove = true;
    }
}