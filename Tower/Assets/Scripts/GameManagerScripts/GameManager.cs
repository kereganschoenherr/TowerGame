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
	public bool inCombat;

	void initialize(){
		//party = new List<Character> ();
		combatEnemies = new List<Enemy> ();
		combatCreatures = new List<Creature> ();

	}
	// Use this for initialization
	void Start () {
		gameOver = false;
		camp = new Room ("Camp", true, 0);
		firstLoad (camp);
		initialize ();
		turn = 0;

		//party.Add (testPlayer.GetComponent<Chef>());




		currentFloor = new Floor (camp);

		cd = GetComponent<CharacterDictionary> ();
		ed = GetComponent<EnemyDictionary> ();
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
	void firstLoad(Room r){
		currentRoom = r;
		Debug.Log (currentRoom.name);
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

		if (currentRoom.hasCombat) {
			for (int i = 0; i < currentRoom.enemyNum; i++) {
				GameObject g = Instantiate (currentRoom.enemies[i]);
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

		if (Input.GetKeyDown (KeyCode.Return)) {

			Debug.Log ("combat creatures : " + combatCreatures.Count + "| turn#: " + turn);
			if (combatCreatures [turn] is Character) {
				combatCreatures [turn].attack (combatEnemies [0]);
				if (combatEnemies [0].health <= 0) {
					GameObject g = combatEnemies[0].gameObject;
					combatCreatures.RemoveAt (0);
					combatEnemies.RemoveAt (0);

					Destroy (g);
				}
			} else if (combatCreatures [turn] is Enemy) {
				combatCreatures [turn].attack (party [0]);
				if (party [0].health <= 0) {
					GameObject g = party [0].gameObject;
					combatCreatures.RemoveAt (0);
					party.RemoveAt (0);

					Destroy (g);
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
