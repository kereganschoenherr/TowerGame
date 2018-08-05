using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class Creature : MonoBehaviour, IComparable<Creature>{

	public float health;
	public float attackDmg;
	public GameManager gm;
	public float speed;
	public int actionPoints;
	public bool alive;
    public List<Effect> effectList;


	int IComparable<Creature>.CompareTo(Creature c1){
		
		if (this.speed > c1.speed) {
			
			return -1;
		} else if (this.speed < c1.speed) {
			
			return 1;
		} else {
			return 0;
		}
	}

	public void attack(Creature c){
		c.takeDmg (attackDmg);
	}

	public virtual void takeDmg(float dmg){
        //health -= dmg;
        //SO FAR ONLY EFFECTS THAT ARE DAMAGE MODIFIERS HAVE BEEN INCLUDED
        //TODO This part here is for effects that take place before dmg calculations

        float temp = dmg;
        for (int i = 0; i < effectList.Count; i++) {
            if (effectList[i] is IOnTakeDmg) {
                IOnTakeDmg effect = effectList[i] as IOnTakeDmg;
                temp *= effect.getModifier();
            }
        }
        health -= temp;

        //TODO this part here is for effects that take place after dmg calculations


        for (int i = 0; i < effectList.Count; i++) {
            if (effectList[i].removeMe) {
                effectList.RemoveAt(i);
            }
        }
	}

    public abstract void turnStart();

    
		
}
