using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
	public GameObject[] txt;
	TextMeshProUGUI partyHp;
	public bool turnOver;
	public CombatStateMachine csm;


	void initialize(){
		//initialize lists and then use in start() to make start() easier to look at
		combatEnemies = new List<Enemy> ();
		combatCreatures = new List<Creature> ();
		csm = GetComponent<CombatStateMachine> ();

	}

	void Start () {
		csm = GetComponent<CombatStateMachine> ();
		turnOver = false;
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
	
	void Awake(){
		
			partyHp = txt[0].GetComponent<TextMeshProUGUI> ();



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
					csm.runCombat();
				}
			}
		}

		partyHp.text = party [0].health.ToString ();


	}
	void setupParty(){
		//helper method to call in start() that randomly sets up a party of four to play test with
		for (int i = 0; i < 4; i++) {
			GameObject g = Instantiate (cd.chef);
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
			csm.combatSetup = true;
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

	}




}
