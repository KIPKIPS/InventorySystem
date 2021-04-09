using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 只拦截事件，不产生渲染消耗。
/// </summary>
public class UIEventGraphic : MaskableGraphic {
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        //base.OnPopulateMesh(vh);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.material = null;
        base.raycastTarget = true;
        base.OnValidate();
    }
#endif
}
