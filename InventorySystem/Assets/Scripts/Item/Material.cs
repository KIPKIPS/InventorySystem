using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//材料类
public class Material : Item {
    public Material(int _id, string _name, ItemType _itemType, Quality _qualityType, string _desc, int _capacity, int _buyPrice, int _sellPrice, string _sprite, string _iconType)
        : base(_id, _name, _itemType, _qualityType, _desc, _capacity, _buyPrice, _sellPrice, _sprite, _iconType) {
    }
}
