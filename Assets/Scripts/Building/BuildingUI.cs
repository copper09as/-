using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    public List<BuildingSlot> slots;
    public List<BuildingDetails> details;
    private void OnEnable()
    {
        UpdateBuildingSlot();
    }
    private void UpdateBuildingSlot()
    {
        details.Clear();
        try
        {
            foreach (var bd in BuildingManager.Instance.buildingBagSo.itemList)
            {
                details.Add(BuildingManager.Instance.GetId(bd.buildingID));
            }
        }
        catch
        {
            Debug.Log("建筑管理器单例未获取");
            return;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            slot.buildingDetails = details[i];
            slot.UpdateSlot();
        }
    }
}
