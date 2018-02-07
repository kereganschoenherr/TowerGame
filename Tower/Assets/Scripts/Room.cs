using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room{
	
	public List<Room> incoming;
	public List<Room> outgoing;
	public string name;

	public Room(string n){
		incoming = new List<Room> ();
		outgoing = new List<Room> ();
		name = n;
	}
		
}
