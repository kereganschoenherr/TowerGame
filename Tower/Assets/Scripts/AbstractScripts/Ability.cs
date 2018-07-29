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


    //use the two following functions in verify to make sure correct targets were selected for a given move
    public bool assertCharacterTarget(int i)
    {
        if (targets[i] is Character)
        {
            return true;
        }
        return false;
    }

    public bool assertEnemyTarget(int i)
    {
        if (targets[i] is Enemy)
        {
            return true;
        }

        return false;
    }

}
