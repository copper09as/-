using System.Collections.Generic;
using UnityEngine;


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
    public bool CanPlaceBuilding(List<Vector2> areaPosition)
    {
        if (grid == null) return false;
        foreach (var area in areaPosition)
        {
            var addPosition = area + grid.transPosition;
            if (addPosition.x < 0 || addPosition.y < 0)
            {
                Debug.Log("越界");
                return false;
            }
            try
            {
                if (gridSlot.grids.Find(i => i.transPosition == addPosition).canPlace == false)
                {
                    Debug.Log("不可放置");
                    return false;
                }
            }
            catch
            {
                return false;
            }

            }

        /*catch
        {
            Debug.Log("错误");
            return false;
        }*/
        return true;
    }

}
