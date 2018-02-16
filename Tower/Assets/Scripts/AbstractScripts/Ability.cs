using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Ability{

	public int hungerCost;
	public int actionCost;
	public int targetNum;
	public List<Creature> targets;


	public abstract void useAbility();
	public abstract bool verify();

}
