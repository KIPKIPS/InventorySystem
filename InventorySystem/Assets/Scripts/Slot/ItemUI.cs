using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUI : MonoBehaviour {
    public Item Item { get; set; }
    public int Amount { get; set; }

    public void SetItem(Item item, int amount = 1) {
        Item = item;
        Amount = amount;
        //更新UI
    }

    public void AddAmount(int amount = 1) {
        Amount += amount;
    }
}
