using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool canPlace;
    public void OnPointerEnter(PointerEventData eventData)
    {
        var x = eventData.pointerCurrentRaycast.gameObject;
        BuildingManager.Instance.grid = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BuildingManager.Instance.grid = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void UpdateSlot()
    {
        
    }
}
