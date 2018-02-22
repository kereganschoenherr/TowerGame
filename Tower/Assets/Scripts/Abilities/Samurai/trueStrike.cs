using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trueStrike : Ability {

	public trueStrike(){
		targetNum = 1;
		targets = new List<Creature> ();
		actionCost = 1;
	}

	public override void useAbility ()
	{

		targets [0].takeDmg (2);
		targets.Clear ();
	}

	public override bool verify ()
	{
		
			return true;
		
	}
}
