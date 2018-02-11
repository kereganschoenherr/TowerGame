using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Floor currentFloor;
	public Room currentRoom;
	public float timer = 0;
	public List<Character> party;
	public List<Enemy> combatEnemies;
	public List<Creature> combatCreatures;
	public GameObject testPlayer;
	public Room camp;
	public CharacterDictionary cd;
	public EnemyDictionary ed;
	public bool gameOver;
	public int turn;
	public int targetSelection;
	public bool inCombat;
	public List<Vector3> enemyPositions;
	public List<Vector3> partyPositions;
	public int floorsClimbed;
	public int attackSelection;
	public GameObject target;
	public GameObject turnIndicator;

	void initialize(){
		//initialize lists and then use in start() to make start() easier to look at
		combatEnemies = new List<Enemy> ();
		combatCreatures = new List<Creature> ();

	}

	void Start () {
		
		attackSelection = 0;
		gameOver = false;
		floorsClimbed = 1;
		//create the camp
		camp = new Room ("Camp", true, 0);
		//firstLoad is the special method for first time room load
		firstLoad (camp);
		//initialize lists
		initialize ();
		turn = 0;
		//set starting floor of the whole run as the camp
		currentFloor = new Floor (camp);
		//initialize enemy and character encyclopedias
		cd = GetComponent<CharacterDictionary> ();
		ed = GetComponent<EnemyDictionary> ();
		//randomly instantiate 4 characters and put them into the starting party
		setupParty ();
	}
	

	void Update () {
		//the general game loop
		if (!gameOver) {
			//check if all party members are dead
			if (party.Count == 0) {
				Debug.Log ("Game Over.");
				gameOver = true;
			}
			//check to see if the room currently in is the camp
			if (currentRoom.isCamp ) {
				//pressing space bar exits the camp and sends player into the first room of a floor
				if (Input.GetKeyDown ("space")) {
					currentFloor.generateFloorPlan (camp, ed.easy);
					loadRoom (currentFloor.floorPlan [0]);
				}
			} else {
				// if the room isn't a camp, then you must be continuing through a floor
				if (Input.GetKeyDown ("space")) {
					loadRoom (currentRoom.outgoing [0]);
				}
				//generic combat check. If in combat, run general combat procedure until combat ends
				if (inCombat) {
					runCombat ();
				}
			}
		}
	}
	void setupParty(){
		//helper method to call in start() that randomly sets up a party of four to play test with
		for (int i = 0; i < 4; i++) {
			GameObject g = Instantiate (cd.characters [Random.Range(0,cd.characters.Count)]);
			g.transform.position = partyPositions [i];
			party.Add (g.GetComponent<Character>());
		}
	}

	void firstLoad(Room r){
		//using the "current room" variable, load up the first room
		currentRoom = r;
		Debug.Log (currentRoom.name);
		Debug.Log ("Current Floor: " + floorsClimbed);
	}

	void loadRoom(Room r){
		//code to clean up the old room can go here if there are bugs (leftover enemies in lists etc.)
		// but shouldn't be necessary if runCombat() is bug-free
		//should be left here so we can space-bar past combat

		for (int i = 0; i < combatEnemies.Count; i++) {
			Destroy(combatEnemies [i].gameObject);
		}
		combatCreatures.Clear ();
		combatEnemies.Clear ();




		//current room now gets updated
		currentRoom = r;

		//let gamemanager know if combat should be started in this room
		if (currentRoom.hasCombat) {
			inCombat = true;
		} else {
			inCombat = false;
		}

		//print name of room for debugging
		Debug.Log (currentRoom.name);
		//if you are getting to a camp, you can add 1 to the number of floors finished
		if(currentRoom.isCamp){
			floorsClimbed++;
			Debug.Log ("Current Floor: " + floorsClimbed);
		}

		//if a room has combat in it, load in and initialize the enemies that go with that room
		//this involves: creating their respective gameobject, putting them somewhere on screen according to the list of vector3s,
		// and add them to the combat enemies list
		if (currentRoom.hasCombat) {
			for (int i = 0; i < currentRoom.enemyNum; i++) {
				GameObject g = Instantiate (currentRoom.enemies[i]);
				g.transform.position = enemyPositions [i];
				combatEnemies.Add (g.GetComponent<Enemy> ());
			}
		}

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

	}

	void runCombat(){
		//this is the general combat loop that controls all combat scenarios

		//put the turn indicator over the correct creature
		turnIndicator.transform.position = new Vector3 (combatCreatures[turn].gameObject.transform.position.x, combatCreatures[turn].gameObject.transform.position.y + 2f, 2f);

		//change targetSelection with right and left arrow keys, the variable which controls aiming 

		if (combatCreatures [turn] is Character) {
			if(Input.GetKeyDown(KeyCode.RightArrow)){
				targetSelection++;
			}
			else if(Input.GetKeyDown(KeyCode.LeftArrow)){
				targetSelection--;
			}

			if(Input.GetKeyDown(KeyCode.C)){
				attackSelection++;
			}

			Character c = combatCreatures [turn] as Character;
		//also change what attack the character is going to use using the c key
			if (attackSelection >= c.moveSet.Count) {
				attackSelection = 0;
			}

			if (targetSelection < 0) {
				targetSelection = combatCreatures.Count - 1;
			}
			else if(targetSelection >= combatCreatures.Count){
				targetSelection = 0;
			}
		}
		//put the target indicator over the correct creature, this is the creature the player is aiming at
		target.transform.position = new Vector3 (combatCreatures[targetSelection].gameObject.transform.position.x, combatCreatures[targetSelection].gameObject.transform.position.y + 1f, 2f);


		// this boolean determines if an attack occurred
			bool attacked = false;

		//you can press either z or x while it is a party members turn
		if (combatCreatures [turn] is Character) {
			if (Input.GetKeyDown (KeyCode.Z)) {
				//executes a basic attack on any Creature (even friendly targets)
				//this is still here for debugging and testing
				combatCreatures [turn].attack (combatCreatures [targetSelection]);
				attacked = true;
				Debug.Log(combatCreatures[turn] + " attacks " + combatCreatures[targetSelection] + " for " + combatCreatures[turn].attackDmg + " damage!");

			} else if (Input.GetKeyDown (KeyCode.X)) {
				//executes a move from the character moveset depending
				// on what the attackSelection variable is which is scrolled through using the c key
				//in the future, all moves will be chosen through the moveSet
				Character c = combatCreatures [turn] as Character;
				c.moveSet[attackSelection] ();
				attacked = true;
				Debug.Log ("moveset attack!");
			}

		} else if (combatCreatures [turn] is Enemy) {
			// if it is the enemies turn, wait one second, then basic attack the 0th party member.
			//in the future, all enemies will also have a moveSet
			// and other targetSelection AI
			timer += Time.deltaTime;
			if (timer > 1f) {
				attacked = true;
				timer = 0;
				combatCreatures [turn].attack (party [0]);
				Debug.Log (combatCreatures [turn] + " attacks " + party [0] + " for " + combatCreatures [turn].attackDmg + " damage!");

			}
		}

		if (attacked) {
			// attacked will be true if someone did a move
			// this currently happens to always corresponds to a turn, perhaps it might not always in the future
			// in which case this for loop should be called according to a more general condition, but its fine for now

			//check to see if any Creature died. If they did clean them up from all the correct lists, destroy them, and pay your respects
			for (int i = 0; i < combatCreatures.Count; i++) {
				if (combatCreatures [i].health <= 0) {
					GameObject g = combatCreatures [i].gameObject;
					Debug.Log (combatCreatures [i] + " is defeated!");
					//HotFix for KILLSKIP bug
					if(i < turn){
						turn--;
					}
					if (combatCreatures [i] is Enemy) {
						combatEnemies.RemoveAt (combatEnemies.IndexOf (g.GetComponent<Enemy> ()));
					} else if (combatCreatures [i] is Character) {
						party.RemoveAt (party.IndexOf (g.GetComponent<Character> ()));
					}
					combatCreatures.RemoveAt (i);
					Destroy (g);
				}
			}

			//reset the target and attack move selections back to 0 after an attack
			targetSelection = 0;
			attackSelection = 0;

			//increment turn
			//! note ! if a person in combatCreatures kills someone lower in the list, it skips over the next person's turn
		
			turn++;
			if (turn >= combatCreatures.Count) {
				turn = 0;
			}

		}

		//if combat should end, let gamemanager know and send indicators back to the top left corner so its easy to tell
		if (combatEnemies.Count == 0 || party.Count == 0) {
			inCombat = false;
			target.transform.position = new Vector3 (-11.82f, 4.45f, 2f);
			turnIndicator.transform.position = new Vector3 (-11.04f, 4.45f, 2f);
			turn = 0;
		}
		
	}


}
