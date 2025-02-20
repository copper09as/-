using MFarm.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    // 原有UI引用
    [SerializeField] private Button nextDayButton;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private NPCFunction shop;
    [SerializeField] private GameObject newsPanel;
    [SerializeField] private Text newsUI;
    [SerializeField] private Button closeNewsButton;
    [SerializeField] private Button openNewsButton;
    
    private int _day;
    private Dictionary<int, int> itemBasePrices = new Dictionary<int, int>(); // 存储物品初始价格

    // 新增：家庭状况和初始资金
    public FamilyStatus familyStatus;
    public int startingMoney;

    public int day
    {
        get => _day;
        set
        {
            _day = value;
            dayText.text = value.ToString();
        }
    }

    protected override void Awake()
    {
        base.Awake();
        nextDayButton.onClick.AddListener(DayUpdate);
        closeNewsButton.onClick.AddListener(CloseNews);
        openNewsButton.onClick.AddListener(OpenNews);
        day = 1;
        InventoryManager.Instance.ClearInventory();

        // 每局游戏开始时随机选择家庭状况
        RandomizeFamilyStatus();
    }

    private void Start()
    {
        // 初始化所有物品的初始价格
        foreach (var item in shop.shopData.itemList)
        {
            var details = InventoryManager.Instance.GetItemDetails(item.itemID);
            itemBasePrices[item.itemID] = details.itemPrice; // 记录初始价格
            details.transPrice = details.itemPrice;           // 设置当前交易价格
        }
        newsPanel.SetActive(true);
    }

    // 随机选择家庭状况
    private void RandomizeFamilyStatus()
    {
        familyStatus = (FamilyStatus)Random.Range(0, 3);  // 随机选择 0~2 之间的值，分别对应 贫穷、小康、富裕

        // 根据随机的家庭状况设置金钱
        SetFamilyStatusAndMoney();
    }

    private void SetFamilyStatusAndMoney()
    {
        // 根据家庭状况设置金钱和播报信息
        switch (familyStatus)
        {
            case FamilyStatus.Wealthy:
                startingMoney = 10000;
                newsUI.text = "家庭状况：富裕。你获得了10000元创业资金！";
                break;
            case FamilyStatus.MiddleClass:
                startingMoney = 5000;
                newsUI.text = "家庭状况：小康。你获得了5000元创业资金！";
                break;
            case FamilyStatus.Poor:
                startingMoney = 1000;
                newsUI.text = "家庭状况：贫穷。你获得了1000元创业资金！";
                break;
        }

        // 设置初始金钱
        InventoryManager.Instance.playerMoney = startingMoney;

        // 显示新闻面板
        newsPanel.SetActive(true);
    }

    private void DayUpdate()
    {
        // 生成每日市场事件
        List<string> newsEntries = GenerateMarketEvents();

        // 构建新闻文本
        newsUI.text = $"Day {day + 1} Market Report:\n" +
                      (newsEntries.Count > 0 ? string.Join("\n", newsEntries) : "市场稳定，价格波动正常。");

        day++;          // 进入下一天
        InventoryManager.Instance.BuildinGain();
        newsPanel.SetActive(true); // 显示新闻面板
        

    }

    private List<string> GenerateMarketEvents()
    {
        List<string> events = new List<string>();
        int eventCount = Random.Range(0, 4); // 随机0~3条事件

        // 随机选择要影响的物品
        var candidates = shop.shopData.itemList.OrderBy(x => Random.value).Take(eventCount).ToList();

        foreach (var item in shop.shopData.itemList)
        {
            var details = InventoryManager.Instance.GetItemDetails(item.itemID);
            int basePrice = itemBasePrices[item.itemID];
            int previousPrice = details.transPrice;

            // 判断是否被选中为市场事件
            MarketCondition condition = candidates.Contains(item) ?
                (Random.Range(0, 2) == 0 ? MarketCondition.Hot : MarketCondition.Cold) :
                MarketCondition.Stable;

            // 计算新价格
            int newPrice = PriceCalculator.CalculateNewPrice(condition, basePrice);
            details.transPrice = newPrice;

            // 记录需要播报的事件
            if (condition != MarketCondition.Stable)
            {
                events.Add($"{details.itemName}: {previousPrice} -> {newPrice} ({(condition == MarketCondition.Hot ? "大热🔥" : "大冷❄️")})");
            }
        }

        return events;
    }

    private void CloseNews() => newsPanel.SetActive(false);
    private void OpenNews() => newsPanel.SetActive(true);
}
