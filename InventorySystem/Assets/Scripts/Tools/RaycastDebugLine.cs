using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastDebugLine : MonoBehaviour {
#if UNITY_EDITOR

    public enum DrawMode {
        None = 1,
        Editor = 2,
        Playing = 4,
    }

    [SerializeField]
    private bool draw = true;
    [SerializeField]
    private bool drawInPlaying = false;
    [SerializeField]
    private bool showDeactive = true;

    [SerializeField]
    private GameObject drawTarget;

    [SerializeField]
    private Color color = new Color(0.6f, 0.5f, 1f);

    private static Vector3[] fourCorners = new Vector3[4];

    private void OnDrawGizmos() {
        if (!this.draw) return;
        if (!this.drawInPlaying && Application.isPlaying) return;

        Graphic[] gs = null;
        if (this.drawTarget == null) {
            gs = GameObject.FindObjectsOfType<Graphic>();
        } else {
            gs = this.drawTarget.GetComponentsInChildren<Graphic>(this.showDeactive);
        }

        Color lastColor = Gizmos.color;
        {
            Gizmos.color = color;
            for (int i = 0; i < gs.Length; i++) {
                Graphic g = gs[i];
                if (g.raycastTarget) {
                    RectTransform rectTransform = g.rectTransform;
                    rectTransform.GetWorldCorners(fourCorners);

                    Gizmos.DrawLine(fourCorners[0], fourCorners[1]);
                    Gizmos.DrawLine(fourCorners[1], fourCorners[2]);
                    Gizmos.DrawLine(fourCorners[2], fourCorners[3]);
                    Gizmos.DrawLine(fourCorners[3], fourCorners[0]);
                }
            }
        }
        Gizmos.color = lastColor;
    }

    private static bool HasFlags(DrawMode m1, DrawMode m2) {
        return (m1 & m2) == m2;
    }


#endif
}
