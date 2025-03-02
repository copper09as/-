// LayerConfig.cs
using UnityEngine;

[CreateAssetMenu(menuName = "Landscape/Layer Config")]
public class LayerConfig : ScriptableObject
{
    [Header("基础设置")]
    public string layerName;
    public Sprite[] sprites; // 2D精灵数组
    public float minDistance = 5f;  // 生成最小距离
    public float maxDistance = 20f; // 生成最大距离
    public float density = 0.3f;    // 生成密度
    public float yPosition; // 每层的 Y 轴位置

    [Header("高级设置")]
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    public float scrollSpeed = -5f;
    public float speedOffset; // 该层级相对于全局速度的偏移量
    public bool useParallax = false; // 是否启用视差滚动
    public float parallaxFactor = 0.5f; // 视差系数
    public int sortingOrder = 0; // 渲染排序层级
    public int maxObjectsInView = 6; // 屏幕内最大生成数
    public float minSpacing = 10f;
}