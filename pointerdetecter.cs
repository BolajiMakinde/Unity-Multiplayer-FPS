using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointerdetecter : MonoBehaviour {

	public PlayerProfile pscript;
	// Use this for initialization
	void Start () {
		
	}

	void OnMouseOver ()
	{
		pscript.overlay = true;
	}

	void OnMouseExit()
	{
		pscript.overlay = false;
	}
}
