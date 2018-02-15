using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicDamage : SingleTarget {

	public float damage;
	public string name;

	public basicDamage(float dmg, string n){
		damage = dmg;
		name = n;
	}

	public override void useAbility(){
		target.takeDmg (damage);
	}

}
