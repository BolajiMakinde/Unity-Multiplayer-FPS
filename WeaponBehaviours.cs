using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;
using RootMotion.FinalIK;

public class WeaponBehaviours : MonoBehaviour
{

    // Use this for initialization

    public FullBodyBipedIK ik;

	public Transform leftHandf;

	public Transform leftElbowf;

	public Transform leftShoulderf;

	public Transform rightHandf;

	public Transform rightElbowf;

	public Transform rightShoulderf;

    public Transform leftHandi;
    
    public Transform rightHandi;

	public float ActiveWeaponID;

	public float pendingWeaponID = 0;

	public Transform replacementaim;

	public Transform Hipweaponholding;

	public Transform Backweaponholding;

	public float cycletimer;

	public bool cycleswitch = false;

	public float mindista;

	[SerializeField] public WeaponStatistics.Set mainset;

	[System.Serializable]

	public class Weapon
	{
		public float cyclespeed = 0.1f;

	//	public float cycletimer;

		public string WeaponName;

		public float WeaponID;

		public InteractionObject weapongo;

		public bool carried;

		public FullBodyBipedEffector[] effectors;

		public Transform LeftHandIdle;

		public Transform RightHandIdle;

		public enum HandBase {right, left};

		public HandBase handbase;

		public bool isReloading;

		public bool waittest;

		public bool mysterybool;

		public float throwspeed;

		public float burstdelay;
	}

	//[SerializeField] public Weapon[] weaponlist;

	public List<Weapon> weaponlist;

	public Weapon pickedweapon;

	public float dropagainwaittime;

	public float droptimerreset;

	public float pickagainwaittime;

	public float picktimerreset;

	public bool enableidle;

	public bool switching;

	public GameObject cycler;

	public Transform lookattransform;

	public Transform elbowbendtransform;

	public Transform shoulderrottransform;

	public float changer;

	public bool cancycle;

	public bool canenabledraw;

	public bool canooficialpick = false;

	void Start ()
	{
		ik.solver.rightHandEffector.positionWeight = 1f;
		ik.solver.leftHandEffector.positionWeight = 1f;
		foreach (Weapon weaponion in weaponlist) {
			weaponion.cyclespeed = 0.1f;
		}
		canenabledraw = true;
//		cycletimer = 0f;
	}

    // Update is called once per frame

    void LateUpdate()
	{
		if (leftHandf != null && mainset.leftHand != null) {
			ik.solver.leftHandEffector.target = leftHandf;
			ik.solver.leftHandEffector.positionWeight = mainset.lhpw;
			ik.solver.leftHandEffector.rotationWeight = mainset.lhrw;
		//	leftHandf.position = new Vector3 (Mathf.Lerp (leftHandf.position.x, mainset.leftHand.transform.position.x, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp (leftHandf.position.y, mainset.leftHand.transform.position.y, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp (leftHandf.position.z, mainset.leftHand.transform.position.z, mainset.looklerpspeed * Time.deltaTime));
			leftHandf.position = Vector3.Lerp (leftHandf.position,mainset.leftHand.transform.position, mainset.looklerpspeed * Time.deltaTime);
			leftHandf.rotation = Quaternion.Lerp (leftHandf.rotation, mainset.leftHand.transform.rotation, mainset.looklerpspeed * Time.deltaTime);
			if (ik.solver.leftHandEffector.bone.gameObject.GetComponent<RotationLimit> () != null) {
				ik.solver.leftHandEffector.bone.gameObject.GetComponent<RotationLimit> ().Apply ();
			}
		} else {
			ik.solver.leftHandEffector.positionWeight = 0;
			ik.solver.leftHandEffector.rotationWeight = 0;
		}
		if (leftElbowf != null && mainset.leftElbow != null) {
			ik.solver.leftArmChain.bendConstraint.bendGoal = leftElbowf;
			leftElbowf.position = new Vector3 (Mathf.Lerp(leftElbowf.position.x, mainset.leftElbow.transform.position.x, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp(leftElbowf.position.y, mainset.leftElbow.transform.position.y, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp(leftElbowf.position.z, mainset.leftElbow.transform.position.z, mainset.looklerpspeed * Time.deltaTime));
			if (ik.solver.leftArmChain.bendConstraint.bone1.GetComponent<RotationLimit>() != null)
			{
				ik.solver.leftArmChain.bendConstraint.bone1.gameObject.GetComponent<RotationLimit> ().Apply ();
			}
		}
		else {
			ik.solver.leftArmChain.bendConstraint.bendGoal = null;
		}
		if (leftShoulderf != null && mainset.leftShoulder != null) {
			ik.solver.leftShoulderEffector.positionWeight = mainset.lspw;
			ik.solver.leftShoulderEffector.rotationWeight = mainset.lsrw;
			ik.solver.leftShoulderEffector.target = leftShoulderf;
			leftShoulderf.position = new Vector3 (Mathf.Lerp(leftShoulderf.position.x, mainset.leftShoulder.transform.position.x, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp(leftShoulderf.position.y, mainset.leftShoulder.transform.position.y, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp(leftShoulderf.position.z, mainset.leftShoulder.transform.position.z, mainset.looklerpspeed * Time.deltaTime));
			if (ik.solver.leftShoulderEffector.bone.gameObject.GetComponent<RotationLimit> () != null) {
				ik.solver.leftShoulderEffector.bone.gameObject.GetComponent<RotationLimitAngle> ().Apply ();
			}
		}
		else {
			ik.solver.leftShoulderEffector.target = null;
			ik.solver.leftShoulderEffector.positionWeight = 0;
			ik.solver.leftShoulderEffector.rotationWeight = 0;
		}
		if (leftElbowf != null && mainset.leftElbow != null) {
			ik.solver.leftArmChain.bendConstraint.weight = mainset.lebw;
		} else {
			ik.solver.leftArmChain.bendConstraint.weight = 0;
		}
		if (rightHandf != null && mainset.rightHand != null) {
			ik.solver.rightHandEffector.positionWeight = mainset.rhpw;
			ik.solver.rightHandEffector.rotationWeight = mainset.rhrw;
			ik.solver.rightHandEffector.target = rightHandf;
			rightHandf.position = new Vector3 (Mathf.Lerp(rightHandf.position.x, mainset.rightHand.transform.position.x, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp(rightHandf.position.y, mainset.rightHand.transform.position.y, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp(rightHandf.position.z, mainset.rightHand.transform.position.z, mainset.looklerpspeed * Time.deltaTime));
			leftHandf.rotation = Quaternion.Lerp (leftHandf.rotation, mainset.leftHand.transform.rotation, mainset.looklerpspeed * Time.deltaTime);
			if (ik.solver.rightHandEffector.bone.gameObject.GetComponent<RotationLimit> () != null) {
				ik.solver.rightHandEffector.bone.gameObject.GetComponent<RotationLimit> ().Apply ();
			}
		}
		else {
			ik.solver.rightHandEffector.positionWeight = 0;
			ik.solver.rightHandEffector.rotationWeight = 0;
		}
		if (rightElbowf != null && mainset.rightElbow != null) {
			ik.solver.rightArmChain.bendConstraint.bendGoal = rightElbowf;
			rightElbowf.position = new Vector3 (Mathf.Lerp (rightElbowf.position.x, mainset.rightElbow.transform.position.x, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp (rightElbowf.position.y, mainset.rightElbow.transform.position.y, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp (rightElbowf.position.z, mainset.rightElbow.transform.position.z, mainset.looklerpspeed * Time.deltaTime));
			if (ik.solver.leftArmChain.bendConstraint.bone1.GetComponent<RotationLimit>() != null)
			{
				ik.solver.leftArmChain.bendConstraint.bone1.gameObject.GetComponent<RotationLimit> ().Apply ();
			}
		} else {
			ik.solver.rightArmChain.bendConstraint.bendGoal = null;
		}
		if (rightShoulderf != null && mainset.rightShoulder != null) {
			ik.solver.rightShoulderEffector.positionWeight = mainset.rspw;
			ik.solver.rightShoulderEffector.rotationWeight = mainset.rsrw;
			ik.solver.rightShoulderEffector.target = rightShoulderf;
			rightShoulderf.position = new Vector3 (Mathf.Lerp (rightShoulderf.position.x, mainset.rightShoulder.transform.position.x, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp (rightShoulderf.position.y, mainset.rightShoulder.transform.position.y, mainset.looklerpspeed * Time.deltaTime), Mathf.Lerp (rightShoulderf.position.z, mainset.rightShoulder.transform.position.z, mainset.looklerpspeed * Time.deltaTime));
			if (ik.solver.rightShoulderEffector.bone.gameObject.GetComponent<RotationLimit> () != null) {
				ik.solver.rightShoulderEffector.bone.gameObject.GetComponent<RotationLimitAngle> ().Apply ();
			}
		} else {
			ik.solver.rightShoulderEffector.target = null;
			ik.solver.rightShoulderEffector.positionWeight = 0;
			ik.solver.rightShoulderEffector.rotationWeight = 0;
		}
		if (rightElbowf != null && mainset.rightElbow != null) {
			ik.solver.rightArmChain.bendConstraint.weight = mainset.rebw;
		}
		else {
			ik.solver.rightArmChain.bendConstraint.weight = 0;
		}
		ik.solver.leftHandEffector.planeRotationOffset.x += mainset.lhrwoffset.x;
		ik.solver.leftHandEffector.planeRotationOffset.y += mainset.lhrwoffset.y;
		ik.solver.leftHandEffector.planeRotationOffset.z += mainset.lhrwoffset.z;
		gameObject.GetComponent<LookAtIK>().solver.IKPositionWeight = Mathf.Lerp(gameObject.GetComponent<LookAtIK>().solver.IKPositionWeight,mainset.lookatweight,Time.deltaTime* mainset.looklerpspeed);
		lookattransform.position = new Vector3 (Mathf.Lerp(lookattransform.position.x, mainset.lookattarget.position.x,Time.deltaTime * mainset.looklerpspeed), Mathf.Lerp(lookattransform.position.y, mainset.lookattarget.position.y,Time.deltaTime * mainset.looklerpspeed),Mathf.Lerp(lookattransform.position.z, mainset.lookattarget.position.z,Time.deltaTime * mainset.looklerpspeed));
		ik.solver.Update();
		changer = 0;
		
		if(dropagainwaittime < 100)
		{
			dropagainwaittime = dropagainwaittime + 0.1f;
		}
		if(pickagainwaittime < 100)
		{
			pickagainwaittime = pickagainwaittime + 0.1f;
		}
		int closestTriggerIndex = gameObject.GetComponent<InteractionSystem> ().GetClosestTriggerIndex();
		//Store current weapon and pick up weapon
		if(Vector3.Distance(rightHandf.position,weaponlist[0].weapongo.transform.position) <= mindista && canooficialpick == true)
		{
			rightHandi = ik.solver.rightHandEffector.bone;
			leftHandi = ik.solver.leftHandEffector.bone;
			if (weaponlist [0].handbase == Weapon.HandBase.right) {
				weaponlist [0].weapongo.transform.parent = rightHandi;
			} else {
				weaponlist [0].weapongo.transform.parent = leftHandi;
			}
			canooficialpick = false;
			weaponlist [0].weapongo.GetComponent<WeaponStatistics> ().currentindex = weaponlist [0].weapongo.GetComponent<WeaponStatistics> ().idleIndex;
		}
		if(cycleswitch == true && (weaponlist[0].weapongo.transform.parent == Hipweaponholding || weaponlist[0].weapongo.transform.parent == Backweaponholding) && closestTriggerIndex != -1)
		{
			if (gameObject.GetComponent<InteractionSystem> ().triggersInRange [0].transform.parent.gameObject.GetComponent<Rigidbody> ().isKinematic == false) {
				//Pick weapon
				Debug.Log ("entered" + "Cyclesewitch =" + cycleswitch + "Closesttrig" + closestTriggerIndex + "Weaponparent" + weaponlist [0].weapongo.transform.parent.gameObject.name + "triggername" + gameObject.GetComponent<InteractionSystem> ().triggersInRange [0].gameObject.name);
				weaponlist.Insert (0, pickedweapon);
				weaponlist [0].carried = true;
				weaponlist [0].weapongo = gameObject.GetComponent<InteractionSystem> ().triggersInRange [0].transform.parent.GetComponent<WeaponStatistics> ().mysets [3].interactobject;
				weaponlist [0].weapongo.transform.GetComponent<WeaponStatistics> ().isCarried = true;
				weaponlist [0].weapongo.GetComponent<WeaponStatistics> ().currentindex = weaponlist [0].weapongo.GetComponent<WeaponStatistics> ().pickIndex;
				weaponlist [0].weapongo.GetComponent<WeaponStatistics> ().parent = gameObject.transform;
				mainset = weaponlist [0].weapongo.GetComponent<WeaponStatistics> ().mysets [0];
				//mainset = weaponlist [0].weapongo.GetComponent<WeaponStatistics> ().mysets [weaponlist [0].weapongo.GetComponent<WeaponStatistics> ().currentindex];
				weaponlist [0].WeaponID = weaponlist [0].weapongo.GetComponent<WeaponStatistics> ().WeaponID;
				weaponlist [0].weapongo.gameObject.GetComponent<WeaponStatistics> ().DisablePhysics ();
				Physics.IgnoreCollision (weaponlist [0].weapongo.GetComponent<BoxCollider> (), transform.GetComponent<CharacterController> ().GetComponent<Collider> ());
				Physics.IgnoreCollision (weaponlist [0].weapongo.GetComponent<BoxCollider> (), weaponlist [0].weapongo.gameObject.GetComponent<WeaponStatistics> ().Characterc.GetComponent<CapsuleCollider> ());
				ActiveWeaponID = weaponlist [0].WeaponID;
				weaponlist [0].weapongo.transform.GetComponent<Rigidbody> ().isKinematic = true;
				pendingWeaponID = weaponlist [0].WeaponID;
				//weaponlist [0].weapongo.gameObject.GetComponent<WeaponStatistics> ().carryingstatus = 1;
				if (mainset.useinteractobject == true) {
					gameObject.GetComponent<InteractionSystem> ().TriggerInteraction (closestTriggerIndex, false, 3);
				} else {
					//StartCoroutine(moveHands())
				}
				gameObject.GetComponent<InteractionSystem> ().triggersInRange [0].gameObject.SetActive (false);
				weaponlist [0].weapongo.gameObject.layer = weaponlist [0].weapongo.transform.GetComponent<WeaponStatistics> ().newcullingmask;
				foreach (Transform child in weaponlist [0].weapongo.gameObject.GetComponentsInChildren<Transform>()) {
					child.gameObject.layer = weaponlist [0].weapongo.transform.GetComponent<WeaponStatistics> ().newcullingmask;
				}
				//	gameObject.GetComponent<AimIK> ().solver.transform = mainset.lookattarget;
				if (mainset.useinteractobject == true) {
					foreach (FullBodyBipedEffector e in mainset.effectors) {
						gameObject.GetComponent<InteractionSystem> ().StartInteraction (e, mainset.interactobject, mainset.interpolate);
					}
				} else {
					//	StartCoroutine((moveHands());
					leftHandi = mainset.leftHand;
					rightHandi = mainset.rightHand;
				}
				weaponlist [0].weapongo.transform.GetComponent<WeaponStatistics> ().pickUpTrigger.SetActive (false);
				weaponlist [0].WeaponName = weaponlist [0].weapongo.gameObject.name;
			}
			//		Debug.Log ("Success!!!");
			switching = false;
			cycleswitch = false;
			canooficialpick = true;
		}
		if((Input.GetButtonDown("Interact") && closestTriggerIndex != -1 && (mainset.useinteractobject == false || gameObject.GetComponent<InteractionSystem>().inInteraction == false) &&/* weaponlist.Count == 0*/ weaponlist[0].weapongo.GetComponent<WeaponStatistics>().currentindex != weaponlist[0].weapongo.GetComponent<WeaponStatistics>().pickIndex && pickagainwaittime >= 100 && gameObject.GetComponent<InteractionSystem> ().triggersInRange [0].gameObject.activeSelf == true) || (switching == true && (ik.solver.leftHandEffector.target == null && ik.solver.rightHandEffector.target == null)/*(ik.solver.leftHandEffector.positionWeight == 0f && ik.solver.rightHandEffector.positionWeight == 0f)*/ && /*gameObject.GetComponent<InteractionSystem>().GetProgress(mainset.effectors[0]) >= .5 &&*/ gameObject.GetComponent<InteractionSystem>().inInteraction == true))//mainset.effectors[0].// <= 0.1f))//GetComponent<InteractionSystem> ().triggersInRange [0].transform.parent.GetComponents<InteractionObject>()[gameObject.GetComponent<InteractionSystem> ().triggersInRange [0].gameObject.GetComponent<WeaponStatistics>().dropIndex].weightCurves[0].GetValue( gameObject.GetComponent<InteractionSystem> ().triggersInRange [0].gameObject.GetComponents<InteractionObject>()[gameObject.GetComponent<InteractionSystem> ().triggersInRange [0].gameObject.GetComponent<WeaponStatistics>().dropIndex].) < 0.1f))
		{
			dropagainwaittime = droptimerreset;
			//Store current Weapon
			foreach (Weapon weaponion in weaponlist)
			{
				weaponion.weapongo.GetComponent<WeaponStatistics> ().carryingstatus++;
				if (weaponion.WeaponID == ActiveWeaponID) {
					weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex = weaponion.weapongo.GetComponent<WeaponStatistics> ().storeIndex;
					mainset = weaponion.weapongo.GetComponent<WeaponStatistics> ().mysets [weaponion.weapongo.GetComponent<WeaponStatistics> ().storeIndex];
					foreach (FullBodyBipedEffector e in mainset.effectors) {
						if (mainset.useinteractobject == true) {
							gameObject.GetComponent<InteractionSystem> ().StartInteraction (e, mainset.interactobject, mainset.interpolate);
						}
					}
				}
			}

		}
		cancycle = true;
		foreach (Weapon weaponion in weaponlist) {
			replacementaim = weaponion.weapongo.gameObject.GetComponent<WeaponStatistics> ().barrel;
			weaponion.WeaponID = weaponion.weapongo.gameObject.GetComponent<WeaponStatistics> ().WeaponID;

			if (weaponion.WeaponID == ActiveWeaponID) {
				if ((CrossPlatformInputManager.GetButtonDown ("Interact") || CrossPlatformInputManager.GetButtonDown ("Switch")) && weaponlist [0].WeaponName != "" && dropagainwaittime >= 100) {

					if (Input.GetButtonDown ("Interact") && closestTriggerIndex != -1 && pickagainwaittime >= 100 && gameObject.GetComponent<InteractionSystem> ().triggersInRange [0].gameObject.activeSelf == true) {
						switching = true;
					}
					if (weaponlist.Count >= 3 && CrossPlatformInputManager.GetButtonDown ("Interact")) {
						pickagainwaittime = picktimerreset;
						weaponion.weapongo.GetComponent<WeaponStatistics> ().pickUpTrigger.SetActive (false);
						weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex = weaponion.weapongo.GetComponent<WeaponStatistics> ().dropIndex;
						mainset = weaponion.weapongo.GetComponent<WeaponStatistics> ().mysets [weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex];
						//ik.solver.leftHandEffector.target = mainset.leftHand;
						//ik.solver.rightHandEffector.target = mainset.rightHand;
						gameObject.GetComponent<InteractionSystem> ().StopAll ();
	//					gameObject.GetComponent<AimIK> ().solver.transform = mainset.lookattarget;
						if (mainset.useinteractobject == true) {
							foreach (FullBodyBipedEffector e in mainset.effectors) {
								gameObject.GetComponent<InteractionSystem> ().StartInteraction (e, mainset.interactobject, mainset.interpolate);
							}
						} else {
						}
						leftHandi.GetComponent<HandPoser> ().poseRoot = mainset.leftHand;
	//					gameObject.GetComponent<AimIK> ().solver.transform = replacementaim;
						weaponion.weapongo.gameObject.GetComponent<WeaponStatistics> ().isCarried = false;
	//					Debug.Log ("Dropped");
	//					Debug.Log ("Interacted instead");
					} else {
						if (weaponion.cyclespeed == 0.1f && ((weaponlist.Count >= 1 && CrossPlatformInputManager.GetButtonDown("Switch") && cancycle == true) || (CrossPlatformInputManager.GetButtonDown("Interact") && closestTriggerIndex != -1))) {
							weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex = weaponion.weapongo.GetComponent<WeaponStatistics> ().storeIndex;
							mainset = weaponion.weapongo.GetComponent<WeaponStatistics> ().mysets [weaponion.weapongo.GetComponent<WeaponStatistics> ().storeIndex];

		//					gameObject.GetComponent<AimIK> ().solver.transform = mainset.lookattarget;
							foreach (FullBodyBipedEffector e in mainset.effectors) {
								if (mainset.useinteractobject == true) {
									gameObject.GetComponent<InteractionSystem> ().StartInteraction (e, mainset.interactobject, mainset.interpolate);
								}
							}
		//					Debug.Log ("Not Quite Cycled");
		//					Debug.Log ("Calledfromhere1");
							foreach (Weapon weaponionn in weaponlist) {
								Debug.Log ("Cycled");
								if (changer <= weaponlist.Count) {
									weaponionn.weapongo.gameObject.GetComponent<WeaponStatistics> ().Cycle (weaponlist.IndexOf (weaponionn));
									weaponionn.cyclespeed = 0.1f;
									cycler = weaponion.weapongo.gameObject;
									changer = changer + 1;
									cancycle = false;
								} else {
									changer = 0;
								}
		//						Debug.Log ("Called");
							}
						} else {
							weaponion.cyclespeed = 0.1f;
						}
					}
				} else {
					weaponion.cyclespeed = 0.1f;
				}
				if ((CrossPlatformInputManager.GetButtonDown ("Fire1") || CrossPlatformInputManager.GetButton("Fire1")) && (gameObject.GetComponent<InteractionSystem>().inInteraction == false || mainset != weaponion.weapongo.GetComponent<WeaponStatistics> ().mysets [weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex])) {
					weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex = weaponion.weapongo.GetComponent<WeaponStatistics> ().fireIndex;
					mainset = weaponion.weapongo.GetComponent<WeaponStatistics> ().mysets [weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex];
	//				gameObject.GetComponent<AimIK> ().solver.transform = mainset.lookattarget;
					if (mainset.useinteractobject == true) {
						foreach (FullBodyBipedEffector e in mainset.effectors) {
							gameObject.GetComponent<InteractionSystem> ().StartInteraction (e, mainset.interactobject, mainset.interpolate);
						}
					}
					leftHandi.GetComponent<HandPoser> ().poseRoot = mainset.leftHand;
					//rightHandi.GetComponent<HandPoser> ().weight = 0f;
	//				gameObject.GetComponent<AimIK> ().solver.transform = weaponion.weapongo.GetComponent<WeaponStatistics>().barrel;
					weaponion.weapongo.gameObject.GetComponent<WeaponStatistics> ().muzzleflash.SetActive (true);
				}
				else if (CrossPlatformInputManager.GetButtonDown ("Reload") && mainset.interactobject.gameObject.GetComponent<WeaponStatistics>().drawing == false) {
					weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex = weaponion.weapongo.GetComponent<WeaponStatistics> ().reloadIndex;
					mainset.active = true;
				//	StartCoroutine(weaponion.weapongo.GetComponent<WeaponStatistics> ().readcurve ());
					mainset = weaponion.weapongo.GetComponent<WeaponStatistics> ().mysets [weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex];
//					ik.solver.leftHandEffector.positionWeight = 0;
//					ik.solver.rightHandEffector.positionWeight = 0;
//					gameObject.GetComponent<AimIK> ().solver.transform = mainset.lookattarget;
					foreach (FullBodyBipedEffector e in mainset.effectors) {
						if (mainset.useinteractobject == true) {
							gameObject.GetComponent<InteractionSystem> ().StartInteraction (e, mainset.interactobject, mainset.interpolate);
						}
					}
					//ik.solver.leftHandEffector.positionWeight = 0f;
					//ik.solver.rightHandEffector.rotationWeight = 0f;
					//ik.solver.leftHandEffector.target = mainset.leftHand;
					//ik.solver.rightHandEffector.target = mainset.rightHand;
					leftHandi.GetComponent<HandPoser>().poseRoot = mainset.leftHand;
	//				Debug.Log ("Realodeded");
				} else if (weaponion.weapongo.GetComponent<WeaponStatistics>().drawing == true && canenabledraw == true) {
					mainset.interactobject.gameObject.GetComponent<WeaponStatistics> ().currentindex = mainset.interactobject.gameObject.GetComponent<WeaponStatistics> ().drawIndex;
					mainset = weaponion.weapongo.GetComponent<WeaponStatistics> ().mysets [weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex];
					//					ik.solver.leftHandEffector.positionWeight = 0;
					//					ik.solver.rightHandEffector.positionWeight = 0;
					//					gameObject.GetComponent<AimIK> ().solver.transform = mainset.lookattarget;
						StartCoroutine (waitxsec());
				//	ik.solver.leftHandEffector.positionWeight = 0f;
				//	ik.solver.rightHandEffector.rotationWeight = 0f;
					canenabledraw = false;
					//ik.solver.leftHandEffector.target = mainset.leftHand;
					//ik.solver.rightHandEffector.target = mainset.rightHand;
					leftHandi.GetComponent<HandPoser>().poseRoot = mainset.leftHand;
					Debug.Log ("Drew");
				} else {
					if ((gameObject.GetComponent<InteractionSystem> ().inInteraction == false && mainset.active == false) && weaponlist[0].weapongo.gameObject.activeSelf == true && weaponlist[0].weapongo.gameObject.GetComponent<Rigidbody>().isKinematic == true && mainset.interactobject.gameObject.GetComponent<WeaponStatistics>().drawing == false) {
						weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex = weaponion.weapongo.GetComponent<WeaponStatistics> ().idleIndex;
						mainset = weaponion.weapongo.GetComponent<WeaponStatistics> ().mysets [weaponion.weapongo.GetComponent<WeaponStatistics> ().currentindex];
					//	ik.solver.leftHandEffector.target = mainset.leftHand;
					//	ik.solver.rightHandEffector.target = mainset.rightHand;
		//				gameObject.GetComponent<AimIK> ().solver.transform = mainset.lookattarget;
						if (mainset.useinteractobject == true) {
							foreach (FullBodyBipedEffector e in mainset.effectors) {
								gameObject.GetComponent<InteractionSystem> ().StartInteraction (e, mainset.interactobject, mainset.interpolate);
							}
						} else {
							
						}
						leftHandi.GetComponent<HandPoser> ().poseRoot = mainset.leftHand;
				//		Debug.Log ("Idled");
					} else {
				//		Debug.Log ("prevented");
					}
					if (CrossPlatformInputManager.GetButtonUp ("Fire1")) {
						weaponion.weapongo.gameObject.GetComponent<WeaponStatistics> ().muzzleflash.SetActive (false);
						weaponion.weapongo.gameObject.GetComponent<WeaponStatistics>().smoke.GetComponent<ParticleSystem> ().loop = false;
					}
				}
				if (weaponion.isReloading == true && weaponion.waittest == true && weaponion.weapongo.gameObject.GetComponent<WeaponStatistics> ().isCarried == true && gameObject.GetComponent<FirstPersonController> ().disableproneoveride == false) {
					foreach (FullBodyBipedEffector e in mainset.effectors) {
						if (weaponion.handbase == Weapon.HandBase.right) {
							ik.solver.leftHandEffector.target = mainset.leftHand;
			////				ik.solver.leftHandEffector.positionWeight = 0f;
			////				ik.solver.leftHandEffector.rotationWeight = 0f;
							Vector3 leftHandTargetPosRelativeToRight = rightHandi.InverseTransformPoint (mainset.leftHand.position);
							Quaternion leftHandTargetRotRelativeToRight = Quaternion.Inverse (rightHandi.rotation) * mainset.leftHand.rotation;
							ik.solver.leftHandEffector.position = rightHandi.TransformPoint (leftHandTargetPosRelativeToRight);
							ik.solver.leftHandEffector.rotation = rightHandi.rotation * leftHandTargetRotRelativeToRight;
							leftHandi.GetComponent<HandPoser> ().poseRoot = mainset.leftHand;
							rightHandi.GetComponent<HandPoser> ().poseRoot = mainset.rightHand;
			//				gameObject.GetComponent<AimIK> ().solver.transform = mainset.lookattarget;
							if (mainset.useinteractobject == true) {
								gameObject.GetComponent<InteractionSystem> ().StartInteraction (e, mainset.interactobject, weaponion.mysterybool);
							}
						} else {
							ik.solver.rightHandEffector.target = mainset.rightHand;
			////				ik.solver.rightHandEffector.positionWeight = 0f;
			////				ik.solver.rightHandEffector.rotationWeight = 0f;
							Vector3 rightHandTargetPosRelativeToRight = leftHandi.InverseTransformPoint (mainset.rightHand.position);
							Quaternion rightHandTargetRotRelativeToRight = Quaternion.Inverse (leftHandi.rotation) * mainset.rightHand.rotation;
							ik.solver.rightHandEffector.position = leftHandi.TransformPoint (rightHandTargetPosRelativeToRight);
							ik.solver.rightHandEffector.rotation = leftHandi.rotation * rightHandTargetRotRelativeToRight;
				//			gameObject.GetComponent<AimIK> ().solver.transform = mainset.lookattarget;
							if (mainset.useinteractobject == true) {
								gameObject.GetComponent<InteractionSystem> ().StartInteraction (e, mainset.interactobject, mainset.interpolate);
							}
							leftHandi.GetComponent<HandPoser> ().poseRoot = mainset.leftHand;
							rightHandi.GetComponent<HandPoser> ().poseRoot = mainset.rightHand;
						}
					}
					weaponion.waittest = false;
				}
				if (gameObject.GetComponent<FirstPersonController> ().disableproneoveride == true) {
					ik.solver.rightHandEffector.target = null;
					ik.solver.rightHandEffector.positionWeight = 0f;
					ik.solver.rightHandEffector.rotationWeight = 0f;
					ik.solver.leftHandEffector.target = null;
					ik.solver.leftHandEffector.positionWeight = 0f;
					ik.solver.leftHandEffector.rotationWeight = 0f;
				}
			} else {
				
			}

		}
	}
	public IEnumerator waitxsec(){
		yield return new WaitForSeconds (mainset.interactobject.gameObject.GetComponent<WeaponStatistics>().drawoffset);
		yield return new WaitUntil(() => gameObject.GetComponent<InteractionSystem>().inInteraction == false);
		ik.solver.leftHandEffector.target = mainset.leftHand;
		ik.solver.rightHandEffector.target = mainset.rightHand;
		gameObject.GetComponent<InteractionSystem> ().StopAll ();
		foreach (FullBodyBipedEffector e in mainset.effectors) {
			if (mainset.useinteractobject == true) {
				gameObject.GetComponent<InteractionSystem> ().StartInteraction (e, mainset.interactobject, mainset.interpolate);
			}
		}
	//	mainset.interactobject.gameObject.GetComponent<WeaponStatistics> ().drawing = false;
		canenabledraw = true;
	//	yield return new WaitForSeconds (mainset.interactobject.gameObject.GetComponent<WeaponStatistics>().drawoffset);
		yield return new WaitUntil (() => gameObject.GetComponent<InteractionSystem>().inInteraction == false); // && mainset.interactobject != mainset.interactobject.gameObject.GetComponent<WeaponStatistics> ().mysets [mainset.interactobject.gameObject.GetComponent<WeaponStatistics> ().storeIndex].interactobject);
		mainset.interactobject.gameObject.GetComponent<WeaponStatistics> ().drawing = false;
		Debug.Log("I'm in!");
	//	canenabledraw = true;
	}
	//public IEnumerator moveHands(Vector3 target, float speed, bool lerp, IKEffector effectort ,float weightgoal) {
	//	if (lerp == true) {
		//	ik.solver.SetEffectorWeights (effectort, Mathf.Lerp (ik.solver.GetEffector (effectort).positionWeight, weightgoal, Time.deltaTime * speed), Mathf.Lerp (ik.solver.GetEffector (effectort).rotationWeight, weightgoal, Time.deltaTime * speed));
	//	} else {
		//	ik.solver.SetEffectorWeights(effectort,Mathf.MoveTowards(ik.solver.GetEffector(effectort).positionWeight,weightgoal,Time.deltaTime*speed),Mathf.Lerp(ik.solver.GetEffector(effectort).rotationWeight,weightgoal,speed));
	//	}

	//}
}