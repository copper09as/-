using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get => instance;
    }

    protected virtual void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }else
        {
            instance = (T)this;
            Debug.Log(this.name+"已完成单例化");
        }
    }

    protected virtual void OnDestory()
    {
        if(instance == this)
        {
            instance = null;
        }
    }
}
