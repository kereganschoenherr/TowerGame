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

	void initialize(){
		party = new List<Character> ();
		combatEnemies = new List<Enemy> ();
		combatCreatures = new List<Creature> ();

	}
	// Use this for initialization
	void Start () {
		gameOver = false;
		camp = new Room ("Camp", true);
		firstLoad (camp);
		initialize ();

		party.Add (testPlayer.GetComponent<Chef>());



		currentFloor = new Floor (camp);

		cd = GetComponent<CharacterDictionary> ();
		ed = GetComponent<EnemyDictionary> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (currentRoom.isCamp) {
			if (Input.GetKeyDown ("space")) {
				currentFloor.generateFloorPlan (camp, ed.easy);
				loadRoom (currentFloor.floorPlan[0]);
			}
		} else {
			if (Input.GetKeyDown ("space")) {
				loadRoom (currentRoom.outgoing [0]);
			}

			if(currentRoom.hasCombat){
				runCombat ();
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
			for (int i = 0; i < currentRoom.enemyReferences.Count; i++) {
				Destroy (currentRoom.enemies [i]);
			}
			//Debug.Log ("size after clear " + combatCreatures.Count);
		}
		combatCreatures.Clear ();
		combatEnemies.Clear ();
		//current room is now updated
		currentRoom = r;
		Debug.Log (currentRoom.name);
		for (int i = 0; i < r.enemyReferences.Count; i++) {
			GameObject g = Instantiate (currentRoom.enemyReferences [i]);
			currentRoom.enemies.Add (g);
			combatEnemies.Add(g.GetComponent<Enemy> ());
		}
		for(int i = 0; i < combatEnemies.Count; i++){
			combatCreatures.Add (combatEnemies[i]);
		}

		for(int i = 0; i < party.Count; i++){
			combatCreatures.Add (party[i]);
		}

		combatCreatures.Sort ();
		//Debug.Log ("size " + combatCreatures.Count);

	}

	void runCombat(){
		
	}


}
