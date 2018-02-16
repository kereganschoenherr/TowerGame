using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStateMachine : MonoBehaviour {
	public GameManager gm;

	//list of states
	//these bools are carefully handled to model a state machine, with each block of the if/else chain representing a state
	//the if/else blocks have code that link the states in a meaningful way
	//now each 'state' will react to keyboard input differently and can only transition to certain other states
	//the game so far is not complicated enough to warrent more robust or object-oriented state management
	public bool combatSetup;
	public bool chooseTurn;
	public bool chooseAbility;
	public bool chooseAbilityTargets;
	public bool enemyTurn;
	public bool combatOver;

	public List<Enemy> combatEnemies;
	public List<Character> party;
	public List<Creature> combatCreatures;
	public Character activeCharacter;
	public Enemy activeEnemy;
	public Ability chosenAbility;

	public int moveSelection;
	public int targetsAdded;
	public int targetSelection;
	public int currentActionPoints;
	public int turn;
	public float timer;

	void Start(){
		gm = GetComponent<GameManager> ();
		combatSetup = true;
		chooseTurn = false;
		chooseAbility = false;
		chooseAbilityTargets = false;
		enemyTurn = false;
		combatOver = false;

		combatEnemies = new List<Enemy> ();
		party = new List<Character> ();
		combatCreatures = new List<Creature> ();
		moveSelection = 0;
		activeCharacter = null;
		activeEnemy = null;
		targetsAdded = 0;
		targetSelection = 0;
		turn = -1;
		timer = 0;

	}

	public void runCombat(){
		if (combatSetup) {
			combatsetup ();
		}else if(chooseTurn){
			chooseturn ();
		}else if (chooseAbility) {
			chooseability ();
		} else if (chooseAbilityTargets) {
			chooseabilitytargets ();
		} else if (enemyTurn) {
			enemyturn ();
		} else if (combatOver) {
			combatover ();
		} 
		else{
			Debug.Log("idling");
		}
	}


	public void combatsetup(){
		//if a room has combat in it, load in and initialize the enemies that go with that room
		//this involves: creating their respective gameobject, putting them somewhere on screen according to the list of vector3s,
		// and add them to the combat enemies list
		for (int i = 0; i < gm.currentRoom.enemyNum; i++) {
			GameObject g = Instantiate (gm.currentRoom.enemies[i]);
			g.transform.position = gm.enemyPositions [i];
			combatEnemies.Add (g.GetComponent<Enemy> ());
		}

		party = gm.party;

		//now add everybody who will be participating in glorious combat to the combatCreatures list!
		for(int i = 0; i < combatEnemies.Count; i++){
			combatCreatures.Add (combatEnemies[i]);
		}

		for(int i = 0; i < party.Count; i++){
			combatCreatures.Add (party[i]);
		}

		//sort the everybody by speed stat, the primitive metric we currently use to control turns.
		//The sorting implementation can be found in Creature class
		combatCreatures.Sort ();
		combatSetup = false;
		chooseTurn = true;
	}

	public void chooseturn(){
		for (int i = 0; i < combatCreatures.Count; i++) {
			if (combatCreatures [i].health <= 0) {

				combatCreatures [i].alive = false;

				Debug.Log (combatCreatures [i] + " is defeated!");

				if (combatCreatures [i] is Enemy) {
					EventManager.onEnemyDeath ();

				} else if (combatCreatures [i] is Character) {
					EventManager.onCharacterDeath ();
				}

				EventManager.onCreatureDeath ();
			}
		}

		if (combatEnemies.Count == 0 || party.Count == 0) {
			gm.inCombat = false;
			gm.target.transform.position = new Vector3 (-11.82f, 4.45f, 2f);
			gm.turnIndicator.transform.position = new Vector3 (-11.04f, 4.45f, 2f);
			turn = 0;
			chooseTurn = false;
			combatOver = true;
			return;
		}

		turn++;
		if (turn >= combatCreatures.Count) {
			turn = 0;
		}

		activeEnemy = null;
		activeCharacter = null;
		if (combatCreatures [turn] is Enemy) {
			activeEnemy = combatCreatures [turn] as Enemy;
			if (activeEnemy.alive) {
				gm.turnIndicator.transform.position = new Vector3 (activeEnemy.gameObject.transform.position.x, activeEnemy.gameObject.transform.position.y + 2f, 2f);
				chooseTurn = false;
				enemyTurn = true;
			}
		} else {
			activeCharacter = combatCreatures [turn] as Character;
			if (activeCharacter.alive) {
				gm.turnIndicator.transform.position = new Vector3 (activeCharacter.gameObject.transform.position.x, activeCharacter.gameObject.transform.position.y + 2f, 2f);
				chooseTurn = false;
				chooseAbility = true;
			}
		}

	}
	public void chooseability(){

		if(Input.GetKeyDown(KeyCode.RightArrow)){
			moveSelection++;
		} 
		else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			moveSelection--;
		}
		if (moveSelection < 0) {
			moveSelection = activeCharacter.moveSet.Count - 1;
		}
		else if(moveSelection >= activeCharacter.moveSet.Count){
			moveSelection = 0;
		}

		if(Input.GetKeyDown(KeyCode.S)){
			chooseAbility = false;
			chooseTurn = true;

		}
		else if (Input.GetKeyDown (KeyCode.Return)) {
			if (activeCharacter.moveSet [moveSelection].actionCost <= currentActionPoints) {
				chosenAbility = activeCharacter.moveSet [moveSelection];
				chooseAbility = false;
				chooseAbilityTargets = true;
			}
		}
		
	}
	public void chooseabilitytargets(){

		if(Input.GetKeyDown(KeyCode.RightArrow)){
			targetSelection++;
		} 
		else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			targetSelection--;
		}
		if (targetSelection < 0) {
			targetSelection = combatCreatures.Count - 1;
		}
		else if(targetSelection >= combatCreatures.Count){
			targetSelection = 0;
		}


		if (targetsAdded < chosenAbility.targetNum) {
			if (Input.GetKeyDown (KeyCode.Return)) {
				chosenAbility.targets.Add (combatCreatures [targetSelection]);
			}
		} else {
			if (chosenAbility.verify ()) {
				chosenAbility.useAbility ();
				currentActionPoints -= chosenAbility.actionCost;
				int min = 100;
				for(int i = 0; i < activeCharacter.moveSet.Count; i++){
					if (min > activeCharacter.moveSet [i].actionCost) {
						min = activeCharacter.moveSet [i].actionCost;
					}
				}

				if (min <= currentActionPoints) {
					chooseAbilityTargets = false;
					chooseAbility = true;
				} else {
					chooseAbilityTargets = false;
					chooseTurn = true;
				}


			}
		}
	}
	public void enemyturn(){
		timer += Time.deltaTime;
		if (timer > 1f) {

			timer = 0;

			activeEnemy.moveSet [0] ();

			Debug.Log (combatCreatures [turn] + " attacks " + party [0] + " for " + combatCreatures [turn].attackDmg + " damage!");

			enemyTurn = false;
			chooseTurn = true;

		}
	}
	public void combatover(){
		for (int i = 0; i < combatEnemies.Count; i++) {
			Destroy(combatEnemies [i].gameObject);
		}
		combatCreatures.Clear ();
		combatEnemies.Clear ();
		gm.inCombat = false;
		combatOver = false;

	}
}

