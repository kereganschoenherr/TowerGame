using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy {



	// Use this for initialization
	void Start () {
		health = baseHealth * healthModifier;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
