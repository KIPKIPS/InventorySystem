using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : ItemSlot {
    // Start is called before the first frame update
    public Equipment.EquipType equipType; //物品槽可以存储的类型
    public Weapon.WeaponType weaponType;

    public override void OnPointerDown(PointerEventData eventData) {
        //持有物品
        //目标格子有装备
        //装备类型一致,替换
        //不一致,不操作
        //目标格子无装备
        //装备类型一致,装上
        //装备类型不一致,返回
        //未持有物品
        //目标格子有装备卸下
        //目标格子无装备,返回
        if (InventoryManager.Instance.IsPickItem) { //手上有物品
            ItemUI pickItem = InventoryManager.Instance.PickedItem;
            bool canStoreEquip = pickItem.Item is Equipment && ((Equipment)pickItem.Item).EquipmentType == equipType;//是否是装备且类型一致
            bool canStoreWeapon = pickItem.Item is Weapon && ((Weapon)pickItem.Item).Type == weaponType;//是否是武器且类型一致
            if (transform.childCount > 0) { //目标格子有物品
                ItemUI curItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                if (canStoreEquip || canStoreWeapon) { //满足存储条件
                    curItemUI.Exchange(pickItem);//交换物品
                }
            } else { //目标格子没有物品
                if (canStoreEquip || canStoreWeapon) { //满足存储条件
                    StoreItem(pickItem.Item, 1);
                    InventoryManager.Instance.PickUpItemAmountMinus();
                }
            }
        } else { //手上没物品,要卸下装备
            if (transform.childCount > 0) { //目标格子有物品
                ItemUI curItemUI = transform.GetChild(0).GetComponent<ItemUI>();
                InventoryManager.Instance.PickUpItem(curItemUI.Item);
                Destroy(transform.GetChild(0).gameObject);
            } else { //目标格子没有物品
                return;
            }

        }
    }
}

