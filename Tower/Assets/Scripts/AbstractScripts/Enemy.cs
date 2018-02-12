using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Enemy : Creature {

	public static float baseHealth = 100;
	public float healthModifier;
	public List<Action> moveSet;

	public void setHealth(float healthMod){
		healthModifier = healthMod;
		health = healthMod * baseHealth;
	}

	public void init(){
		setHealth (healthModifier);
		moveSet = new List<Action> ();
		gm = GameObject.Find ("GameManager").GetComponent<GameManager>();
	}

	public abstract int chooseAttack ();
	public abstract int chooseTarget ();
}
