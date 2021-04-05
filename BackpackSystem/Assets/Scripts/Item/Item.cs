using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//物品基类
public class Item {
    //使用属性来方便控制访问权限
    public int ID { get; set; }
    public string Name { get; set; }
    public ItemType Type { get; set; }//物品类型
    public Quality QualityType { get; set; }//品质类型
    public string Description { get; set; }//物品描述
    public int Capacity { get; set; }
    public int BuyPrice { get; set; }//购买价格
    public int SellPrice { get; set; }//出售价格

    public Item(int _id, string _name, ItemType _itemType, Quality _qualityType, string _desc, int _capacity, int _buyPrice, int _sellPrice) {
        ID = _id;
        Name = _name;
        Type = _itemType;
        QualityType = _qualityType;
        Description = _desc;
        Capacity = _capacity;
        BuyPrice = _buyPrice;
        SellPrice = _sellPrice;
    }

    public enum ItemType {
        Consumable,//消耗品
        Equipment,//装备
        Weapon,//武器
        Material,//材料
    }
    public enum Quality {
        Common,//普通
        Uncommon,//不普通的
        Rare,//稀有的
        Epic,//史诗的
        Legendary,//传说的
        Artifact,//神话的
    }
}
