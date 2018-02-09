using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : Creature {

	public static float baseHealth = 100;
	public float healthModifier = 1;

	public void setHealth(float healthMod){
		healthModifier = healthMod;
		health = healthMod * baseHealth;
	}


	public abstract void specialAttack ();




	void Start () {
		
	}
	

	void Update () {
		
	}
}
