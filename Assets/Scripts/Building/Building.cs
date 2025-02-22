using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Building : MonoBehaviour
{
    public BuildingDetails buildingDetails;
    public BuildingID buildingID;
    public bool isPlace;
    public Vector2 centerGrid;
    private void OnDisable()
    {
        BuildingManager.Instance.buildingSave.buildingIDs.Remove(buildingID);
    }
    private void Start()
    {
        this.buildingID.buildingID = buildingDetails.buildingID;
        this.buildingID.isFinish = 0;
        this.buildingID.pos = centerGrid;
       
    }

    private void OnMouseDown()
    {
        try
        {
            BuildingManager.Instance.ClearGird(buildingDetails.areaGrid,centerGrid);
        }
        catch
        {
            return;
        }
        if (!isPlace) return;
        TempBuilder<TempBuild> tempBuilder = new TempBuilder<TempBuild>();
        tempBuilder.Create(BuildingManager.Instance.tempBuild);
        tempBuilder.AddSprite(buildingDetails.sprite);
        tempBuilder.SetDetails(buildingDetails);
        /*TempBuild tempBuild;
        tempBuild = BuildingManager.Instance.tempBuild;
        tempBuild.GetComponent<Image>().sprite = buildingDetails.sprite;
        tempBuild.buildingDetails = buildingDetails;
        tempBuild.isDrag = true;*/
        
        Destroy(gameObject);
    }
}
