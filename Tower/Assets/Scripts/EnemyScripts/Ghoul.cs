using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : Enemy {

	// Use this for initialization
	void Start () {
		setHealth (1f);
		attackDmg = 10;
		speed = 35;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
