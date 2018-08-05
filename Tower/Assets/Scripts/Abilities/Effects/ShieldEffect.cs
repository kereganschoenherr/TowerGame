using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : Effect, IOnTakeDmg {

    public ShieldEffect() {
        removeMe = true;
    }
    public float getModifier() {
        return 0;
    }
}
