using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnapsackPanel : Inventory {
    private static KnapsackPanel _instatce;

    public static KnapsackPanel Instance {
        get {
            if (_instatce == null) {
                _instatce = GameObject.Find("KnapsackPanel").GetComponent<KnapsackPanel>();
            }
            return _instatce;
        }
    }
}
