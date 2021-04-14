using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    private ItemSlot[] itemSlotList;
    public float targetAlpha = 1;
    public float smoothSpeed = 5;
    private CanvasGroup canvasGroup;

    public virtual void Update() {
        if (targetAlpha != canvasGroup.alpha) {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothSpeed * Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) <= 0.01f) {
                canvasGroup.alpha = targetAlpha;
            }
        }
    }
    public virtual void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        itemSlotList = CreateItemSlotList();
    }

    public ItemSlot[] CreateItemSlotList() {
        ItemSlot[] list = transform.Find("ContentBg").Find("Container").GetComponentsInChildren<ItemSlot>();
        return list;
    }

    //存储item,返回一个bool值
    public bool StoreItem(int id) {
        Item item = InventoryManager.Instance.GetItemById(id);
        //print(item == null);
        return StoreItem(item);
    }
    public bool StoreItem(Item item) {
        if (item == null) {
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
            ItemSlot slot = FindSameIdItemSlot(item);
            if (slot != null) {
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
                //print("get empty");
                return slot;
            }
        }
        return null;
    }

    //查找id相同的物品
    public ItemSlot FindSameIdItemSlot(Item item) {
        foreach (ItemSlot slot in itemSlotList) {
            if (slot.transform.childCount >= 1 && slot.GetItemID() == item.ID && !slot.IsFilled()) {
                return slot;
            }
        }
        return null;
    }

    public void Show() {
        targetAlpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide() {
        targetAlpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void SwitchPanelDisplayStatus() {
        if (targetAlpha == 0) {
            Show();
        } else {
            Hide();
        }
    }
}
