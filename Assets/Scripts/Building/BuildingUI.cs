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
        foreach (var bd in BuildingManager.Instance.buildingBagSo.itemList)
        {
            details.Add(BuildingManager.Instance.GetId(bd.buildingID));
        }
        while(slots.Count < details.Count)
        {
            var slot = Instantiate(slots[0], this.transform);
            slots.Add(slot);
        }
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            slot.buildingDetails = details[i];
            slot.UpdateSlot();
        }
    }
}
