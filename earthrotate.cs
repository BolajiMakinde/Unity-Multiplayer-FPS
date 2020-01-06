using UnityEngine;
using System.Collections;

public class earthrotate : MonoBehaviour {

	public Vector3 rotatespeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (rotatespeed.x*Time.deltaTime, rotatespeed.y*Time.deltaTime,rotatespeed.z * Time.deltaTime);
	}
}
