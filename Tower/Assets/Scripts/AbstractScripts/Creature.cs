﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class Creature : MonoBehaviour, IComparable<Creature>{

	public float health;
	public float attackDmg;

	public float speed;

	int IComparable<Creature>.CompareTo(Creature c1){
		
		if (this.speed > c1.speed) {
			
			return -1;
		} else if (this.speed < c1.speed) {
			
			return 1;
		} else {
			return 0;
		}
	}

	public void attack(Creature c){
		c.takeDmg (attackDmg);
	}

	public void takeDmg(float dmg){
		health -= dmg;
	}

	void Start () {
		
	}
	

	void Update () {
		
	}

}
