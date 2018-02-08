using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : Character {



	void Start () {
		setHealth (1f);
		attackDmg = 34;
		speed = 30;

	}
	

	void Update () {
		if (health <= 0) {
			
		}
	}
}
