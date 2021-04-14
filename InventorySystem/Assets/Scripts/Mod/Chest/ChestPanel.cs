using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestPanel : Inventory {
    private static ChestPanel _instatce;

    public static ChestPanel Instance {
        get {
            if (_instatce == null) {
                _instatce = GameObject.Find("ChestPanel").GetComponent<ChestPanel>();
            }
            return _instatce;
        }
    }
}
