using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killDmg : Ability {

	public killDmg(){
		targetNum = 1;
		targets = new List<Creature> ();
		actionCost = 1;
	}

	public override void useAbility ()
	{
		targets [0].takeDmg (1900);
		targets.Clear ();
	}

	public override bool verify ()
	{
		return true;
	}
}
