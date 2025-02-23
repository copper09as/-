using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingBuilder<T> : Builder<T> where T :Building
{
    GameObject gridBuild = new GameObject();
    BuildingID buildingID = new BuildingID();

    public override void AddSprite(Sprite sprite)
    {
        gridBuild.AddComponent<SpriteRenderer>();
        gridBuild.GetComponent<SpriteRenderer>().sortingLayerName = "Building";
        gridBuild.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    public override void SetDetails(BuildingDetails buildingDetails)
    {
        gridBuild.AddComponent<T>();
        gridBuild.GetComponent<T>().buildingDetails = buildingDetails;
        buildingID.buildingID = buildingDetails.buildingID;
    }
    public override void SetTrans(Vector2 position, Vector2 scale,LayerMask layer)
    {
        gridBuild.transform.position = position;
        gridBuild.transform.localScale = scale;
        gridBuild.layer = layer;
    }
    public override void Create(Transform transform,Vector2 centerPosition,bool isLoad)
    {
        if(!isLoad)
        {
           
            buildingID.isFinish = 0;
            buildingID.pos = centerPosition;
            SaveManager.Instance.buildingSave.buildingIDs.Add(buildingID);
        }
       
        gridBuild.transform.parent = transform;
        gridBuild.GetComponent<Building>().centerGrid = centerPosition;
        gridBuild.GetComponent<Building>().buildingID.pos = centerPosition;
        gridBuild.AddComponent<PolygonCollider2D>();
        gridBuild.GetComponent<T>().isPlace = true;
        BuildingManager.Instance.buildings.Add(gridBuild.GetComponent<Building>());
    }

    public override void Create()
    {
        gridBuild.AddComponent<PolygonCollider2D>();
        gridBuild.GetComponent<T>().isPlace = true;
        BuildingManager.Instance.buildings.Add(gridBuild.GetComponent<Building>());
    }
    public override void Create(T build)
    {
        throw new System.NotImplementedException();
    }

    public override void Create(Transform transform)
    {
        throw new System.NotImplementedException();
    }
    /*public void Create(Image image,LayerMask layer, BuildingDetails buildingDetails,Vector2 position,Vector2 scale,Transform transform)
{
GameObject gridBuild = new GameObject();
gridBuild.AddComponent<SpriteRenderer>();
gridBuild.GetComponent<SpriteRenderer>().sprite = image.sprite;
gridBuild.layer = 6;
gridBuild.AddComponent<T>();
gridBuild.GetComponent<T>().buildingDetails = buildingDetails;
gridBuild.transform.position = position;
gridBuild.transform.parent = transform;
gridBuild.AddComponent<PolygonCollider2D>();
gridBuild.transform.localScale = scale;
gridBuild.GetComponent<T>().isPlace = true;
BuildingManager.Instance.buildings.Add(gridBuild.GetComponent<Building>());
}*/
}
