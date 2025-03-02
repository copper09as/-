// ScrollingObject.cs
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [Header("移动设置")]
    public float scrollSpeed = 5f; // 移动速度（左负右正）
    public bool isParallax = false; // 是否启用视差效果
    public Transform parallaxReference; // 视差参考对象（通常是相机）

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        if (parallaxReference == null) parallaxReference = Camera.main.transform;
    }

    void Update()
    {
        if (isParallax)
        {
            // 视差滚动逻辑（基于参考对象位移）
            float parallax = (parallaxReference.position.x - startPosition.x) * scrollSpeed;
            transform.position = new Vector3(startPosition.x + parallax, transform.position.y, transform.position.z);
        }
        else
        {
            // 恒定速度移动
            transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        }
    }
}