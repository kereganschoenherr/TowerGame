using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Creature : MonoBehaviour, IComparable<Creature>{

	public float health;
	public float attackDmg;
	public bool isTurn;
	public float speed;

	int CompareTo(Creature c1, Creature c2){
		if (c1.speed > c2.speed) {
			return 1;
		} else if (c2.speed < c1.speed) {
			return -1;
		} else {
			return 0;
		}
	}


	void Start () {
		
	}
	

	void Update () {
		
	}

}
