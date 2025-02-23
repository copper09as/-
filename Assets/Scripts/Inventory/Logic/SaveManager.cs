using MFarm.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : Singleton<SaveManager>
{
    public BuildingSave buildingSave;
    public Button saveButton;

    void Start()
    {
        saveButton.onClick.AddListener(OnSave);
        Load(); // �ȳ��Լ��ش浵
    }

    private void Load()
    {
        var tempSave = GameSave.LoadByJson<BuildingSave>("Load1.json");
        if (tempSave == null)
        {
            Debug.Log("No save found, creating new game");
            InitNewGame();
            return;
        }

        buildingSave = tempSave;

        // ȷ���б��ʼ��
        if (buildingSave.buildingSlots == null)
            buildingSave.buildingSlots = new List<bool>();


        // �ؽ�����

            foreach (var loadBuilding in buildingSave.buildingIDs)
            {
                var buildingData = BuildingManager.Instance.GetId(loadBuilding.buildingID);
                if (buildingData == null) continue;

                BuildingBuilder<Building> builder = new BuildingBuilder<Building>();
                builder.AddSprite(buildingData.sprite);
                builder.SetDetails(buildingData);

                var targetGrid = BuildingManager.Instance.gridSlot?.grids?.Find(i => i.transPosition == loadBuilding.pos);
                if (targetGrid != null)
                {
                    builder.SetTrans(targetGrid.transform.position, new Vector2(0.1f, 0.1f), 6);
                    builder.Create(BuildingManager.Instance.GridBuilds, loadBuilding.pos, true);
                    SaveManager.Instance.buildingSave.buildingIDs.Add(builder.buildingID);
            }

                // ������������״̬
                if (buildingData.areaGrid != null)
                {
                    foreach (var area in buildingData.areaGrid)
                    {
                        var addPosition = area + loadBuilding.pos;
                        var grid = BuildingManager.Instance.gridSlot?.grids?.Find(i => i.transPosition == addPosition);
                        if (grid != null) grid.canPlace = false;
                    }
                }
            }
        

        // ���²�λ״̬
        if (InventoryManager.Instance?.buildingUi?.slots != null)
        {
            for (int i = 0; i < InventoryManager.Instance.buildingUi.slots.Count; i++)
            {
                if (i < buildingSave.buildingSlots.Count)
                {
                    InventoryManager.Instance.buildingUi.slots[i].isFinish = buildingSave.buildingSlots[i];
                    InventoryManager.Instance.buildingUi.slots[i].UpdateSlot();
                }
            }
        }

        // ���½�Ǯ
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.playerMoney = buildingSave.playerMoney;
        }
    }

    private void InitNewGame()
    {
        buildingSave = new BuildingSave()
        {
            buildingIDs = new List<BuildingID>(),
            buildingSlots = new List<bool>(),
            playerMoney = 0
        };

        // ��ʼ����λ
        if (InventoryManager.Instance?.buildingUi?.slots != null)
        {
            foreach (var slot in InventoryManager.Instance.buildingUi.slots)
            {
                slot.isFinish = false;
                slot.UpdateSlot();
                buildingSave.buildingSlots.Add(false); // ͬ�����浵
            }
        }
    }

    private void OnSave()
    {
        if (buildingSave == null) return;

        // ���½�������
       

        // ���½�Ǯ
        if (InventoryManager.Instance != null)
        {
            buildingSave.playerMoney = InventoryManager.Instance.playerMoney;
        }

        // ���²�λ״̬
        if (InventoryManager.Instance?.buildingUi?.slots != null)
        {
            buildingSave.buildingSlots.Clear();
            foreach (var slot in InventoryManager.Instance.buildingUi.slots)
            {
                buildingSave.buildingSlots.Add(slot.isFinish);
            }
        }

        GameSave.SaveByJson("Load1.json", buildingSave);
    }
}