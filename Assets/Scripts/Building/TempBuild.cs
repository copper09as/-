using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
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
            gameObject.SetActive(value);
            BuildingManager.Instance.isDrag = value;

        }
    }

    void Update()
    {
        if(isDrag)
            this.transform.position = Input.mousePosition;
        if (Input.GetMouseButtonDown(1)) isDrag = false;
        if (Input.GetMouseButtonDown(0) && BuildingManager.Instance.grid != null)
        {
            isDrag = false;
            GameObject gridBuild = new GameObject();
            gridBuild.AddComponent<UnityEngine.UI.Image>();
            gridBuild.GetComponent<UnityEngine.UI.Image>().sprite = GetComponent<UnityEngine.UI.Image>().sprite;
            gridBuild.AddComponent<Building>();
            gridBuild.GetComponent<Building>().buildingDetails = this.buildingDetails;
            gridBuild.transform.position = BuildingManager.Instance.grid.transform.position;
            gridBuild.transform.parent = BuildingManager.Instance.GridBuilds;
            BuildingManager.Instance.buildings.Add(gridBuild.GetComponent<Building>());
        }
    }
}
