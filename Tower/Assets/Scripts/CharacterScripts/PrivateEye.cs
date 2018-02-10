using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrivateEye : Character {



	void Start () {
		moveSet = new List<Action> ();
		setHealth (healthModifier);
		moveSet.Add (() => move1 ());
		moveSet.Add (() => move2 ());
		Debug.Log (moveSet.Count);
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
