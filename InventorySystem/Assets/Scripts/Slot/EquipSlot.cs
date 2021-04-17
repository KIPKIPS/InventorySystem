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

    }
}

