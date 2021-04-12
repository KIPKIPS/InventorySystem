using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {
    public Item Item { get; set; }
    public int Amount { get; set; }
    private Image itemImage;
    private Text amountText;

    private Image ItemImage {
        get {
            if (itemImage == null)
                itemImage = GetComponent<Image>();
            return itemImage;
        }
    }

    private Text AmountText {
        get {
            if (amountText == null)
                amountText = transform.GetChild(0).GetComponent<Text>();
            return amountText;
        }
    }

    void Start() {

    }

    public void SetItem(Item item, int amount = 1) {
        Item = item;
        Amount = amount;
        //更新UI
        ItemImage.sprite = Resources.Load<Sprite>("UI/Icon/" + item.Sprite);
        AmountText.text = Amount.ToString();
    }

    public void AddAmount(int amount = 1) {
        Amount += amount;
        AmountText.text = Amount.ToString();
    }
    public void SetAmount(int amount) {
        Amount = amount;
        AmountText.text = Amount.ToString();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void SetLocalPosition(Vector3 pos) {
        transform.localPosition = pos;
    }

    public void SetItemUI(ItemUI itemUI) {
        SetItem(itemUI.Item, itemUI.Amount);
    }
}