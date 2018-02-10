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
			
			Room r = new Room ("Room " + i.ToString (), false, Random.Range(0,3));

			// if a room is meant to have enemies in it, give them a reference of enemies to choose from
			if (r.enemyNum > 0) {
				r.addEnemies (enemies);
			}
			// now that enemiesReferences have been added to the room, randomly decide on the actual list of enemies that will be instantiated once the room is entered.
			//randomly add references into the Room.enemies until you hit the enemyNum limit
		for(int j = 0; j < r.enemyNum; j++){
				r.enemies.Add (r.enemyReferences [Random.Range (0, r.enemyReferences.Count)]);
			}



			//once the room r is done being constructed. add it to the floorPlan

			floorPlan.Add (r);


		}
		//Create connections between rooms 
		for (int i = 0; i < roomNum; i++) {
			if (i < roomNum - 1) {
				floorPlan [i].outgoing.Add (floorPlan [i + 1]);
			} else {
				floorPlan [i].outgoing.Add (camp);
			}
		}

	}
}
