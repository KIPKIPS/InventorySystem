using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class InventoryManager : BaseSingleton<InventoryManager> {

    private ItemTips itemTips;
    private bool isTipsShow = false;
    private Canvas canvas;

    private Canvas MainCanvas {
        get {
            if (canvas == null) {
                canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            }
            return canvas;
        }
    }
    void Start() {
        AnalysisItemJsonData();
        itemTips = GameObject.FindObjectOfType<ItemTips>();
    }

    void Update() {

    }

    private List<Item> itemList;
    //解析物品的json数据
    public void AnalysisItemJsonData() {
        itemList = new List<Item>();
        //文本zai Unity是TextAsset类型
        List<JObject> list = DataManager.Instance.LoadJsonByPath<List<JObject>>("ConfigData/ItemData.json");
        foreach (JObject jObj in list) {
            string typeStr = jObj.GetValue("type").ToString();
            Item.ItemType type = (Item.ItemType)System.Enum.Parse(typeof(Item.ItemType), typeStr);

            //解析公有字段
            int id = int.Parse(jObj.GetValue("id").ToString());
            string name = jObj.GetValue("name").ToString();
            Item.Quality qualityType = (Item.Quality)System.Enum.Parse(typeof(Item.Quality), jObj.GetValue("quality").ToString());
            string description = jObj.GetValue("description").ToString();
            int capacity = int.Parse(jObj.GetValue("capacity").ToString());
            int buyPrice = int.Parse(jObj.GetValue("buyPrice").ToString());
            int sellPrice = int.Parse(jObj.GetValue("sellPrice").ToString());
            string sprite = jObj.GetValue("sprite").ToString();
            string iconType = jObj.GetValue("iconType").ToString();
            //构造对象
            Item item = null;
            switch (type) {
                case Item.ItemType.Consumable:
                    int hp = int.Parse(jObj.GetValue("hp").ToString());
                    int mp = int.Parse(jObj.GetValue("mp").ToString());
                    item = new Consumable(id, name, type, qualityType, description, capacity, buyPrice, sellPrice, sprite, iconType, hp, mp);
                    break;
                case Item.ItemType.Equipment:
                    int strength = int.Parse(jObj.GetValue("strength").ToString());
                    int intellect = int.Parse(jObj.GetValue("intellect").ToString());
                    int agility = int.Parse(jObj.GetValue("strength").ToString());
                    int stamina = int.Parse(jObj.GetValue("intellect").ToString());
                    Equipment.EquipType equipType = (Equipment.EquipType)System.Enum.Parse(typeof(Equipment.EquipType), jObj.GetValue("equipType").ToString());
                    item = new Equipment(id, name, type, qualityType, description, capacity, buyPrice, sellPrice, sprite, iconType, strength, intellect, agility, stamina, equipType);
                    break;
                case Item.ItemType.Weapon:
                    int damage = int.Parse(jObj.GetValue("damage").ToString());
                    Weapon.WeaponType wType = (Weapon.WeaponType)System.Enum.Parse(typeof(Weapon.WeaponType), jObj.GetValue("weaponType").ToString());
                    item = new Weapon(id, name, type, qualityType, description, capacity, buyPrice, sellPrice, sprite, iconType, damage, wType);
                    break;
                case Item.ItemType.Material:
                    item = new Material(id, name, type, qualityType, description, capacity, buyPrice, sellPrice, sprite, iconType);
                    break;
            }
            itemList.Add(item);
            //Debug.Log(item.IconType);
        }
    }

    public Item GetItemById(int id) {
        foreach (Item item in itemList) {
            if (item.ID == id) {
                //print("find it");
                return item;
            }
        }
        return null;
    }

    public void ShowItemTips(string content) {
        itemTips.Show(content);
        isTipsShow = true;
        // if (isTipsShow) { //tips跟随鼠标
        //
        // }
        //把屏幕坐标转化成Canvas画布上的坐标
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(MainCanvas.transform as RectTransform, Input.mousePosition, null, out position);
        itemTips.SetLocalPosition(position);
    }

    public void HideItemTips() {
        itemTips.Hide();
        isTipsShow = false;
    }
}
