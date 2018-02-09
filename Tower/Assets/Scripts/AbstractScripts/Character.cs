using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Creature {

	public static float baseHealth = 100;
	public float healthModifier = 1;

	public void setHealth(float healthMod){
		healthModifier = healthMod;
		health = healthMod * baseHealth;
	}




	void Start () {
		
	}
	

	void Update () {
		
	}
}
