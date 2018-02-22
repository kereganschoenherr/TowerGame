using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class multiAttack : Ability {

	public multiAttack(){
		targetNum = 3;
		targets = new List<Creature> ();
		actionCost = 1;
	}

	public override void useAbility ()
	{

		targets [0].takeDmg (1);
		targets [1].takeDmg (1);
		targets [2].takeDmg (1);
		targets.Clear ();
	}

	public override bool verify ()
	{
			return true;
		
	}
}
