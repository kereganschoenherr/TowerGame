using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Ability {

    
    public Shield() {
        targetNum = 1;
        targets = new List<Creature>();
        actionCost = 1;
    }

    public override void useAbility()
    {
        targets[0].effectList.Add(new ShieldEffect());
    }

    public override bool verify()
    {
        Debug.Log(targets.Count);
        return assertCharacterTarget(0);
    }


}
