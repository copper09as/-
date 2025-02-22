using MFarm.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : Singleton<SaveManager>
{
    public BuildingSave buildingSave;
    public Button saveButton;
    // Start is called before the first frame update
    void Start()
    {
        saveButton.onClick.AddListener(OnSave);
        Load();


    }
    private void Load()
    {
        buildingSave = GameSave.LoadByJson<BuildingSave>("Load1.json");
        InventoryManager.Instance.playerMoney = buildingSave.playerMoney;
        foreach (var loadBuilding in buildingSave.buildingIDs)
        {
            Debug.Log(buildingSave.buildingIDs.Count);
            BuildingBuilder<Building> builder = new BuildingBuilder<Building>();
            builder.AddSprite(BuildingManager.Instance.GetId(loadBuilding.buildingID).sprite);
            builder.SetDetails(BuildingManager.Instance.GetId(loadBuilding.buildingID));
            builder.SetTrans(BuildingManager.Instance.gridSlot.grids.Find(i => i.transPosition == loadBuilding.pos).transform.position, new Vector2(0.1f, 0.1f), 6);
            builder.Create(BuildingManager.Instance.GridBuilds, loadBuilding.pos, true);
            foreach (var area in BuildingManager.Instance.GetId(loadBuilding.buildingID).areaGrid)
            {
                var addPosition = area + loadBuilding.pos;
                BuildingManager.Instance.gridSlot.grids.Find(i => i.transPosition == addPosition).canPlace = false;
            }
        }
        for (int i = 0; i < InventoryManager.Instance.buildingUi.slots.Count; i++)
        {
            InventoryManager.Instance.buildingUi.slots[i].isFinish = buildingSave.buildingSlots[i];
            Debug.Log(buildingSave.buildingSlots[i]);
            InventoryManager.Instance.buildingUi.slots[i].UpdateSlot();
        }

    }
    private void OnSave()
    {
        buildingSave.playerMoney = InventoryManager.Instance.playerMoney;
        GameSave.SaveByJson("Load1.json", this.buildingSave);
        try
        {
            for (int i = 0; i < InventoryManager.Instance.buildingUi.slots.Count; i++)
            {
                buildingSave.buildingSlots[i] = InventoryManager.Instance.buildingUi.slots[i].isFinish;
            }
        }
        catch
        {
            while(buildingSave.buildingSlots.Count< InventoryManager.Instance.buildingUi.slots.Count)
            {
                buildingSave.buildingSlots.Add(false);
            }
            for (int i = 0; i < InventoryManager.Instance.buildingUi.slots.Count; i++)
            {
                buildingSave.buildingSlots[i] = InventoryManager.Instance.buildingUi.slots[i].isFinish;
            }
        }

        Debug.Log(JsonUtility.ToJson(buildingSave));

    }


    // Update is called once per frame

}
