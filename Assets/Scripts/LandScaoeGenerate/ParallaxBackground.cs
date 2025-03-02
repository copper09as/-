using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("配置")]
    public Transform cameraTransform; // 相机
    public float parallaxFactor = 0.5f; // 视差系数（0-1）
    public bool horizontalScroll = true; // 是否水平滚动
    public bool verticalScroll = false; // 是否垂直滚动

    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        // 计算相机移动的差值
        Vector3 delta = cameraTransform.position - lastCameraPosition;

        // 根据视差系数调整背景移动
        Vector3 movement = new Vector3(
            horizontalScroll ? delta.x * parallaxFactor : 0,
            verticalScroll ? delta.y * parallaxFactor : 0,
            0
        );

        // 移动背景
        transform.position += movement;

        // 更新相机位置
        lastCameraPosition = cameraTransform.position;
    }
}