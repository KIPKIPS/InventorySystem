using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {
    public Item Item { get; set; }
    public int Amount { get; set; }
    private Image itemImage;
    private Text amountText;

    private float targetScale = 1f;
    public float animateScale = 1.5f;
    public float smoothSpeed = 20f;

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

    void Update() {
        if (transform.localScale.x != targetScale) {
            //动画
            float scale = Mathf.Lerp(transform.localScale.x, targetScale, Time.deltaTime * smoothSpeed);
            transform.localScale = Vector3.one * scale;
            if (transform.localScale.x - targetScale <= 0.01f) {
                transform.localScale = Vector3.one * targetScale;
            }
        }
    }

    public void SetItem(Item item, int amount = 1) {
        transform.localScale = Vector3.one * animateScale;
        Item = item;
        Amount = amount;
        //更新UI
        ItemImage.sprite = Resources.Load<Sprite>("UI/Icon/" + item.Sprite);
        AmountText.text = Amount.ToString();
    }

    public void AddAmount(int amount = 1) {
        transform.localScale = Vector3.one * animateScale;
        Amount += amount;
        AmountText.text = Amount.ToString();
    }
    public void SetAmount(int amount) {
        transform.localScale = Vector3.one * animateScale;
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