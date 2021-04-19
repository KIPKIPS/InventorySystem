using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolePanel : Inventory {
    private static RolePanel _instatce;
    public static RolePanel Instance {
        get {
            if (_instatce == null) {
                _instatce = GameObject.Find("RolePanel").GetComponent<RolePanel>();
            }
            return _instatce;
        }
    }
    public void PutOn(ItemUI itemUI) {
        ItemUI exitItemUI = null;
        foreach (ItemSlot slot in itemSlotList) {
            EquipSlot equipSlot = (EquipSlot)slot;
            if (equipSlot.CanDress(itemUI.Item)) {
                print(">>>>>>>>");
                if (equipSlot.transform.childCount > 0) { //该部位已经有装备
                    exitItemUI = equipSlot.transform.GetChild(0).GetComponent<ItemUI>();//.Item;//得到身上已有的装备
                    //equipSlot.transform.GetChild(0).GetComponent<ItemUI>().SetItem(exitItem);
                    itemUI.Exchange(exitItemUI);
                } else {
                    equipSlot.StoreItem(itemUI.Item);
                    Destroy(itemUI.gameObject);
                }
                break;
            }
        }
    }

    public void PutOff(ItemUI itemUI, ItemSlot slot) {
        slot.StoreItem(itemUI.Item);
    }
}
