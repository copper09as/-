using System.Collections.Generic;
using UnityEngine;

public class LandscapeGenerator : MonoBehaviour
{
    [Header("��������")]
    public Transform cameraTransform;
    public float globalScrollSpeed = 2f; // ȫ�ֹ����ٶ�
    public LayerConfig[] layers;
    public float pregenerateMultiplier = 1.5f;

    [Header("����")]
    public bool showSpawnBounds = true;

    private const float chunkSize = 5f;
    private Dictionary<Vector2, GameObject> activeChunks = new Dictionary<Vector2, GameObject>();
    private float totalScrolledDistance; // �ۼƹ�������

    void Start()
    {
        GenerateInitialChunks();
    }

    void Update()
    {
        // �������¹�������
        totalScrolledDistance += globalScrollSpeed * Time.deltaTime;

        // ���ݹ���������㵱ǰ����λ��
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

        // ���㵱ǰӦ�ô��ڵ����鷶Χ
        float currentChunkX = Mathf.Floor(virtualX / chunkSize) * chunkSize;

        // ����ǰ������
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

        // ж�غ�����
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

    // �޸ĺ���������ɷ���
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

    // �޸ĺ�����������ж�
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

                    // �޸Ĺ����ű�����
                    ScrollingObject scroll = obj.AddComponent<ScrollingObject>();
                    scroll.scrollSpeed = globalScrollSpeed + layer.speedOffset; // ȫ���ٶ� + �㼶ƫ��
                    scroll.isParallax = false; // �����Ӳ�Ч��

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

        // ���Ƽ�෶Χ
        foreach (var chunk in activeChunks)
        {
            foreach (Transform child in chunk.Value.transform)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(child.position, layers[1].minSpacing); // ����ľ��Ϊ��
            }
        }
    }
}