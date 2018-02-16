using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {



	// Use this for initialization
	void Start () {
		init ();
		moveSet.Add (() => move1 ());
	}
	
	// Update is called once per frame
	void Update () {
	}
	public void move1(){
		gm.combatCreatures [gm.turn].attack (gm.party [0]);
		gm.turnOver = true;
	}

}
