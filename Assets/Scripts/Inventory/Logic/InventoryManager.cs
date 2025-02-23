using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;

        [Header("背包数据")]
        public InventoryBag_SO playerBag;
        private InventoryBag_SO currentBoxBag;
        [Header("交易")]
        [SerializeField]private int _playerMoney;
        protected override void Awake()
        {
            base.Awake();
        }
        public int playerMoney
        {
            
            get
            {
                return _playerMoney;
            }   
            set
            {
                _playerMoney = value;
                
                EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
            }
        }
            [Header("建筑")]
        public BuildingUI buildingUi;
        private void Start()
        {
            // 初始化背包
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// 通过ID，返回对应的物品信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        /// <summary>
        /// 添加物品到Player背包里
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">是否需要销毁物品</param>
        public void AddItem(Item item, bool toDestory)
        {
            // 背包是否有这个物品
            var index = GetItemIndexInBag(item.itemID);

            // 添加物品
            // 还没处理： 背包不存在物品，且背包没有空位的时候
            AddItemAtIndex(item.itemID, index, 1);

            if (toDestory)
            {
                Destroy(item.gameObject);
            }

            // 更新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// 检查背包是否有空位
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                // 当itemID为0，背包有位置
                if (playerBag.itemList[i].itemID == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                // 当itemID为0，背包有位置
                if (playerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 在指定背包序号位置添加物品
        /// </summary>
        /// <param name="ID">物品的ID</param>
        /// <param name="index">物品在背包的序号</param>
        /// <param name="amount">添加的数量</param>
        private void AddItemAtIndex(int ID, int index, int amount = 1)
        {
            // index == -1 背包没有这个物品
            if (index == -1)
            {
                // 1. 背包没有空位
                if (!CheckBagCapacity())
                {
                    return;
                }

                // 2. 有空位
                // 生成新的物品
                var item = new InventoryItem { itemID = ID, itemAmout = amount };

                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    // 当itemID为0，背包有位置，位置为 i
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item; // 添加物品到Bag
                        break;
                    }
                }

            }
            else  // 背包有这个物品
            {
                int currentAmount = playerBag.itemList[index].itemAmout + amount;
                var item = new InventoryItem { itemID = ID, itemAmout = currentAmount };
                playerBag.itemList[index] = item;
            }
        }

        /// <summary>
        /// Player背包范围内交换物品
        /// </summary>
        /// <param name="fromIndex">起始序号</param>
        /// <param name="targetIndex">目标序号</param>
        public void SwapItem(InventoryLocation locationFrom, int fromIndex, InventoryLocation locationTarget, int targetIndex)
        {
            var currentList = GetItemList(locationFrom);
            var targetList = GetItemList(locationTarget);

            InventoryItem currentItem = currentList[fromIndex];

            if (targetIndex < targetList.Count)
            {
                InventoryItem targetItem = targetList[targetIndex];

                if (targetItem.itemID != 0 && currentItem.itemID != targetItem.itemID)//有不相同的两个物品
                {
                    currentList[fromIndex] = targetItem;
                    targetList[targetIndex] = currentItem;
                }
                else if (currentItem.itemID == targetItem.itemID)//有相同的两个物品
                {
                    targetItem.itemAmout += currentItem.itemAmout;
                    targetList[targetIndex] = targetItem;
                    currentList[fromIndex] = new InventoryItem();
                }
                else//目标空格子
                {
                    targetList[targetIndex] = currentItem;
                    currentList[fromIndex] = new InventoryItem();
                }
                EventHandler.CallUpdateInventoryUI(locationFrom, currentList);
                EventHandler.CallUpdateInventoryUI(locationTarget, targetList);
            }
        }


        /// <summary>
        /// 根据位置返回背包数据列表
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private List<InventoryItem> GetItemList(InventoryLocation location)
        {
            return location switch
            {
                InventoryLocation.Player => playerBag.itemList,
                InventoryLocation.Box => currentBoxBag.itemList,
                _ => null,
            };
        }
        /// <summary>
        /// 移除指定数量的背包物品
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <param name="removeAmount">数量</param>
        public void RemoveItem(int ID, int removeAmount)
        {
            var index = GetItemIndexInBag(ID);

            if (playerBag.itemList[index].itemAmout > removeAmount)
            {
                var amount = playerBag.itemList[index].itemAmout - removeAmount;
                var item = new InventoryItem { itemID = ID, itemAmout = amount };
                playerBag.itemList[index] = item;
            }
            else if (playerBag.itemList[index].itemAmout == removeAmount)
            {
                var item = new InventoryItem();
                playerBag.itemList[index] = item;
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }


        // 获取背包中的物品数量
        public int GetItemAmountInBag(int itemID)
        {
            var index = GetItemIndexInBag(itemID);
            if (index >= 0)
            {
                return playerBag.itemList[index].itemAmout;
            }
            return 0;
        }


        /// <summary>
        /// 清空玩家背包中的所有物品
        /// </summary>
        public void ClearInventory()
        {
            // 清空背包中的所有物品
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                playerBag.itemList[i] = new InventoryItem(); // 清空每个物品
            }

            // 更新UI，确保UI也反映出背包清空后的状态
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }


        public void TradeItem(ItemDetails itemDetails, int amount, bool isSellTrade)
        {
            int cost = itemDetails.transPrice * amount;
            //获得背包物品位置
            int index = GetItemIndexInBag(itemDetails.itemID);

            if (isSellTrade)     //卖
            {
                if (playerBag.itemList[index].itemAmout >= amount)
                {
                    RemoveItem(itemDetails.itemID, amount);
                    cost = (int)(cost * itemDetails.sellPercentage);
                    playerMoney += cost;
                }
            }
            else if (playerMoney - cost >= 0)    //买
            {
                if (CheckBagCapacity())
                {
                    AddItemAtIndex(itemDetails.itemID, index, amount);
                }
                playerMoney -= cost;
            }
            //刷新UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
    
    public void BuildinGain()
        {
            foreach (var build in buildingUi.slots)
            {
                if (build.isFinish)
                {
                    InventoryManager.Instance.playerMoney += build.buildingDetails.gain;
                    Debug.Log(InventoryManager.Instance.playerMoney.ToString());
                }
            }
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
    }
}

