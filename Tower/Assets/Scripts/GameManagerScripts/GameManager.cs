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

	void initialize(){
		//party = new List<Character> ();
		combatEnemies = new List<Enemy> ();
		combatCreatures = new List<Creature> ();

	}
	// Use this for initialization
	void Start () {
		gameOver = false;
		floorsClimbed = 1;
		camp = new Room ("Camp", true, 0);
		firstLoad (camp);
		initialize ();
		turn = 0;


		//party.Add (testPlayer.GetComponent<Chef>());




		currentFloor = new Floor (camp);

		cd = GetComponent<CharacterDictionary> ();
		ed = GetComponent<EnemyDictionary> ();
		setupParty ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameOver) {
			if (party.Count == 0) {
				Debug.Log ("Game Over.");
				gameOver = true;
			}
			if (currentRoom.isCamp ) {
				if (Input.GetKeyDown ("space")) {
					currentFloor.generateFloorPlan (camp, ed.easy);
					loadRoom (currentFloor.floorPlan [0]);
				}
			} else {
				if (Input.GetKeyDown ("space")) {
					loadRoom (currentRoom.outgoing [0]);
				}

				if (inCombat) {
					runCombat ();
				}
			}
		}
	}
	void setupParty(){
		for (int i = 0; i < 4; i++) {
			GameObject g = Instantiate (cd.characters [Random.Range(0,cd.characters.Count)]);
			g.transform.position = partyPositions [i];

			party.Add (g.GetComponent<Character>());
		}
	}
	void firstLoad(Room r){
		currentRoom = r;
		Debug.Log (currentRoom.name);
		Debug.Log ("Current Floor: " + floorsClimbed);
	}

	void loadRoom(Room r){
		//deal with old room
		if (!currentRoom.isCamp) {
			for (int i = 0; i < combatEnemies.Count; i++) {
				Destroy (combatEnemies [i].gameObject);
			}

		} 

		combatCreatures.Clear ();
		combatEnemies.Clear ();
		//current room is now updated

		currentRoom = r;

		if (currentRoom.hasCombat) {
			inCombat = true;
		} else {
			inCombat = false;
		}

		Debug.Log (currentRoom.name);
		if(currentRoom.isCamp){
			floorsClimbed++;
			Debug.Log ("Current Floor: " + floorsClimbed);
		}

		if (currentRoom.hasCombat) {
			for (int i = 0; i < currentRoom.enemyNum; i++) {
				GameObject g = Instantiate (currentRoom.enemies[i]);
				g.transform.position = enemyPositions [i];
				combatEnemies.Add (g.GetComponent<Enemy> ());
			}
		}
		for(int i = 0; i < combatEnemies.Count; i++){
			combatCreatures.Add (combatEnemies[i]);
		}

		for(int i = 0; i < party.Count; i++){
			combatCreatures.Add (party[i]);
		}

		combatCreatures.Sort ();

	}

	void runCombat(){
		if (combatCreatures [turn] is Character) {
			if(Input.GetKeyDown(KeyCode.RightArrow)){
				targetSelection++;
			}
			else if(Input.GetKeyDown(KeyCode.LeftArrow)){
				targetSelection--;
			}
			if (targetSelection < 0) {
				targetSelection = combatEnemies.Count - 1;
			}
			else if(targetSelection >= combatEnemies.Count){
				targetSelection = 0;
			}
		}
		if (Input.GetKeyDown (KeyCode.Z)) {

			//Debug.Log ("combat creatures : " + combatCreatures.Count + "| turn#: " + turn);
			if (combatCreatures [turn] is Character) {
				combatCreatures [turn].attack (combatEnemies [targetSelection]);
				Debug.Log (combatCreatures [turn] + " attacks " + combatEnemies [targetSelection] + " for " + combatCreatures [turn].attackDmg + " damage!");
				if (combatEnemies [targetSelection].health <= 0) {
					Debug.Log (combatEnemies [targetSelection] + " is defeated!");
					GameObject g = combatEnemies[targetSelection].gameObject;

					combatCreatures.RemoveAt (combatCreatures.IndexOf(g.GetComponent<Creature>()));
					combatEnemies.RemoveAt (combatEnemies.IndexOf(g.GetComponent<Enemy>()));

					Destroy (g);
				}
			} else if (combatCreatures [turn] is Enemy) {
				combatCreatures [turn].attack (party [0]);
				Debug.Log (combatCreatures [turn] + " attacks " + party [0] + " for " + combatCreatures [turn].attackDmg + " damage!");
				if (party [0].health <= 0) {
					Debug.Log (party [0] + " is defeated! : (");
					GameObject p = party [0].gameObject;
					combatCreatures.RemoveAt (combatCreatures.IndexOf(p.GetComponent<Creature>()));
					party.RemoveAt (party.IndexOf(p.GetComponent<Character>()));

					Destroy (p);
				}
			}

			turn++;
			if (turn >= combatCreatures.Count) {
				turn = 0;
			}
			if (combatEnemies.Count == 0 || party.Count == 0) {
				inCombat = false;
				turn = 0;
			}

		}
	}


}
