using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class BuildingManager : Singleton<BuildingManager>
{
    public BuildingBagSo buildingBagSo;
    public BuildingSo buildingSO;
    public List<Building> buildings;
    public TempBuild tempBuild;
    public Transform GridBuilds;
    public GridSlot gridSlot;
    public Grid grid;
    public bool isDrag;
    public BuildingDetails GetId(int id)
    {
        return buildingSO.BuildingList.Find(i => i.buildingID == id);
    }
    public bool CanPlaceBuilding()
    {
        if (grid == null) return false;
        return grid.canPlace;

    }

}
