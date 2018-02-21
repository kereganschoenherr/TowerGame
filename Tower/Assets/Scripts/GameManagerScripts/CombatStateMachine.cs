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
	public int enemiesDead;
	public int charactersDead;

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

		enemiesDead = 0;
		charactersDead = 0;

	}

	// Welcome to the state machine that controls combat! In GameManager, when inCombat is true, runCombat will run.
	// The runCombat function emulates a state machine by a short control flow of else-if statements. There are 6 states
	// currently, and each state has its own varaiable to represent it in the else-if blocks.
	//NEVER should any two booleans in this set be true at the same time. It is up to the programmer to make sure that
	// the varaibles are handled correctly. This system could have been made more fool proof, but the game so far isn't really
	// complicated enough to require anything more sophisticated

	//Let's review the states

	// combatSetup // 

	/// <summary>
	/// The combatSetup state only runs through its code once
	/// and it is affected by no user input.
	/// This is where the lists that we were using before are first utilized: party, combatEnemies, and combatCreatures
	/// these lists get filled in (the code was stolen from the old loadRoom function) and combat can start
	/// a slew of other variables are reset here as well to prevent bugs after entering/leaving combat
	/// a few times. I threw these assignments in the combatSetupInit() function at the bottom of this script
	/// 
	/// Incoming states: None
	/// Outgoing states: ChooseTurn
	/// </summary>

	// chooseTurn // 

	/// <summary>
	/// This is where, if combat has just started, the first turn will be chosen. After that,
	/// it serves as a sort of "in between turns" state.
	/// This is where things are checked like creature deaths and combatOver status
	/// This state's code will run only once, unless it tries to give a turn to something that has died,
	/// then it will run again and try to give a turn to the next creature in line
	/// 
	/// you can see that this states is a bit of a hub by looking at the state connectivity below
	/// 
	/// Incoming states: enemyTurn, chooseTargets, chooseAbility(only if turn is skipped with 's')
	/// Outgoing states: chooseAbility, enemyTurn, combatOver
	/// </summary>

	// Choose Ability //
	/// <summary>
	/// Using left/right arrow keys, the player can move through the ability list of a character
	/// Clicking enter will select an ability, if the character has enough action points to use it
	/// if a player wants to skip their turn, they can press 's', this input is mostly there for debugging
	/// we can keep track of if a player should actually be able to skip later
	/// 
	/// Incoming States: chooseTurn
	/// Outgoing States: chooseAbilityTargets, chooseTurn
	/// </summary>

	// Choose Ability Targets //
	/// <summary>
	/// In this state the player can dynamically choose targets based on the ability they chose in chooseAbility
	/// using left/right arrow keys and selecting with enter
	/// These two states have a strong connection because this state needs to know about the ability just previously
	/// chosen to work properly
	/// Here is also where it will be checked if valid targets were chosen for the currently selected ability
	/// this state is where a given ability will actually be "fired"
	/// chooseAbility might be the new state if an ability is used and the player still has unspent action points
	/// 
	/// Incoming States: ChooseAbility 
	/// Outgoing States: ChooseTurn, ChooseAbility
	/// </summary>

	// Enemy Turn // 
	/// <summary>
	/// Nefarious enemy gets to make their move in this state
	/// no ai currently implemented, they just beat on the 0th party member, even if they're dead
	/// this state just sends us back to choose Turn
	/// 
	/// Incoming States: chooseTurn
	/// Outgoing States: chooseTurn
	/// </summary>

	// combat over //

	/// <summary>
	/// Somewhere in choose turn it was decided that combat should be over so here we are
	/// clear out enemies and combat creatures and get rid of all that junk we'll never need again
	/// let gamemanager know that combat's over so stop running combat in GameManager.Update()
	/// 
	/// incoming states: chooseTurn
	/// outgoing states: none
	/// </summary>


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

		//set up the combat variables for a new round of combat
		combatSetupInit();

		//access GameManager and load in the room's enemies
		for (int i = 0; i < gm.currentRoom.enemyNum; i++) {
			GameObject g = Instantiate (gm.currentRoom.enemies[i]);
			g.transform.position = gm.enemyPositions [i];
			combatEnemies.Add (g.GetComponent<Enemy> ());
		}

		//load in the party from GameManager
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
		Debug.Log ("choose turn");
		combatSetup = false;
		chooseTurn = true;
	}

	public void chooseturn(){
		
		//chooseTurn: hub for in between turn stuff, and its also the first state entered after combatSetup

		//check for people dying!
		for (int i = 0; i < combatCreatures.Count; i++) {
			if (combatCreatures [i].health <= 0 && combatCreatures [i].alive) {

				combatCreatures [i].alive = false;
				combatCreatures [i].gameObject.GetComponent<SpriteRenderer> ().enabled = false;
				Debug.Log (combatCreatures [i] + " is defeated!");

				if (combatCreatures [i] is Enemy) {
					EventManager.onEnemyDeath ();

					enemiesDead++;

				} else if (combatCreatures [i] is Character) {
					//EventManager.onCharacterDeath ();
					// put !null check around events with no subscribers or get an error
					charactersDead ++;
				}

				//EventManager.onCreatureDeath ();
				// put !null check around events with no subscribers or get an error
			}
		}

		//check for the fight being over
		//we don't have game over yet
		if (combatEnemies.Count == enemiesDead || party.Count == charactersDead) {
			
			gm.target.transform.position = new Vector3 (-11.82f, 4.45f, 2f);
			gm.turnIndicator.transform.position = new Vector3 (-11.04f, 4.45f, 2f);
			turn = 0;
			Debug.Log ("combat over");
			chooseTurn = false;
			combatOver = true;
			return;
		}

		//the all important 'turn' variable. look at it. so simple. one incremetation. yet so pivotal.
		turn++;
		if (turn >= combatCreatures.Count) {
			turn = 0;
		}

		//reset creatures, i am not sure if this NEEDS to be here, but i was ascared of bugs
		activeEnemy = null;
		activeCharacter = null;

		//if an enemy comes next, go the the enemyTurn state, where the enemy will do something evil probably
		if (combatCreatures [turn] is Enemy) {
			activeEnemy = combatCreatures [turn] as Enemy;
			if (activeEnemy.alive) {
				gm.turnIndicator.transform.position = new Vector3 (activeEnemy.gameObject.transform.position.x, activeEnemy.gameObject.transform.position.y + 2f, 2f);
				Debug.Log ("enemy turn");
				chooseTurn = false;
				enemyTurn = true;
			}
		} else {
			//or if a character comes next, go to chooseAbility state, with currentActionPoints being updated
			activeCharacter = combatCreatures [turn] as Character;
			if (activeCharacter.alive) {
				gm.turnIndicator.transform.position = new Vector3 (activeCharacter.gameObject.transform.position.x, activeCharacter.gameObject.transform.position.y + 2f, 2f);
				Debug.Log ("choose ability");
				currentActionPoints = activeCharacter.actionPoints;
				chooseTurn = false;
				chooseAbility = true;
			}
		}

	}
	public void chooseability(){
		
		// this state takes left/right as inputs to change ability selectio
		//use 's' key to skip for now but I haven't tested it
		//enter selects the current ability IF you have the appropriate amount of action points left
		targetSelection = 0;
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
			Debug.Log ("choose turn");
			chooseAbility = false;
			chooseTurn = true;

		}
		else if (Input.GetKeyDown (KeyCode.Return)) {
			if (activeCharacter.moveSet [moveSelection].actionCost <= currentActionPoints) {
				chosenAbility = activeCharacter.moveSet [moveSelection];
				Debug.Log ("choose targets");
				chooseAbility = false;
				chooseAbilityTargets = true;
			}
		}
	}
	public void chooseabilitytargets(){
		//here you can choose some targets and then run the chosen ability
		//this state takes left/right input to choose targets, enter to select them
		//upon selecting the last target, the ability fires, IF it passes the Ability.verify() call, which should be custom for every ability
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			do {
				
				targetSelection++;
				if (targetSelection >= combatCreatures.Count) {
					targetSelection = 0;
				}
			} while(!combatCreatures [targetSelection].alive);
		}
		
		else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			do {
				
				targetSelection--;
				if (targetSelection < 0) {
					targetSelection = combatCreatures.Count -1;
				}
			} while(!combatCreatures [targetSelection].alive);

		}
		/*
		if (targetSelection < 0) {
			targetSelection = combatCreatures.Count - 1;
		}
		else if(targetSelection >= combatCreatures.Count){
			targetSelection = 0;
		}
*/
		gm.target.transform.position = new Vector3 (combatCreatures[targetSelection].gameObject.transform.position.x, combatCreatures[targetSelection].gameObject.transform.position.y + 1f, 2f);

		if (targetsAdded < chosenAbility.targetNum) {
			if (Input.GetKeyDown (KeyCode.Return)) {
				targetsAdded += 1;
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
					Debug.Log ("choose another ability");
					targetsAdded = 0;
					gm.target.transform.position = new Vector3 (-11.82f, 4.45f, 2f);
					chooseAbilityTargets = false;
					chooseAbility = true;
				} else {
					Debug.Log ("choose turn");
					targetsAdded = 0;
					gm.target.transform.position = new Vector3 (-11.82f, 4.45f, 2f);
					chooseAbilityTargets = false;
					chooseTurn = true;

				}


			}

		}

	}
	public void enemyturn(){
		//run some AI so enemy does something, then go back to choose turn
		timer += Time.deltaTime;
		if (timer > 1f) {

			timer = 0;

			party [0].takeDmg (5);

			Debug.Log (activeEnemy +  " attacks " + party [0] + " for " + combatCreatures [turn].attackDmg + " damage!");
			Debug.Log ("choose turn");
			enemyTurn = false;
			chooseTurn = true;

		}
	}
	public void combatover(){
		//clean everything up, combat's ended, move along, nothing to see here etc. etc.
		for (int i = 0; i < combatEnemies.Count; i++) {
			Destroy(combatEnemies [i].gameObject);
		}

		for(int i = 0; i < party.Count; i++){
			if (!party [i].alive) {
				GameObject g = party [i].gameObject;
				party.RemoveAt (i);
				Destroy (g);
			}
		}
		combatCreatures.Clear ();
		combatEnemies.Clear ();
		Debug.Log ("exiting");

		gm.inCombat = false;
		combatOver = false;

	}


	void combatSetupInit(){
		//here are some important variables getting intialized
		enemiesDead = 0;
		charactersDead = 0;

		moveSelection = 0;
		activeCharacter = null;
		activeEnemy = null;
		targetsAdded = 0;
		targetSelection = 0;
		turn = -1;
	}

}