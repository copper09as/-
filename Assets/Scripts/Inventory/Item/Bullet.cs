using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed; // �ӵ��ٶ�
    private Rigidbody2D rb2d; // �ӵ��� Rigidbody2D ���
    public float lifeTime = 1f; // �ӵ��Ĵ��ʱ�䣨1�룩

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        // �ӵ�����ʱ������һ����ʱ����1�������
        Invoke("HideBullet", lifeTime);
    }

    void OnDisable()
    {
        // �ӵ�������ʱ��ȡ��֮ǰ�Ķ�ʱ��
        CancelInvoke("HideBullet");
    }

    // �����ӵ����ٶ�
    public void SetSpeed(Vector2 direction)
    {
        rb2d.velocity = direction.normalized * speed;
    }

    // �����ӵ������ջ����٣�
    private void HideBullet()
    {
        // ʹ�ö���ػ����ӵ�
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
      
    }


}