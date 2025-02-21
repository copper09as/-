using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Building : MonoBehaviour
{
    public BuildingDetails buildingDetails;
    public bool isPlace;
    private void OnMouseDown()
    {
        if (!isPlace) return;
        TempBuild tempBuild;
        tempBuild = BuildingManager.Instance.tempBuild;
        tempBuild.GetComponent<Image>().sprite = buildingDetails.sprite;
        tempBuild.buildingDetails = buildingDetails;
        tempBuild.isDrag = true;
        Destroy(gameObject);
    }
}
