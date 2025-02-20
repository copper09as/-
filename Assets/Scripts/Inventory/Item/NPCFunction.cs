using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFunction : MonoBehaviour
{
    public InventoryBag_SO shopData;

    private void Start()
    {
        OpenShop();  // ����Ϸ��ʼʱ��ֱ�Ӵ��̵�
 
    }

    private void OpenShop()
    {
        // ֱ�ӿ����̵�
        EventHandler.CallBaseBagOpenEvent(SlotType.Shop, shopData);
        EventHandler.CallUpdateGameStateEvent(GameState.Pause);
    }
}
