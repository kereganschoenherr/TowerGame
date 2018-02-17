using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chef : Character {



	void Start () {
		init ();
		moveSet.Add (new basicDmg());
		moveSet.Add (new killDmg());

	}

	// Update is called once per frame
	void Update () {

	}

	public void move1(){
		speed += 7;
	}

	public void move2(){
		health += 17;
	}

}
