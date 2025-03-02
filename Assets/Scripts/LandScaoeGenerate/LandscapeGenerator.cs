using System.Collections.Generic;
using UnityEngine;

public class LandscapeGenerator : MonoBehaviour
{
    [Header("基础设置")]
    public Transform cameraTransform;
    public float globalScrollSpeed = 2f; // 全局滚动速度
    public LayerConfig[] layers;
    public float pregenerateMultiplier = 1.5f;

    [Header("调试")]
    public bool showSpawnBounds = true;

    private const float chunkSize = 5f;
    private Dictionary<Vector2, GameObject> activeChunks = new Dictionary<Vector2, GameObject>();
    private float totalScrolledDistance; // 累计滚动距离

    void Start()
    {
        GenerateInitialChunks();
    }

    void Update()
    {
        // 持续更新滚动距离
        totalScrolledDistance += globalScrollSpeed * Time.deltaTime;

        // 根据滚动距离计算当前虚拟位置
        float virtualX = totalScrolledDistance;
        UpdateChunks(virtualX);
    }

    void GenerateInitialChunks()
    {
        float maxDistance = GetMaxLayerDistance();
        int chunksToGenerate = Mathf.CeilToInt(maxDistance * pregenerateMultiplier / chunkSize);

        for (int x = -chunksToGenerate; x <= chunksToGenerate; x++)
        {
            Vector2 chunkPos = new Vector2(x * chunkSize, 0);
            GenerateChunk(chunkPos);
        }
    }

    void UpdateChunks(float virtualX)
    {
        float maxDistance = GetMaxLayerDistance();
        int chunksVisible = Mathf.CeilToInt(maxDistance / chunkSize);

        // 计算当前应该存在的区块范围
        float currentChunkX = Mathf.Floor(virtualX / chunkSize) * chunkSize;

        // 生成前方区块
        for (float x = currentChunkX - chunksVisible * chunkSize;
             x <= currentChunkX + chunksVisible * chunkSize;
             x += chunkSize)
        {
            Vector2 chunkPos = new Vector2(x, 0);
            if (!activeChunks.ContainsKey(chunkPos))
            {
                GenerateChunk(chunkPos);
            }
        }

        // 卸载后方区块
        List<Vector2> toRemove = new List<Vector2>();
        foreach (var chunk in activeChunks)
        {
            if (chunk.Key.x < virtualX - maxDistance * pregenerateMultiplier)
            {
                Destroy(chunk.Value);
                toRemove.Add(chunk.Key);
            }
        }

        foreach (var pos in toRemove)
            activeChunks.Remove(pos);
    }

    float GetMaxLayerDistance()
    {
        float max = 0f;
        foreach (LayerConfig layer in layers)
            if (layer.maxDistance > max) max = layer.maxDistance;
        return max;
    }

    // 修改后的区块生成方法
    void GenerateChunk(Vector2 pos)
    {
        GameObject chunk = new GameObject($"Chunk_{pos}");
        chunk.transform.position = pos;

        foreach (LayerConfig layer in layers)
        {
            if (ShouldGenerateInChunk(pos, layer))
                GenerateLayerObjects(chunk.transform, pos, layer);
        }

        activeChunks.Add(pos, chunk);
    }

    // 修改后的生成条件判断
    bool ShouldGenerateInChunk(Vector2 pos, LayerConfig layer)
    {
        float distance = Mathf.Abs(pos.x - totalScrolledDistance);
        return distance >= layer.minDistance && distance <= layer.maxDistance;
    }


    void GenerateLayerObjects(Transform parent, Vector2 pos, LayerConfig layer)
    {
        float minSpacing = layer.minSpacing;
        int maxAttemptsPerPoint = 10;
        float gridStep = minSpacing * 1.5f;

        for (float x = -chunkSize / 2; x < chunkSize / 2; x += gridStep)
        {
            for (int attempt = 0; attempt < maxAttemptsPerPoint; attempt++)
            {
                float randomOffsetX = Random.Range(-gridStep / 2, gridStep / 2);
                Vector2 objPos = new Vector2(
                    pos.x + x + randomOffsetX,
                    layer.yPosition + Random.Range(-0.5f, 0.5f)
                );

                if (!IsTooClose(objPos, parent, minSpacing))
                {
                    GameObject obj = new GameObject("Element");
                    SpriteRenderer renderer = obj.AddComponent<SpriteRenderer>();
                    renderer.sprite = layer.sprites[Random.Range(0, layer.sprites.Length)];
                    renderer.sortingOrder = layer.sortingOrder;
                    obj.transform.position = objPos;
                    obj.transform.SetParent(parent);

                    // 修改滚动脚本参数
                    ScrollingObject scroll = obj.AddComponent<ScrollingObject>();
                    scroll.scrollSpeed = globalScrollSpeed + layer.speedOffset; // 全局速度 + 层级偏移
                    scroll.isParallax = false; // 禁用视差效果

                    float scaleFactor = Random.Range(layer.scaleRange.x, layer.scaleRange.y);
                    obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
                    break;
                }
            }
        }
    }

    bool IsTooClose(Vector2 pos, Transform parent, float minDistance)
    {
        foreach (Transform child in parent)
        {
            if (Vector2.Distance(pos, child.position) < minDistance)
                return true;
        }
        return false;
    }

    void OnDrawGizmos()
    {
        if (!showSpawnBounds || cameraTransform == null) return;

        Gizmos.color = Color.green;
        float maxDistance = GetMaxLayerDistance();
        Gizmos.DrawWireSphere(cameraTransform.position, maxDistance);

        // 绘制间距范围
        foreach (var chunk in activeChunks)
        {
            foreach (Transform child in chunk.Value.transform)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(child.position, layers[1].minSpacing); // 以树木层为例
            }
        }
    }
}