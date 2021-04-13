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
    public void OnPointerDown(PointerEventData eventData) {

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
        if (transform.childCount > 0) { //非空格子
            ItemUI curItmeUI = transform.GetChild(0).GetComponent<ItemUI>();
            if (InventoryManager.Instance.IsPickItem == false) { //没拾起的物品,处理拾取物品
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) { //ctrl
                    int amountPicked = Mathf.CeilToInt(curItmeUI.Amount / 2f);
                    Debug.Log(amountPicked);
                    InventoryManager.Instance.PickUpItem(curItmeUI.Item, amountPicked);
                    if (curItmeUI.Amount - amountPicked <= 0) { //剩余物品数量小于0,销毁格子物品
                        Debug.Log(curItmeUI.Amount);
                        Destroy(curItmeUI.gameObject);
                    } else { //剩余物品数量大于0,更新数量
                        curItmeUI.SetAmount(curItmeUI.Amount - amountPicked);
                    }

                } else { //not ctrl
                    InventoryManager.Instance.PickUpItem(curItmeUI.Item, curItmeUI.Amount);
                    Destroy(curItmeUI.gameObject);
                }
            } else { //有拾起的物品,处理放下
                if (curItmeUI.Item.ID == InventoryManager.Instance.PickedItem.Item.ID) { //id一致
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) {
                        if (curItmeUI.Item.Capacity > curItmeUI.Amount) { //有容量
                            curItmeUI.AddAmount();//格子加一个
                            InventoryManager.Instance.PickUpItemAmountMinus();//手上减一
                        } else {
                            Debug.LogWarning("当前格子容量不足,放不下了");
                            return;//没容量,放不下
                        }
                    } else {
                        int pickAmount = InventoryManager.Instance.PickedItem.Amount;
                        if (curItmeUI.Item.Capacity > curItmeUI.Amount) { //有容量,可以放下
                            int remainCapacity = curItmeUI.Item.Capacity - curItmeUI.Amount;
                            int dropAmount = remainCapacity >= pickAmount ? pickAmount : remainCapacity;
                            curItmeUI.AddAmount(dropAmount);//格子加上可放置的
                            InventoryManager.Instance.PickUpItemAmountMinus(dropAmount);//手上减去可放置的
                        } else {
                            Debug.LogWarning("当前格子容量不足,放不下了");
                            return;//没容量,放不下
                        }
                    }
                } else {
                    Debug.LogWarning("物品不是同一种类");
                    return;
                }
            }
        } else {

        }
    }

}
