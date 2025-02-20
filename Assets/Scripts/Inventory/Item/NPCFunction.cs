using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFunction : MonoBehaviour
{
    public InventoryBag_SO shopData;

    private void Start()
    {
        OpenShop();  // 在游戏开始时就直接打开商店
 
    }

    private void OpenShop()
    {
        // 直接开启商店
        EventHandler.CallBaseBagOpenEvent(SlotType.Shop, shopData);
        EventHandler.CallUpdateGameStateEvent(GameState.Pause);
    }
}
