using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Floor currentFloor;
	public Room currentRoom;
	public float timer = 0;
	public Character player;
	public Room camp;
	public CharacterDictionary cd;
	public EnemyDictionary ed;

	// Use this for initialization
	void Start () {
		camp = new Room ("Camp", true);
		firstLoad (camp);

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
			timer += Time.deltaTime;

			if (timer > 2) {
				timer = 0;
				loadRoom (currentRoom.outgoing [0]);

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
		}

		//current room is now updated
		currentRoom = r;
		Debug.Log (currentRoom.name);
		for (int i = 0; i < r.enemyReferences.Count; i++) {
			GameObject g = Instantiate (currentRoom.enemyReferences [i]);
			currentRoom.enemies.Add (g);

		}
	}
}
