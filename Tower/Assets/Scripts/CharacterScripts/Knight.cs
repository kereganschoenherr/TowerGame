using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Knight: Character {

	void Start () {
		init ();

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
