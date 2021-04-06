using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    private ItemSlot[] itemSlotList;
    public virtual void Start() {
        itemSlotList = GetComponentsInChildren<ItemSlot>();
    }

    //存储item,返回一个bool值
    public bool StoreItem(int id) {
        Item item = InventoryManager.Instance.GetItemById(id);
        return StoreItem(item);
    }
    public bool StoreItem(Item item) {
        if (item != null) {
            Debug.LogWarning("存储物品的id不存在");
            return false;
        }

        if (item.Capacity == 1) {
            ItemSlot slot = FindEmptySlot();
            if (slot == null) {
                Debug.LogWarning("没有空的物品槽");
                return false;
            } else {
                slot.StoreItem(item);//存起来
            }
        } else {
            ItemSlot slot = FindSameTypeItemSlot(item);
            if (slot == null) {
                slot.StoreItem(item);
            } else {
                ItemSlot emptySlot = FindEmptySlot();
                if (emptySlot != null) {
                    emptySlot.StoreItem(item);
                } else {
                    Debug.LogWarning("没有空的物品槽");
                    return false;
                }
            }
        }
        return true;
    }

    //查找空格子
    public ItemSlot FindEmptySlot() {
        foreach (ItemSlot slot in itemSlotList) {
            if (slot.transform.childCount == 0) { //子节点为空(没有挂载任何物体),认为是空格子
                return slot;
            }
        }
        return null;
    }

    //查找id相同的物品
    public ItemSlot FindSameTypeItemSlot(Item item) {
        foreach (ItemSlot slot in itemSlotList) {
            if (slot.transform.childCount >= 1 && slot.GetItemType() == item.Type && !slot.IsFilled()) {
                return slot;
            }
        }
        return null;
    }
}
