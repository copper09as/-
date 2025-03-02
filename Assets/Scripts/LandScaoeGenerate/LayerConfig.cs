// LayerConfig.cs
using UnityEngine;

[CreateAssetMenu(menuName = "Landscape/Layer Config")]
public class LayerConfig : ScriptableObject
{
    [Header("��������")]
    public string layerName;
    public Sprite[] sprites; // 2D��������
    public float minDistance = 5f;  // ������С����
    public float maxDistance = 20f; // ����������
    public float density = 0.3f;    // �����ܶ�
    public float yPosition; // ÿ��� Y ��λ��

    [Header("�߼�����")]
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f);
    public float scrollSpeed = -5f;
    public float speedOffset; // �ò㼶�����ȫ���ٶȵ�ƫ����
    public bool useParallax = false; // �Ƿ������Ӳ����
    public float parallaxFactor = 0.5f; // �Ӳ�ϵ��
    public int sortingOrder = 0; // ��Ⱦ����㼶
    public int maxObjectsInView = 6; // ��Ļ�����������
    public float minSpacing = 10f;
}