using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MFarm.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public ItemToolTip itemTooltip;

        [Header("拖拽图片")]
        public Image dragItem;

        [Header("玩家背包UI")]
        [SerializeField] private GameObject bagUI;

        private bool bagOpened;

        [Header("通用背包")]
        [SerializeField] private GameObject baseBag;


        public GameObject shopSlotPrefab;

        [Header("交易UI")]
        public TradeUI tradeUI;

        public TextMeshProUGUI playerMoneyText;

        [SerializeField] private SlotUI[] playerSlots;

        [SerializeField] private List<SlotUI> baseBagSlots;

        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
            EventHandler.BaseBagOpenEvent += OnBaseBagOpenEvent;
            EventHandler.BaseBagCloseEvent += OnBaseBagCloseEvent;
            EventHandler.ShowTradeUI += OnShowTradeUI;
        }

        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;
            EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
            EventHandler.BaseBagOpenEvent -= OnBaseBagOpenEvent;
            EventHandler.BaseBagCloseEvent -= OnBaseBagCloseEvent;
            EventHandler.ShowTradeUI -= OnShowTradeUI;
        }

        private void Start()
        {
            for (int i = 0; i < playerSlots.Length; i++)
            {
                playerSlots[i].slotIndex = i;
            }

            // 初始化时直接打开背包和商店
            OpenBagUI();  // 背包开启
            OpenShopUI();  // 商店开启

            playerMoneyText.text = InventoryManager.Instance.playerMoney.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenBagUI();
            }
        }

        private void OnShowTradeUI(ItemDetails item, bool isSell)
        {
            tradeUI.gameObject.SetActive(true);
            tradeUI.SetupTradeUI(item, isSell);
        }

        private void OpenShopUI()
        {
            bagUI.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            bagUI.SetActive(true);
            bagOpened = true;
        }

        private void OnBaseBagOpenEvent(SlotType slotType, InventoryBag_SO bagData)
        {
            GameObject prefab = slotType switch
            {
                SlotType.Shop => shopSlotPrefab,
                _ => null,
            };

            // 生成背包UI
            baseBag.SetActive(true);
            baseBagSlots = new List<SlotUI>();

            for (int i = 0; i < bagData.itemList.Count; i++)
            {
                var slot = Instantiate(prefab, baseBag.transform.GetChild(0)).GetComponent<SlotUI>();
                slot.slotIndex = i;
                baseBagSlots.Add(slot);

                // 获取物品信息
                var itemDetails = InventoryManager.Instance.GetItemDetails(bagData.itemList[i].itemID);

                // 更新物品名字和价格
                slot.itemNameText.text = itemDetails.itemName; // 设置物品名字
                slot.itemPriceText.text = itemDetails.itemPrice.ToString(); // 设置物品价格
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(baseBag.GetComponent<RectTransform>());

            if (slotType == SlotType.Shop)
            {
                // 商店UI始终开启
                bagUI.SetActive(true);
                bagOpened = true;
            }

            // 更新UI显示
            OnUpdateInventoryUI(InventoryLocation.Box, bagData.itemList);
        }

        private void OnBaseBagCloseEvent(SlotType slotType, InventoryBag_SO bagData)
        {
            // 移除商店UI关闭的逻辑，商店UI始终开启
            if (slotType != SlotType.Shop)
            {
                baseBag.SetActive(false);
                itemTooltip.gameObject.SetActive(false);
                UpdateSlotHighlight(-1);

                foreach (var slot in baseBagSlots)
                {
                    Destroy(slot.gameObject);
                }
                baseBagSlots.Clear();
            }
        }

        private void OnBeforeSceneUnloadEvent()
        {
            UpdateSlotHighlight(-1);
        }

        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch (location)
            {
                case InventoryLocation.Player:
                    for (int i = 0; i < playerSlots.Length; i++)
                    {
                        if (list[i].itemAmout > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            playerSlots[i].UpdateSlot(item, list[i].itemAmout);

                        }
                        else
                        {
                            playerSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
                case InventoryLocation.Box:
                    for (int i = 0; i < baseBagSlots.Count; i++)
                    {
                        if (list[i].itemAmout > 0)
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                            baseBagSlots[i].UpdateSlot(item, list[i].itemAmout);
                        }
                        else
                        {
                            baseBagSlots[i].UpdateEmptySlot();
                        }
                    }
                    break;
            }

            playerMoneyText.text = InventoryManager.Instance.playerMoney.ToString();
        }

        public void OpenBagUI()
        {
            bagOpened = !bagOpened;
            bagUI.SetActive(bagOpened);
        }

        public void UpdateSlotHighlight(int index)
        {
            foreach (var slot in playerSlots)
            {
                if (slot.isSelected && slot.slotIndex == index)
                {
                    slot.slotHighlight.gameObject.SetActive(true);
                }
                else
                {
                    slot.isSelected = false;
                    slot.slotHighlight.gameObject.SetActive(false);
                }
            }
        }
    }

}


