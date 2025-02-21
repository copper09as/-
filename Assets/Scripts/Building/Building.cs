using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Building : MonoBehaviour
{
    public BuildingDetails buildingDetails;
    public bool isPlace;
    public Vector2 centerGrid;
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
