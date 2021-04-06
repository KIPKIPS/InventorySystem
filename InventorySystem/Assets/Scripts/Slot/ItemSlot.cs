using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour {
    //将item放在itemSlot下,若存在slot,则物品数量增加1
    public void StoreItem(Item item) {

    }

    //得到物品槽存储的物品类型
    public Item.ItemType GetItemType() {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Type;
    }

    public bool IsFilled() {
        ItemUI itemUI = transform.GetChild(0).GetComponent<ItemUI>();
        return itemUI.Amount >= itemUI.Item.Capacity;
    }
}
