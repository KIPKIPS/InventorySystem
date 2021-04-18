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
    public void PutOn(Item item) {
        Item exitItem = null;
        foreach (ItemSlot slot in itemSlotList) {
            EquipSlot equipSlot = (EquipSlot)slot;
            if (equipSlot.CanDress(item)) {
                if (equipSlot.transform.childCount > 0) { //该部位已经有装备
                    // if (expr) {
                    //     
                    // }
                } else {
                    equipSlot.StoreItem(item);
                }

            }
        }
    }

    public void PutOff(Item item) {

    }
}
