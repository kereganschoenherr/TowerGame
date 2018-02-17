using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Character : Creature {

	public static float baseHealth = 100;
	public float healthModifier;
	public List<Ability> moveSet;


	public void setHealth(float healthMod){
		healthModifier = healthMod;
		health = healthMod * baseHealth;

	}

	public void init(){
		moveSet = new List<Ability> ();
		setHealth (healthModifier);
		gm = GameObject.Find ("GameManager").GetComponent<GameManager>();
		actionPoints = 1;
		EventManager.enemyDeath += huzzah;
		alive = true;
	}


	public bool assertCharacterTarget(){
		if (gm.combatCreatures [gm.targetSelection] is Character) {
			return true;
		}
		return false;
	}

	public bool assertEnemyTarget(){
		if(gm.combatCreatures[gm.targetSelection] is Enemy){
			return true;
		}

		return false;
	}

	public void huzzah(){
		Debug.Log (this.gameObject + " says huzzah!");
		}




}
