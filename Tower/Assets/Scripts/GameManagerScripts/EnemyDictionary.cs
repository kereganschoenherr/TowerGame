using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDictionary : MonoBehaviour {

	public GameObject slime;
	public List<GameObject> easy = new List<GameObject>();

	void Start(){
		easy.Add (slime);
	}

}
