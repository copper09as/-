using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace MFarm.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] public Image slotHighlight;
        [SerializeField] private Button button;

        [Header("格子类型")]
        public SlotType slotType;

        public bool isSelected;
        public int slotIndex;

        // 物品信息
        public ItemDetails itemDetails;
        public int itemAmount;

        public InventoryLocation Location
        {
            get
            {
                return slotType switch
                {
                    SlotType.Bag => InventoryLocation.Player,
                    SlotType.Box => InventoryLocation.Box,
                    _ => InventoryLocation.Player,
                };
            }
        }

        public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Start()
        {
            isSelected = false;

            if (itemDetails == null)
            {
                UpdateEmptySlot();
            }
        }

        /// <summary>
        /// 更新格子UI和信息
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">持有数量</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;

            slotImage.enabled = true;
            button.interactable = true;
        }

        /// <summary>
        /// 将Slot更新为空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
                inventoryUI.UpdateSlotHighlight(-1);
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
            itemDetails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        // 点击触发交易或选择
        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails == null) return;

            // 点击商店物品触发购买
            if (slotType == SlotType.Shop)
            {
                EventHandler.CallShowTradeUI(itemDetails, false);
            }
            // 点击背包物品触发卖出
            else if (slotType == SlotType.Bag)
            {
                EventHandler.CallShowTradeUI(itemDetails, true);
            }
            // 其他情况（如箱子）保持原有选择逻辑
            else
            {
                isSelected = !isSelected;
                inventoryUI.UpdateSlotHighlight(slotIndex);
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }


        

        // 鼠标进入物品时显示 Tooltip
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (itemDetails != null)
            {
                inventoryUI.itemTooltip.gameObject.SetActive(true);
                inventoryUI.itemTooltip.SetupTooltip(itemDetails, slotType);
            }
        }

        // 鼠标离开物品时隐藏 Tooltip
        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemTooltip.gameObject.SetActive(false);
        }
    }
}