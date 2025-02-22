using MFarm.Inventory;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveManager : Singleton<SaveManager>
{
    public BuildingSave buildingSave;
    public Button saveButton;

    void Start()
    {
        // ȷ��buildingSave��ʼ��
        buildingSave = new BuildingSave()
        {
            buildingIDs = new List<BuildingID>(),
            buildingSlots = new List<bool>(),
            playerMoney = 0
        };

        saveButton.onClick.AddListener(OnSave);
        Load();
    }

    private void Load()
    {
        // ���Լ��ش浵
        var tempSave = GameSave.LoadByJson<BuildingSave>("Load1.json");

        // �����״μ������
        if (tempSave == null)
        {
            Debug.Log("No save found, creating new game");
            InitNewGame();
            return;
        }

        buildingSave = tempSave;

        // ȷ��buildingSlots��ʼ��
        if (buildingSave.buildingSlots == null)
            buildingSave.buildingSlots = new List<bool>();

        // ��ʼ������
        if (buildingSave.buildingIDs != null)
        {
            foreach (var loadBuilding in buildingSave.buildingIDs)
            {
                if (BuildingManager.Instance == null) continue;

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
                }

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
        }

        // ����buildingSlots
        if (InventoryManager.Instance?.buildingUi?.slots != null)
        {
            // �޸�ѭ���ṹ
            while (buildingSave.buildingSlots.Count < InventoryManager.Instance.buildingUi.slots.Count)
            {
                buildingSave.buildingSlots.Add(false);
            }

            for (int i = 0; i < InventoryManager.Instance.buildingUi.slots.Count; i++)
            {
                if (i < buildingSave.buildingSlots.Count)
                {
                    InventoryManager.Instance.buildingUi.slots[i].isFinish = buildingSave.buildingSlots[i];
                    InventoryManager.Instance.buildingUi.slots[i].UpdateSlot();
                }
            }
        }

        // ��ʼ����Ǯ
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.playerMoney = buildingSave.playerMoney;
        }
    }

    private void InitNewGame()
    {
        // ��ʼ��ȫ����Ϸ����
        buildingSave = new BuildingSave()
        {
            buildingIDs = new List<BuildingID>(),
            buildingSlots = new List<bool>(),
            playerMoney = 0
        };

        // ��ʼ��������λ
        if (InventoryManager.Instance?.buildingUi?.slots != null)
        {
            foreach (var slot in InventoryManager.Instance.buildingUi.slots)
            {
                slot.isFinish = false;
                slot.UpdateSlot();
            }
        }
    }

    private void OnSave()
    {
        if (buildingSave == null) return;

        // �����Ǯ
        if (InventoryManager.Instance != null)
        {
            buildingSave.playerMoney = InventoryManager.Instance.playerMoney;
        }

        // ���潨��״̬
        if (InventoryManager.Instance?.buildingUi?.slots != null)
        {
            buildingSave.buildingSlots = new List<bool>();
            foreach (var slot in InventoryManager.Instance.buildingUi.slots)
            {
                buildingSave.buildingSlots.Add(slot.isFinish);
            }
        }

        GameSave.SaveByJson("Load1.json", buildingSave);
    }
}