using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Building_SO", menuName = "Building/BuildingList")]
public class BuildingSo : ScriptableObject
{
    public List<BuildingDetails> BuildingList;
}
