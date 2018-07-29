using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicDmg : Ability {

	public basicDmg(){
		targetNum = 2;
		targets = new List<Creature> ();
		actionCost = 1;
	}

	public override void useAbility ()
	{
		
		targets [0].takeDmg (1);
		targets [1].takeDmg (1);
		targets.Clear ();
	}

	public override bool verify ()
	{
        return assertEnemyTarget(0) && assertEnemyTarget(1);

    }


}
