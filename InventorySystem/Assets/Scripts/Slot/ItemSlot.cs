using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject itemPrefab;
    private ItemUI itemUI;

    public ItemUI ItemUI {
        get {
            if (itemUI == null) {
                itemUI = transform.GetChild(0).GetComponent<ItemUI>();
            }
            return itemUI;
        }
    }
    //将item放在itemSlot下,若存在slot,则物品数量增加1
    public void StoreItem(Item item) {
        if (transform.childCount == 0) {
            GameObject itemGameObject = Instantiate(itemPrefab);
            itemGameObject.transform.SetParent(transform);
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.transform.localRotation = Quaternion.identity;
            if (itemUI == null) {
                itemUI = itemGameObject.GetComponent<ItemUI>();
            }
            itemUI.SetItem(item);
        } else {
            ItemUI.AddAmount();
        }
    }

    //得到物品槽存储的物品类型
    public Item.ItemType GetItemType() {
        return transform.GetChild(0).GetComponent<ItemUI>().Item.Type;
    }

    public int GetItemID() {
        return itemUI.Item.ID;
    }

    public bool IsFilled() {
        return itemUI.Amount >= itemUI.Item.Capacity;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (transform.childCount > 0) {
            InventoryManager.Instance.HideItemTips();
        }
    }
    public void OnPointerEnter(PointerEventData eventData) {
        if (transform.childCount > 0) {
            string text = ItemUI.Item.GetItemDesc();
            InventoryManager.Instance.ShowItemTips(text);
        }
    }
}
