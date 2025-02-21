using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveDuration = 1f; // 移动动画时长

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
            Debug.Log("今天已经移动过了，明天再来吧！");
            return;
        }

        // 首次移动
        if (_currentLocation == null)
        {
            MoveToLocation(targetLocation);
            return;
        }

        // 检查是否是相邻地点
        if (IsLocationConnected(targetLocation))
        {
            MoveToLocation(targetLocation);
        }
        else
        {
            Debug.Log("无法移动到非相邻地点！");
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