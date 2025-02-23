using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        
        [Header("��Ʒ����")]
        public ItemDataList_SO itemDataList_SO;

        [Header("��������")]
        public InventoryBag_SO playerBag;
        private InventoryBag_SO currentBoxBag;
        [Header("����")]
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
            [Header("����")]
        public BuildingUI buildingUi;
        private void Start()
        {
            // ��ʼ������
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// ͨ��ID�����ض�Ӧ����Ʒ��Ϣ
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        /// <summary>
        /// �����Ʒ��Player������
        /// </summary>
        /// <param name="item"></param>
        /// <param name="toDestory">�Ƿ���Ҫ������Ʒ</param>
        public void AddItem(Item item, bool toDestory)
        {
            // �����Ƿ��������Ʒ
            var index = GetItemIndexInBag(item.itemID);

            // �����Ʒ
            // ��û���� ������������Ʒ���ұ���û�п�λ��ʱ��
            AddItemAtIndex(item.itemID, index, 1);

            if (toDestory)
            {
                Destroy(item.gameObject);
            }

            // ����UI
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// <summary>
        /// ��鱳���Ƿ��п�λ
        /// </summary>
        /// <returns></returns>
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                // ��itemIDΪ0��������λ��
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
                // ��itemIDΪ0��������λ��
                if (playerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// ��ָ���������λ�������Ʒ
        /// </summary>
        /// <param name="ID">��Ʒ��ID</param>
        /// <param name="index">��Ʒ�ڱ��������</param>
        /// <param name="amount">��ӵ�����</param>
        private void AddItemAtIndex(int ID, int index, int amount = 1)
        {
            // index == -1 ����û�������Ʒ
            if (index == -1)
            {
                // 1. ����û�п�λ
                if (!CheckBagCapacity())
                {
                    return;
                }

                // 2. �п�λ
                // �����µ���Ʒ
                var item = new InventoryItem { itemID = ID, itemAmout = amount };

                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    // ��itemIDΪ0��������λ�ã�λ��Ϊ i
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item; // �����Ʒ��Bag
                        break;
                    }
                }

            }
            else  // �����������Ʒ
            {
                int currentAmount = playerBag.itemList[index].itemAmout + amount;
                var item = new InventoryItem { itemID = ID, itemAmout = currentAmount };
                playerBag.itemList[index] = item;
            }
        }

        /// <summary>
        /// Player������Χ�ڽ�����Ʒ
        /// </summary>
        /// <param name="fromIndex">��ʼ���</param>
        /// <param name="targetIndex">Ŀ�����</param>
        public void SwapItem(InventoryLocation locationFrom, int fromIndex, InventoryLocation locationTarget, int targetIndex)
        {
            var currentList = GetItemList(locationFrom);
            var targetList = GetItemList(locationTarget);

            InventoryItem currentItem = currentList[fromIndex];

            if (targetIndex < targetList.Count)
            {
                InventoryItem targetItem = targetList[targetIndex];

                if (targetItem.itemID != 0 && currentItem.itemID != targetItem.itemID)//�в���ͬ��������Ʒ
                {
                    currentList[fromIndex] = targetItem;
                    targetList[targetIndex] = currentItem;
                }
                else if (currentItem.itemID == targetItem.itemID)//����ͬ��������Ʒ
                {
                    targetItem.itemAmout += currentItem.itemAmout;
                    targetList[targetIndex] = targetItem;
                    currentList[fromIndex] = new InventoryItem();
                }
                else//Ŀ��ո���
                {
                    targetList[targetIndex] = currentItem;
                    currentList[fromIndex] = new InventoryItem();
                }
                EventHandler.CallUpdateInventoryUI(locationFrom, currentList);
                EventHandler.CallUpdateInventoryUI(locationTarget, targetList);
            }
        }


        /// <summary>
        /// ����λ�÷��ر��������б�
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
        /// �Ƴ�ָ�������ı�����Ʒ
        /// </summary>
        /// <param name="ID">��ƷID</param>
        /// <param name="removeAmount">����</param>
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


        // ��ȡ�����е���Ʒ����
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
        /// �����ұ����е�������Ʒ
        /// </summary>
        public void ClearInventory()
        {
            // ��ձ����е�������Ʒ
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                playerBag.itemList[i] = new InventoryItem(); // ���ÿ����Ʒ
            }

            // ����UI��ȷ��UIҲ��ӳ��������պ��״̬
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }


        public void TradeItem(ItemDetails itemDetails, int amount, bool isSellTrade)
        {
            int cost = itemDetails.transPrice * amount;
            //��ñ�����Ʒλ��
            int index = GetItemIndexInBag(itemDetails.itemID);

            if (isSellTrade)     //��
            {
                if (playerBag.itemList[index].itemAmout >= amount)
                {
                    RemoveItem(itemDetails.itemID, amount);
                    cost = (int)(cost * itemDetails.sellPercentage);
                    playerMoney += cost;
                }
            }
            else if (playerMoney - cost >= 0)    //��
            {
                if (CheckBagCapacity())
                {
                    AddItemAtIndex(itemDetails.itemID, index, amount);
                }
                playerMoney -= cost;
            }
            //ˢ��UI
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

