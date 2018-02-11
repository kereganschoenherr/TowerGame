using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Doctor : Character {



	void Start () {
		init ();
		moveSet.Add (() => move1 ());
		moveSet.Add (() => move2 ());

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
