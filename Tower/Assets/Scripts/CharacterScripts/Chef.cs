using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chef : Character {

	void Start () {
		init ();
		moveSet.Add (new basicDmg());
		moveSet.Add (new killDmg());
		moveSet.Add (new meatMallet ());
        moveSet.Add (new Shield());

	}

	// Update is called once per frame
	void Update () {

	}

    public override void turnStart()
    {
    }

}
