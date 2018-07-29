using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dodge : Ability {
    public Samurai sam;

    public dodge(Samurai s) {
        targetNum = 0;
        actionCost = 1;
        sam = s;
    }

    public override void useAbility() {
        sam.dodge = true;
    }

    public override bool verify() {
        return true;
    }
}
