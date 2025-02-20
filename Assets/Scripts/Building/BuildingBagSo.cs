using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Building_SO", menuName = "Building/BuildingBagSo")]
public class BuildingBagSo : ScriptableObject
{
    public List<BuildingID> itemList;
}
