using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
	//pattern for making externally triggerable events
	public delegate void ed (); //define a delegate, give it some trivial name
	public static event ed enemyDeath; //reference this for subscrition e.g. EventManager.enemyDeath += someMethod; 
	public static void onEnemyDeath(){ // call this when appropriate in GameManager
		enemyDeath ();
	}

	public delegate void cd ();
	public static event cd characterDeath;
	public static void onCharacterDeath(){
		characterDeath ();
	}

	public delegate void crd();
	public static event crd creatureDeath;
	public static void onCreatureDeath(){
		creatureDeath ();
	}

	public delegate void enemdmg();
	public static event enemdmg enemyTakeDmg;
	public static void onEnemyTakeDmg(){
		onEnemyTakeDmg();
	}


	public delegate void chardmg();
	public static event chardmg characterTakeDmg;
	public static void onCharacterTakeDmg(){
		onCharacterTakeDmg();
	}

	public delegate void creatDmg();
	public static event creatDmg creatureTakeDmg;
	public static void onCreatureTakeDmg(){
		onCreatureTakeDmg();
	}



}
