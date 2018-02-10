using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Character : Creature {

	public static float baseHealth = 100;
	public float healthModifier = 1;
	public List<Action> moveSet;

	public void setHealth(float healthMod){
		healthModifier = healthMod;
		health = healthMod * baseHealth;
	}



}
