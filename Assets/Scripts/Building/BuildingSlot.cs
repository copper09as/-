using System.Collections;
using System.Collections.Generic;
using MFarm.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BuildingSlot : MonoBehaviour, IPointerDownHandler
{
    public BuildingDetails buildingDetails;
    public bool isFinish = false;
    private Button buildButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isFinish) return;

        TempBuilder<TempBuild> tempBuilder = new TempBuilder<TempBuild>();
        tempBuilder.Create(BuildingManager.Instance.tempBuild);
        tempBuilder.AddSprite(buildingDetails.sprite);
        tempBuilder.SetDetails(buildingDetails);

    }
    private void Start()
    {
        this.buildButton = this.transform.GetChild(0).gameObject.GetComponent<Button>();
        buildButton.onClick.AddListener(StartBuild);

    }
    public void UpdateSlot()
    {
        this.gameObject.GetComponent<Image>().sprite = buildingDetails.sprite;
        this.gameObject.name = buildingDetails.name;
        if (!this.isFinish) this.gameObject.GetComponent<Image>().sprite = null;
    }
    public void StartBuild()
    {
        if (isFinish) return;
        if (InventoryManager.Instance.playerMoney < this.buildingDetails.price) return;
        InventoryManager.Instance.playerMoney -= this.buildingDetails.price;
        this.gameObject.GetComponent<Image>().sprite = buildingDetails.sprite;
        Debug.Log("Success");
        isFinish = true;
    }
    
}
