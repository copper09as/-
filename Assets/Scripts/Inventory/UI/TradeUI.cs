using MFarm.Inventory;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class TradeUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public InputField tradeAmount;
    public TextMeshProUGUI totalCostText;  // 显示花费金币的文本
    public Button submitButton;
    public Button cancelButton;
    public Button minButton;  // 最小数量按钮
    public Button maxButton;  // 最大数量按钮

    private ItemDetails item;
    private bool isSellTrade;

    private void Awake()
    {
        cancelButton.onClick.AddListener(CancelTrade);
        submitButton.onClick.AddListener(TradeItem);
        minButton.onClick.AddListener(SetMinAmount);
        maxButton.onClick.AddListener(SetMaxAmount);
    }

    /// <summary>
    /// 设置TradeUI显示详情
    /// </summary>
    /// <param name="item"></param>
    /// <param name="isSell"></param>
    public void SetupTradeUI(ItemDetails item, bool isSell)
    {
        this.item = item;
        itemIcon.sprite = item.itemIcon;
        itemName.text = item.itemName;
        isSellTrade = isSell;
        tradeAmount.text = string.Empty;
        UpdateTotalCost();  // 更新花费显示
    }

    private void TradeItem()
    {
        int amount;
        try
        {
            amount = Convert.ToInt32(tradeAmount.text);
        }
        catch
        {
            Debug.LogWarning("没有输入");
            return;
        }
   
        // 防止输入无效数量
        if (amount < 0 ) return;

        InventoryManager.Instance.TradeItem(item, amount, isSellTrade);

        CancelTrade();
    }

    private void CancelTrade()
    {
        this.gameObject.SetActive(false); // 关闭交易UI
    }

    private void SetMinAmount()
    {
        // 最小数量为 1
        tradeAmount.text = "1";
        UpdateTotalCost();
    }

    private void SetMaxAmount()
    {
        int maxAmount = 0;

        if (isSellTrade)
        {
            // 卖物品时最大数量为当前物品数量
            var index = InventoryManager.Instance.GetItemIndexInBag(item.itemID);
            if (index >= 0)
            {
                maxAmount = InventoryManager.Instance.playerBag.itemList[index].itemAmout;
            }
        }
        else
        {
            // 买物品时最大数量为玩家的金钱数 / 物品价格
            maxAmount = InventoryManager.Instance.playerMoney / item.transPrice;
        }

        tradeAmount.text = maxAmount.ToString();
        UpdateTotalCost();
    }

    private void UpdateTotalCost()
    {
        int amount = 0;

        if (int.TryParse(tradeAmount.text, out amount) && amount > 0)
        {
            // 计算所需金币
            int totalCost = item.transPrice * amount;
            totalCostText.text = $"Spend: {totalCost}";
        }
        else
        {
            totalCostText.text = $"Spend:0";
        }
    }
}
