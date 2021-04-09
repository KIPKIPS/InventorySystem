using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTips : MonoBehaviour {
    private Text toolTipText;
    private Text contentText;
    private CanvasGroup canvasGroup;
    private float targetAlpha = 0;
    public float smoothTime = 10f;

    public Text ToolTipText {
        get {
            if (toolTipText == null) {
                toolTipText = GetComponent<Text>();
            }
            return toolTipText;
        }
    }
    public Text ContentText {
        get {
            if (contentText == null) {
                contentText = GameObject.Find("ContentText").GetComponent<Text>();
            }
            return contentText;
        }
    }

    public CanvasGroup CanvasGroup {
        get {
            if (canvasGroup == null) {
                canvasGroup = GetComponent<CanvasGroup>();
            }
            return canvasGroup;
        }
    }
    public void Start() {
    }

    void Update() {
        if (CanvasGroup.alpha != targetAlpha) {
            CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, targetAlpha, smoothTime * Time.deltaTime);
            if (Mathf.Abs(CanvasGroup.alpha - targetAlpha) <= 0.01f) {
                CanvasGroup.alpha = targetAlpha;
            }
        }
    }

    public void Show(string text) {
        targetAlpha = 1;
        ToolTipText.text = text;
        ContentText.text = text;
    }

    public void Hide() {
        targetAlpha = 0;
    }

    public void SetLocalPosition(Vector2 pos) {
        transform.localPosition = pos;
    }
}
