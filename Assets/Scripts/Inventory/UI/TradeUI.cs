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
    public TextMeshProUGUI totalCostText;  // ��ʾ���ѽ�ҵ��ı�
    public Button submitButton;
    public Button cancelButton;
    public Button minButton;  // ��С������ť
    public Button maxButton;  // ���������ť

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
    /// ����TradeUI��ʾ����
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
        UpdateTotalCost();  // ���»�����ʾ
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
            Debug.LogWarning("û������");
            return;
        }
   
        // ��ֹ������Ч����
        if (amount < 0 ) return;

        InventoryManager.Instance.TradeItem(item, amount, isSellTrade);

        CancelTrade();
    }

    private void CancelTrade()
    {
        this.gameObject.SetActive(false); // �رս���UI
    }

    private void SetMinAmount()
    {
        // ��С����Ϊ 1
        tradeAmount.text = "1";
        UpdateTotalCost();
    }

    private void SetMaxAmount()
    {
        int maxAmount = 0;

        if (isSellTrade)
        {
            // ����Ʒʱ�������Ϊ��ǰ��Ʒ����
            var index = InventoryManager.Instance.GetItemIndexInBag(item.itemID);
            if (index >= 0)
            {
                maxAmount = InventoryManager.Instance.playerBag.itemList[index].itemAmout;
            }
        }
        else
        {
            // ����Ʒʱ�������Ϊ��ҵĽ�Ǯ�� / ��Ʒ�۸�
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
            // ����������
            int totalCost = item.transPrice * amount;
            totalCostText.text = $"Spend: {totalCost}";
        }
        else
        {
            totalCostText.text = $"Spend:0";
        }
    }
}
