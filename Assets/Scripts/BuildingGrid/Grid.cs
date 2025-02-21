using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour
{
    [SerializeField] private bool _canPlace;
    [SerializeField] private Color initColor;
    [SerializeField] private Color transColor;
    public bool canPlace
    { 
        get
        {
            return _canPlace;
        }
        set
        {
            _canPlace = value;
            if(value)
            GetComponent<SpriteRenderer>().color = initColor;
            else
            GetComponent<SpriteRenderer>().color = transColor;
        }
    }
    public LayerMask houseLayerMask;
    public Vector2 transPosition;
    private void OnMouseEnter()
    {
        Debug.Log("ssa");
     BuildingManager.Instance.grid = this;
        
    }
    
    private void OnMouseExit()
    {
        BuildingManager.Instance.grid = null;
    }

    // Start is called before the first frame update
    private void Update()
    {
         //CheckObstacle();
    }
    private void CheckObstacle()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, this.GetComponent<SpriteRenderer>().bounds.extents.x/1.5f, houseLayerMask);
        if(collider != null)
        {
            canPlace = false;
        }
        else
        {
            canPlace = true;
        }
    }
}
