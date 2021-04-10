using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//消耗品类
public class Consumable : Item {
    // Start is called before the first frame update
    public int HP { get; set; }
    public int MP { get; set; }
    //继承父类的构造方法
    public Consumable(int _id, string _name, ItemType _itemType, Quality _qualityType, string _desc, int _capacity, int _buyPrice, int _sellPrice, string _sprite, string _iconType,
        int _hp, int _mp)
        : base(_id, _name, _itemType, _qualityType, _desc, _capacity, _buyPrice, _sellPrice, _sprite, _iconType) {
        HP = _hp;
        MP = _mp;
    }

    public override string GetItemDesc() {
        string text = base.GetItemDesc();
        text += "\n" + "回复生命值" + HP;
        text += "\n" + "回复法力值" + MP;
        return text;
    }
}
