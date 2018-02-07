using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature {

	public static float baseHealth = 100;
	public float healthModifier;

	public void setHealth(float healthMod){
		healthModifier = healthMod;
		health = healthMod * baseHealth;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
