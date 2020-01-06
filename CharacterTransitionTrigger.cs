using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using RootMotion.FinalIK;
using UnityStandardAssets.Characters.FirstPerson;

public class CharacterTransitionTrigger : MonoBehaviour {

	public enum PositionStatus {
		prone2crouch,
		crouch2prone,
		crouch2idle
	}
	public List<int> ignorelayers;

	public PositionStatus mystatus;

	public bool istrue;
	// Use this for initialization
	void Start () {
		
	}
//	void FixedUpdate ()
//	{
//		if (mystatus == PositionStatus.crouch2prone) {
//			istrue = true;
//		}
//		else if (mystatus == PositionStatus.prone2crouch) {
//			istrue = true;
//		}
//		else {
//			istrue = true;
//		}
//	}
//	
	// Update is called once per frame
//	void OnTriggerEnter (Collider other)
//	{
//		if (!ignorelayers.Contains(other.gameObject.layer)) {
//			if (mystatus == PositionStatus.crouch2prone) {
//				transform.parent.GetComponent<FirstPersonController> ().cancrouch2prone = false;
//			}
//			if (mystatus == PositionStatus.prone2crouch) {
//				transform.parent.GetComponent<FirstPersonController> ().canprone2crouch = false;
//			}
//			if (mystatus == PositionStatus.crouch2idle) {
//				transform.parent.GetComponent<FirstPersonController> ().cancrouch2idle = false;
//			}
//		}
//	}
	void OnTriggerStay(Collider otheroo)
	{
//	//	Debug.Log ("Wdlaojd" + otheroo.name);
//		bool istrue = true;
//		istrue = true;
		if (!ignorelayers.Contains (otheroo.gameObject.layer)) {
//			if (mystatus == PositionStatus.crouch2prone) {
//				istrue = false;
//			}
//			if (mystatus == PositionStatus.prone2crouch) {
//				istrue = false;
//			}
//			if (mystatus == PositionStatus.crouch2idle) {
//				istrue = false;
//				Debug.Log (otheroo.name);
//			}
//		}
//
//		if (mystatus == PositionStatus.crouch2prone) {
//			transform.parent.GetComponent<FirstPersonController> ().cancrouch2prone = istrue;
//		}
//		if (mystatus == PositionStatus.prone2crouch) {
//			transform.parent.GetComponent<FirstPersonController> ().canprone2crouch = istrue;
//		}
//		if (mystatus == PositionStatus.crouch2idle) {
//			transform.parent.GetComponent<FirstPersonController> ().cancrouch2idle = istrue;
//			Debug.Log ("istrue");
//		}
			if (mystatus == PositionStatus.crouch2prone) {
				transform.parent.GetComponent<FirstPersonController> ().cancrouch2prone = false;
			} else if (mystatus == PositionStatus.prone2crouch) {
				transform.parent.GetComponent<FirstPersonController> ().canprone2crouch = false;
			} else {
				transform.parent.GetComponent<FirstPersonController> ().cancrouch2idle = false;
			}
		}
	}
	void OnTriggerExit (Collider othero)
	{
		bool istrue = true;
		istrue = true;
//		foreach (Collider other in othero) {
		if (!ignorelayers.Contains (othero.gameObject.layer)) {
//				if (mystatus == PositionStatus.crouch2prone) {
//					istrue = true;
//				}
//				if (mystatus == PositionStatus.prone2crouch) {
//					istrue = false;
//				}
//				if (mystatus == PositionStatus.crouch2idle) {
//					istrue = false;
//				}
//			}
//		}
//
			if (mystatus == PositionStatus.crouch2prone) {
				transform.parent.GetComponent<FirstPersonController> ().cancrouch2prone = istrue;
			}
			if (mystatus == PositionStatus.prone2crouch) {
				transform.parent.GetComponent<FirstPersonController> ().canprone2crouch = istrue;
			}
			if (mystatus == PositionStatus.crouch2idle) {
				transform.parent.GetComponent<FirstPersonController> ().cancrouch2idle = istrue;
			}
		}
	}
}
