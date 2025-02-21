using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Build;
using UnityEngine;
[System.Serializable]
public class BuildingDetails
{
    public int buildingID;
    public List<Vector2> areaGrid;
    public Vector2 centerPosition;
    public int price;
    public int gain;
    public Sprite sprite;
    public string name;

}
[System.Serializable]
public struct BuildingID
{
    public int buildingID; // д╛хо0
    public int isFinish;


}

