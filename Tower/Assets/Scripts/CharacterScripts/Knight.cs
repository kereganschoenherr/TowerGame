using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Knight: Character {

	void Start () {
		init ();
        moveSet.Add(new Shield());

	}

	// Update is called once per frame
	void Update () {

	}

    public override void turnStart()
    {
    }

}
