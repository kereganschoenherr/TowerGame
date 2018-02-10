using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room{
	
	public List<Room> incoming;
	public List<Room> outgoing;
	public string name;
	public bool isCamp;
	public List<GameObject> enemyReferences;
	public List<GameObject> enemies;
	public bool hasCombat;
	public int enemyNum;


	public Room(string n, bool camp, int enemyNum){
		this.enemyNum = enemyNum;
		incoming = new List<Room> ();
		outgoing = new List<Room> ();
		enemyReferences = new List<GameObject> ();
		enemies = new List<GameObject> ();
		name = n;
		isCamp = camp;
		hasCombat = false;
	}

	public void addEnemies(List<GameObject> enem){
		enemyReferences = enem;
		hasCombat = true;
	}


		
}
