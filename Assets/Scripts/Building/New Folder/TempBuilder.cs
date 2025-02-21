using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempBuilder<T> : Builder<T> where T : TempBuild
{
    T tempBuild;

    public override void AddSprite(Sprite sprite)
    {
        tempBuild.GetComponent<Image>().sprite = sprite;
    }

    public override void Create(Transform transform)
    {
        throw new System.NotImplementedException();
    }

    public override void Create()
    {
        throw new System.NotImplementedException();
    }

    public override void Create(T build)
    {
        tempBuild = build;
        tempBuild.isDrag = true;
        
    }

    public override void SetDetails(BuildingDetails buildingDetails)
    {
        tempBuild.GetComponent<T>().buildingDetails = buildingDetails;
    }
}
