using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight: Character {

	// Use this for initialization
	void Start () {
		setHealth (healthModifier);


	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public override void specialAttack(){
		health += 5;
	}
}
