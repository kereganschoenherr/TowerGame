using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class State{
	public GameManager gm;
	abstract public void run();

}
