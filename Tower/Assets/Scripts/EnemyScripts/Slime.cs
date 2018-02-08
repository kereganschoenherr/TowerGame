using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {



	// Use this for initialization
	void Start () {
		setHealth (1f);
		attackDmg = 10;
		speed = 20;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			
		}
	}
}
