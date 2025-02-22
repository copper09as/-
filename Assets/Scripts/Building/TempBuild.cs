using UnityEngine;
public class TempBuild : MonoBehaviour
{
    private bool _isDrag;
    public BuildingDetails buildingDetails;

    public bool isDrag
    {
        get
        {
            return _isDrag;
        }
        set
        {
            _isDrag = value;
            this.transform.position = Input.mousePosition;
            gameObject.SetActive(value);
            BuildingManager.Instance.isDrag = value;

        }
    }

    void Update()
    {
        if (isDrag)
            this.transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(1)) isDrag = false;
        if (Input.GetMouseButtonDown(0) && BuildingManager.Instance.CanPlaceBuilding(buildingDetails.areaGrid))
        {
            isDrag = false;
            BuildingBuilder<Building> builder = new BuildingBuilder<Building>();
            builder.AddSprite(GetComponent<UnityEngine.UI.Image>().sprite);
            builder.SetDetails(buildingDetails);
            builder.SetTrans(BuildingManager.Instance.grid.transform.position, new Vector2(0.1f, 0.1f), 6);
            builder.Create(BuildingManager.Instance.GridBuilds,BuildingManager.Instance.grid.transPosition,false);
            /*GameObject gridBuild = new GameObject();
            gridBuild.AddComponent<SpriteRenderer>();
            gridBuild.GetComponent<SpriteRenderer>().sprite = GetComponent<UnityEngine.UI.Image>().sprite;
            gridBuild.layer = 6;
            gridBuild.AddComponent<Building>();
            gridBuild.GetComponent<Building>().buildingDetails = this.buildingDetails;
            gridBuild.transform.position = BuildingManager.Instance.grid.transform.position;
            gridBuild.transform.parent = BuildingManager.Instance.GridBuilds;
            gridBuild.AddComponent<PolygonCollider2D>();
            gridBuild.transform.localScale = new Vector2(0.1f, 0.1f);
            gridBuild.GetComponent<Building>().isPlace = true;
            BuildingManager.Instance.buildings.Add(gridBuild.GetComponent<Building>());*/
        }
    }
}
