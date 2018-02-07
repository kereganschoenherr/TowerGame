using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Floor currentFloor;
	public Room currentRoom;
	public float timer = 0;
	public Character player;

	// Use this for initialization
	void Start () {
		Floor testFloor = new Floor ();
		Room r1 = new Room ("r1");
		Room r2 = new Room ("r2");
		Room r3 = new Room ("r3");
		testFloor.floorPlan.Add (r1);
		testFloor.floorPlan.Add (r2);
		testFloor.floorPlan.Add (r3);
		currentRoom = r1;
		r1.outgoing.Add (r2);
		r2.outgoing.Add (r3);
		r3.outgoing.Add (r1);

	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer > 5) {
			timer = 0;
			loadRoom (currentRoom.outgoing [0]);
			Debug.Log (currentRoom.name);
		}
	}

	void loadRoom(Room r){
		currentRoom = r;
	}
}
