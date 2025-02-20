using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed; // 子弹速度
    private Rigidbody2D rb2d; // 子弹的 Rigidbody2D 组件
    public float lifeTime = 1f; // 子弹的存活时间（1秒）

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        // 子弹激活时，设置一个定时器，1秒后隐藏
        Invoke("HideBullet", lifeTime);
    }

    void OnDisable()
    {
        // 子弹被隐藏时，取消之前的定时器
        CancelInvoke("HideBullet");
    }

    // 设置子弹的速度
    public void SetSpeed(Vector2 direction)
    {
        rb2d.velocity = direction.normalized * speed;
    }

    // 隐藏子弹（回收或销毁）
    private void HideBullet()
    {
        // 使用对象池回收子弹
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.PushObject(gameObject);
        }
      
    }


}