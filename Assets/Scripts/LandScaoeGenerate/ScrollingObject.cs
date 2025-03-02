// ScrollingObject.cs
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [Header("�ƶ�����")]
    public float scrollSpeed = 5f; // �ƶ��ٶȣ���������
    public bool isParallax = false; // �Ƿ������Ӳ�Ч��
    public Transform parallaxReference; // �Ӳ�ο�����ͨ���������

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
            // �Ӳ�����߼������ڲο�����λ�ƣ�
            float parallax = (parallaxReference.position.x - startPosition.x) * scrollSpeed;
            transform.position = new Vector3(startPosition.x + parallax, transform.position.y, transform.position.z);
        }
        else
        {
            // �㶨�ٶ��ƶ�
            transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);
        }
    }
}