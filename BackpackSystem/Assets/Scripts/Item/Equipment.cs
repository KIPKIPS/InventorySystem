using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item {
    public Equipment(int _id, string _name, ItemType _itemType, Quality _qualityType, string _desc, int _capacity, int _buyPrice, int _sellPrice)
        : base(_id, _name, _itemType, _qualityType, _desc, _capacity, _buyPrice, _sellPrice) {

    }

}
