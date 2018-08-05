using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killingBlow : Ability {

	public killingBlow(){
		targetNum = 1;
		targets = new List<Creature> ();
		actionCost = 1;
	}

	public override void useAbility ()
	{
		if (targets [0].health < 5) {
			targets [0].takeDmg (4);
		} else {
			targets [0].takeDmg (3);
		}

		targets.Clear ();
	}

	public override bool verify ()
	{

		return true;

	}
}
