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
        // 同id的物品,直接放下
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

                } else { //not ctrl

                    InventoryManager.Instance.PickedItem.SetItemUI(curItmeUI);
                    InventoryManager.Instance.PickedItem.Show();
                    InventoryManager.Instance.IsPickItem = true;
                    Destroy(curItmeUI.gameObject);
                }
            }
        } else {

        }
    }

}
