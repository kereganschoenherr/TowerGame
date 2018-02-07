using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor{

	public List<Room> floorPlan;

	public Floor(Room camp){
		floorPlan = new List<Room> ();

	}
	public void generateFloorPlan(Room camp, List<GameObject> enemies){
		floorPlan.Clear ();
		int roomNum = Random.Range (3, 8);
		Debug.Log ("Number of Rooms: " + roomNum.ToString());
		//Create rooms
		for (int i = 0; i < roomNum; i++) {
			Room r = new Room ("Room " + i.ToString (), false);
			floorPlan.Add (r);
		}
		//Create connections
		for (int i = 0; i < roomNum; i++) {
			if (i < roomNum - 1) {
				floorPlan [i].outgoing.Add (floorPlan [i + 1]);
			} else {
				floorPlan [i].outgoing.Add (camp);
			}
		}

		floorPlan [roomNum / 2].addEnemies(enemies);
	}
}
