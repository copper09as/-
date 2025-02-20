using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingManager : Singleton<BuildingManager>
{
    public BuildingBagSo buildingBagSo;
    public BuildingSo buildingSO;
    
    public BuildingDetails GetId(int id)
    {
        return buildingSO.BuildingList.Find(i => i.buildingID == id);
    }

}
