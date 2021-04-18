using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
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
    public void StoreItem(Item item, int amount) {
        if (transform.childCount == 0) {
            GameObject itemGameObject = Instantiate(itemPrefab);
            itemGameObject.transform.SetParent(transform);
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.transform.localRotation = Quaternion.identity;
            if (itemUI == null) {
                itemUI = itemGameObject.GetComponent<ItemUI>();
            }
            itemUI.SetItem(item, amount);
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
            InventoryManager.Instance.ShowItemTips(text, new Vector2(transform.position.x, transform.position.y));
        }
    }

    //鼠标Down下,拾起物品
    public virtual void OnPointerDown(PointerEventData eventData) {

        // 空格子
        // pickItem非空,把pickItem放置到格子上
        // ctrl,放置pickItem单个物品
        // not ctrl,全部放置

        // 非空格子
        // pickItem不为空
        // 同id的物品,放下
        // 全部放得下
        // ctrl 每次放一个
        // not ctrl 全部放置
        // 放下一部分
        // ctrl 每次放一个
        // not ctrl 全部放置
        // 不同id,把pickItem的物品与自身物品交换

        // pickItem为空,拾起物品,放到pickItem上
        // ctrl 拾起一半
        // not ctrl 全部拾起
        if (eventData.button == PointerEventData.InputButton.Left) { //左键
            bool keepLeftOrRightCtrlKey = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            if (transform.childCount > 0) { //非空格子
                ItemUI curItem = transform.GetChild(0).GetComponent<ItemUI>();
                if (InventoryManager.Instance.IsPickItem == false) { //没拾起的物品,处理拾取物品
                    if (keepLeftOrRightCtrlKey) { //ctrl
                        int amountPicked = Mathf.CeilToInt(curItem.Amount / 2f);
                        //Debug.Log(amountPicked);
                        InventoryManager.Instance.PickUpItem(curItem.Item, amountPicked);
                        if (curItem.Amount - amountPicked <= 0) { //剩余物品数量小于0,销毁格子物品
                            Debug.Log(curItem.Amount);
                            Destroy(curItem.gameObject);
                        } else { //剩余物品数量大于0,更新数量
                            curItem.SetAmount(curItem.Amount - amountPicked);
                        }

                    } else { //not ctrl
                        InventoryManager.Instance.PickUpItem(curItem.Item, curItem.Amount);
                        Destroy(curItem.gameObject);
                    }
                } else { //有拾起的物品,处理放下
                    if (curItem.Item.ID == InventoryManager.Instance.PickedItem.Item.ID) { //id一致
                        if (keepLeftOrRightCtrlKey) {
                            if (curItem.Item.Capacity > curItem.Amount) { //有容量
                                curItem.AddAmount();//格子加一个
                                InventoryManager.Instance.PickUpItemAmountMinus();//手上减一
                            } else {
                                Debug.LogWarning("当前格子容量不足,放不下了");
                                return;//没容量,放不下
                            }
                        } else {
                            int pickAmount = InventoryManager.Instance.PickedItem.Amount;
                            if (curItem.Item.Capacity > curItem.Amount) { //有容量,可以放下
                                int remainCapacity = curItem.Item.Capacity - curItem.Amount;
                                int dropAmount = remainCapacity >= pickAmount ? pickAmount : remainCapacity;
                                curItem.AddAmount(dropAmount);//格子加上可放置的
                                InventoryManager.Instance.PickUpItemAmountMinus(dropAmount);//手上减去可放置的
                            } else {
                                Debug.LogWarning("当前格子容量不足,放不下了");
                                return;//没容量,放不下
                            }
                        }
                    } else {
                        // Debug.LogWarning("物品不是同一种类");
                        // return;
                        //处理交换,不分ctrl or not ctrl
                        // Item cacheCurItem = curItem.Item;
                        // int cacheAmount = curItem.Amount;
                        // curItem.SetItem(InventoryManager.Instance.PickedItem.Item, InventoryManager.Instance.PickedItem.Amount);
                        // InventoryManager.Instance.PickedItem.SetItem(cacheCurItem, cacheAmount);
                        curItem.Exchange(InventoryManager.Instance.PickedItem);
                    }
                }
            } else { //空格子
                if (InventoryManager.Instance.IsPickItem) { //手上有物品
                    if (keepLeftOrRightCtrlKey) {//ctrl 放一个
                        StoreItem(InventoryManager.Instance.PickedItem.Item);
                        InventoryManager.Instance.PickUpItemAmountMinus();
                    } else { //全部放下
                        StoreItem(InventoryManager.Instance.PickedItem.Item, InventoryManager.Instance.PickedItem.Amount);
                        InventoryManager.Instance.PickUpItemAmountMinus(InventoryManager.Instance.PickedItem.Amount);
                    }
                } else { //二者皆空,不处理
                    return;
                }
            }
        } else if (eventData.button == PointerEventData.InputButton.Right) {//右键就直接穿戴
            if (transform.childCount > 0) {
                ItemUI curitemUI = transform.GetChild(0).GetComponent<ItemUI>();
                if (curitemUI != null && (curitemUI.Item is Equipment || curitemUI.Item is Weapon)) {
                    RolePanel.Instance.PutOn(curitemUI.Item);
                    curitemUI.ReduceAmount();
                    if (curitemUI.Amount <= 0) {
                        Destroy(curitemUI.gameObject);
                    }
                }
            }
        }

    }

}
