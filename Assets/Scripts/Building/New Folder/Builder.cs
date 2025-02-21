using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class Builder<T> : MonoBehaviour
{
    public abstract void Create(Transform transform);
    public abstract void Create(Transform transform, Vector2 centerPosition);
    public abstract void Create();
    public abstract void Create(T build);
    public abstract void AddSprite(Sprite sprite);
    public abstract void SetDetails(BuildingDetails buildingDetails);
    public virtual void SetTrans(Vector2 position, Vector2 scale, LayerMask layer)
    {
        return;
    }
}
