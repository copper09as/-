using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace MFarm.Inventory
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("�����ȡ")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] public Image slotHighlight;
        [SerializeField] private Button button;

        [Header("��������")]
        public SlotType slotType;

        public bool isSelected;
        public int slotIndex;

        // ��Ʒ��Ϣ
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
        /// ���¸���UI����Ϣ
        /// </summary>
        /// <param name="item">ItemDetails</param>
        /// <param name="amount">��������</param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;

            slotImage.enabled = true;
            button.interactable = true;
        }

        /// <summary>
        /// ��Slot����Ϊ��
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

        // ����������׻�ѡ��
        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails == null) return;

            // ����̵���Ʒ��������
            if (slotType == SlotType.Shop)
            {
                EventHandler.CallShowTradeUI(itemDetails, false);
            }
            // ���������Ʒ��������
            else if (slotType == SlotType.Bag)
            {
                EventHandler.CallShowTradeUI(itemDetails, true);
            }
            // ��������������ӣ�����ԭ��ѡ���߼�
            else
            {
                isSelected = !isSelected;
                inventoryUI.UpdateSlotHighlight(slotIndex);
                EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
        }


        

        // ��������Ʒʱ��ʾ Tooltip
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (itemDetails != null)
            {
                inventoryUI.itemTooltip.gameObject.SetActive(true);
                inventoryUI.itemTooltip.SetupTooltip(itemDetails, slotType);
            }
        }

        // ����뿪��Ʒʱ���� Tooltip
        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemTooltip.gameObject.SetActive(false);
        }
    }
}