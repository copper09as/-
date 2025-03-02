using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("����")]
    public Transform cameraTransform; // ���
    public float parallaxFactor = 0.5f; // �Ӳ�ϵ����0-1��
    public bool horizontalScroll = true; // �Ƿ�ˮƽ����
    public bool verticalScroll = false; // �Ƿ�ֱ����

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
        // ��������ƶ��Ĳ�ֵ
        Vector3 delta = cameraTransform.position - lastCameraPosition;

        // �����Ӳ�ϵ�����������ƶ�
        Vector3 movement = new Vector3(
            horizontalScroll ? delta.x * parallaxFactor : 0,
            verticalScroll ? delta.y * parallaxFactor : 0,
            0
        );

        // �ƶ�����
        transform.position += movement;

        // �������λ��
        lastCameraPosition = cameraTransform.position;
    }
}