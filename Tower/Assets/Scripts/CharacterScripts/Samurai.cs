using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Samurai : Character {

    public bool dodge;

	void Start () {
		init ();
        dodge = false;
        moveSet.Add(new dodge(this));
        moveSet.Add(new killingBlow());
        moveSet.Add(new multiAttack());
        moveSet.Add(new trueStrike());
        

	}

	// Update is called once per frame
	void Update () {

	}

	

    public override void takeDmg(float dmg) {
        if (dodge){
            health -= dmg * .2f;
            dodge = false;
        }
        else{
            health -= dmg;
        }

        
    }

    public override void turnStart() {
        dodge = false;
    }
    


}
