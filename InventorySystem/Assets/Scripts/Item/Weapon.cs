using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {
    public int Damage { get; set; }
    public WeaponType Type { get; set; }
    //继承父类的构造方法
    public Weapon(int _id, string _name, ItemType _itemType, Quality _qualityType, string _desc, int _capacity, int _buyPrice, int _sellPrice, string _sprite, string _iconType,
        int _damage, WeaponType _type)
        : base(_id, _name, _itemType, _qualityType, _desc, _capacity, _buyPrice, _sellPrice, _sprite, _iconType) {
        Type = _type;
        Damage = _damage;
    }
    public enum WeaponType {
        None,
        MainHand,
        OffHand,
    }

    public override string GetItemDesc() {
        string text = base.GetItemDesc();
        text += "\n" + "伤害值: " + Damage;
        text += "\n" + "武器类型" + Type.ToString();
        return text;
    }
}
