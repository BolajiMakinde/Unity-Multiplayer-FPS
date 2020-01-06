using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityEditor;
using RootMotion.FinalIK;

public class WeaponStatistics : MonoBehaviour {

	public float WeaponID;

	public GameObject Characterc;

	public GameObject MagHand;

	public GameObject CockHand;

	public Transform LeftHandIdle;

	public Transform RightHandIdle;

	public Transform trueparent;

	public int carryingstatus;

	public Transform barrel;

	public bool editmode;

	public enum CarryPosition

	{
		hip, back
	}

	public CarryPosition mycarryposition;

	public int currentindex;

	[System.Serializable]

	public class Set
	{
		public string name;

		public bool useinteractobject;

		public InteractionObject interactobject;

		public int index;

		public FullBodyBipedEffector[] effectors;

		public enum HandRoot
		{
			right , left
		}

		public HandRoot handroot;

		public Transform leftHand;

		public float lhpw;

		public float lhrw;

		public Vector3 lhrwoffset;

		public Transform rightHand;

		public float rhpw;

		public float rhrw;

		public Vector3 rhrwoffset;

		public Transform leftElbow;

		public float lebw;

		public Vector3 lerwoffset;

		public Transform rightElbow;

		public float rebw;

		public Vector3 rerwoffset;

		public Transform leftShoulder;

		public float lspw;

		public float lsrw;

		public Transform rightShoulder;

		public float rspw;

		public float rsrw;

		public bool interpolate;

		public Transform lookattarget;

		public float looklerpspeed;

		public float lookatweight;

		public float nonbaseelbowweight;

		public GameObject elbowbendweight;

		public float nonbaseshoulderweight;

		public GameObject shoulderbendweight;

		public bool lockposition;

		public float useaimikweight;

		public float enableaimikspeed;

		public AnimationCurve nonbasecurve;

		public bool active;
	}

	public enum HandRoot
	{
		right , left
	}

	public GameObject pickUpTrigger;

	public int idleIndex;

	public int fireIndex;

	public int reloadIndex;

	public int storeIndex;

	public int dropIndex;

	public int pickIndex;

	public int drawIndex;

	public HandRoot myHandRoot;

	public Vector3 localPos;

	public Vector3 localRot;

	public Set[] mysets;

	public float ammoleft;

	public bool isCarried;

	public Transform parent;

	public GameObject scene;

	public float throwspeed;

	public Vector3 throwdirection;

	public float waittime;

	public float pickuptime;

	public int newcullingmask;

	public bool enable;

	public float switchspeed;

	public GameObject muzzleflash;

	public GameObject muzzlelight;

	public AnimationCurve muzzleIntensityvariance;

	public AnimationCurve muzzlecolorRvariance;

	public AnimationCurve muzzlecorlorGvariance;

	public AnimationCurve muzzlecolorBvariance;

	public AnimationCurve muzzleRotateVariance;

	public bool rotatx;

	public bool rotaty;

	public bool rotatz;

	public float muzzledisablefrequency;

	public float muzzlespeedmultiplier;

	public bool muzzleintiatied;

	[Tooltip("This should not be a child of the muzzleflash object and should have loop enabled in the particlesystem")]
	public GameObject smoke;

	public float drawoffset;

	public bool drawing;

	public float curvetimer;

	public float switchrotationmult = 10.0f;

	// Use this for initialization
	void Start () {
		Physics.IgnoreCollision (gameObject.GetComponent<BoxCollider>(),Characterc.GetComponent<CapsuleCollider>(),true);
		Physics.IgnoreCollision (gameObject.GetComponent<BoxCollider> (), parent.GetComponent<CharacterController> ().GetComponent<Collider>(),true);
		pickUpTrigger.SetActive (false);
//		foreach (Set sette in mysets) {
//			sette.handroot = myHandRoot;
//		}
	}
	
	// Update is called once per frame
	void Update () {
		parent.GetComponent<AimIK> ().solver.IKPositionWeight = Mathf.Lerp (parent.GetComponent<AimIK> ().solver.IKPositionWeight, mysets[currentindex].useaimikweight, Time.deltaTime * mysets [currentindex].enableaimikspeed);
		if(muzzleflash != null && muzzleflash.activeInHierarchy == true && muzzleintiatied == true && carryingstatus == 1 && gameObject.GetComponent<Rigidbody>().isKinematic == true){
			StartCoroutine(MuzzleFlash ());
			muzzleintiatied = false;
		}
		if (isCarried == true) {
			gameObject.GetComponent<Animator> ().enabled = true;
			gameObject.layer = newcullingmask;
			foreach (Transform child in gameObject.GetComponentsInChildren<Transform>()) {
				child.gameObject.layer = newcullingmask;
			}
			if (carryingstatus == 1) {
				foreach (Set myset in mysets) {
					myset.interactobject = gameObject.GetComponents<InteractionObject> () [myset.index];
					if (myset == parent.GetComponent<WeaponBehaviours> ().mainset && myset.name == "Reload") {
						//				Debug.Log (", " + ", " +  ", " + myset.interactobject.);
					}
				}
				gameObject.GetComponent<Rigidbody> ().isKinematic = true;
				if (trueparent == transform.parent && editmode == false) {
					transform.localPosition = localPos;
					transform.localRotation = Quaternion.Euler (localRot);
				}
				parent.GetComponent<WeaponBehaviours> ().mainset = mysets [currentindex];
			} else {
				if (parent.gameObject.GetComponent<AimIK> ().solver.transform != barrel) {
					parent.gameObject.GetComponent<AimIK> ().solver.transform = parent.gameObject.GetComponent<WeaponBehaviours> ().replacementaim;
				}
				//if ((gameObject.transform.position == parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.position && mycarryposition == CarryPosition.hip) || (gameObject.transform.position == parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform.position && mycarryposition == CarryPosition.back)) {
				if(gameObject.transform.localPosition == Vector3.zero && gameObject.transform.localRotation == Quaternion.identity) { 
					curvetimer = 0;
				} else {
					curvetimer += Time.deltaTime;
					if (mycarryposition == CarryPosition.hip && drawing == false) {
						gameObject.GetComponent<Rigidbody> ().isKinematic = true;
						gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.position, switchspeed * switchrotationmult* Time.deltaTime);
						//gameObject.transform.position.y += gameObject.GetComponent<WeaponBehaviours> ().mainset.nonbasecurve.Evaluate (curvetimer);
						//gameObject.transform.position = new Vector3(Mathf.Lerp(gameObject.transform.position.x, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.position.x,Time.deltaTime * switchspeed), Mathf.Lerp(gameObject.transform.position.y, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.position.y,Time.deltaTime * switchspeed),Mathf.Lerp(gameObject.transform.position.z, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.position.z,Time.deltaTime * switchspeed));
						gameObject.transform.rotation = Quaternion.RotateTowards (gameObject.transform.rotation, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.rotation, Time.deltaTime * switchrotationmult * switchspeed);
						//	gameObject.transform.rotation = new Quaternion(Mathf.Lerp(gameObject.transform.rotation.x, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.rotation.x,Time.deltaTime * switchspeed), Mathf.Lerp(gameObject.transform.rotation.y, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.rotation.y,Time.deltaTime * switchspeed),Mathf.Lerp(gameObject.transform.rotation.z, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.rotation.z,Time.deltaTime * switchspeed), Mathf.Lerp(gameObject.transform.rotation.w, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.rotation.w,Time.deltaTime * switchspeed));
//					gameObject.transform.position.y = Mathf.Lerp(gameObject.transform.position.y, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.position.y,Time.deltaTime * switchspeed);
//					gameObject.transform.position.z = Mathf.Lerp(gameObject.transform.position.z, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.position.z,Time.deltaTime * switchspeed);
//					gameObject.transform.rotation.x = Mathf.Lerp(gameObject.transform.rotation.x, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.rotation.x,Time.deltaTime * switchspeed);
//					gameObject.transform.rotation.y = Mathf.Lerp(gameObject.transform.rotation.y, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.rotation.y,Time.deltaTime * switchspeed);
//					gameObject.transform.rotation.z = Mathf.Lerp(gameObject.transform.rotation.z, parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform.rotation.z,Time.deltaTime * switchspeed);
						if (gameObject.transform.parent != parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform) {
							parent.GetComponent<WeaponBehaviours> ().cycleswitch = true;
						//	parent.GetComponent<WeaponBehaviours> ().canooficialpick = true;
							gameObject.transform.parent = parent.GetComponent<WeaponBehaviours> ().Hipweaponholding.transform;
						}
					} else {
						if (drawing == false) {
							gameObject.GetComponent<Rigidbody> ().isKinematic = true;
							gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform.position, switchspeed * switchrotationmult * Time.deltaTime);
							//gameObject.transform.position = new Vector3 (Mathf.Lerp (gameObject.transform.position.x, parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform.position.x, Time.deltaTime * switchspeed), Mathf.Lerp (gameObject.transform.position.y, parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform.position.y, Time.deltaTime * switchspeed), Mathf.Lerp (gameObject.transform.position.z, parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform.position.z, Time.deltaTime * switchspeed));
							gameObject.transform.rotation = Quaternion.RotateTowards (gameObject.transform.rotation, parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform.rotation, Time.deltaTime * switchrotationmult * switchspeed);
							//gameObject.transform.rotation = new Quaternion (Mathf.Lerp (gameObject.transform.rotation.x, parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform.rotation.x, Time.deltaTime * switchspeed), Mathf.Lerp (gameObject.transform.rotation.y, parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform.rotation.y, Time.deltaTime * switchspeed), Mathf.Lerp (gameObject.transform.rotation.z, parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform.rotation.z, Time.deltaTime * switchspeed), Mathf.Lerp (gameObject.transform.rotation.w, parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform.rotation.w, Time.deltaTime * switchspeed));
							if (gameObject.transform.parent != parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform) {
								parent.GetComponent<WeaponBehaviours> ().cycleswitch = true;
						//		parent.GetComponent<WeaponBehaviours> ().canooficialpick = true;
								gameObject.transform.parent = parent.GetComponent<WeaponBehaviours> ().Backweaponholding.transform;
							}
						}
					}
				}
			}
		} 
		else {
			if (gameObject.GetComponent<Rigidbody> ().isKinematic == true) {
				gameObject.GetComponent<Animator> ().enabled = false;
				gameObject.transform.SetParent (scene.transform);
	//			Debug.Log ("Set Scene");
				gameObject.GetComponent<Rigidbody> ().isKinematic = false;
				gameObject.layer = 0;
				foreach (Transform child in gameObject.GetComponentsInChildren<Transform>()) {
					if (child != pickUpTrigger) {
						child.gameObject.layer = 0;
					}
				}
				gameObject.transform.localRotation = Quaternion.Euler (throwdirection);
				gameObject.GetComponent<Rigidbody> ().AddForce (transform.localRotation.x * throwspeed,transform.localRotation.y * throwspeed,transform.localRotation.z * throwspeed);
		//add back later		parent.GetComponent<WeaponBehaviours> ().weaponlist.Remove (parent.GetComponent<WeaponBehaviours>().weaponlist[parent.GetComponent<WeaponBehaviours>().ActiveWeaponID - 1]);
			}
		}
		if (gameObject.GetComponent<Rigidbody> ().isKinematic == false && enable == true && pickUpTrigger.activeSelf == false && transform.parent == scene.transform)
		{
			StartCoroutine (Physicss(waittime));
			enable = false;
	//		Debug.Log ("Disabled");
		}
	}

	private IEnumerator Physicss(float resettime)
	{
		resettime = waittime;
		yield return new WaitForSeconds (resettime);
		Physics.IgnoreCollision (gameObject.GetComponent<BoxCollider> (), Characterc.GetComponent<CapsuleCollider> (), false);
		Physics.IgnoreCollision (gameObject.GetComponent<BoxCollider> (), parent.GetComponent<CharacterController> ().GetComponent<Collider>(), false);
		yield return new WaitForSeconds (pickuptime);
		pickUpTrigger.SetActive (true);
		enable = true;
	}
	private IEnumerator MuzzleFlash (){
		float timer = 0f;
		float muzzleintensityvarianceca = muzzleIntensityvariance.Evaluate (timer * muzzlespeedmultiplier);
		float muzrca = muzzlecolorRvariance.Evaluate (timer * muzzlespeedmultiplier);
		float muzgca = muzzlecorlorGvariance.Evaluate (timer * muzzlespeedmultiplier);
		float muzbca = muzzlecolorBvariance.Evaluate (timer * muzzlespeedmultiplier);
		float muzrotateca = muzzleRotateVariance.Evaluate (timer * muzzlespeedmultiplier);
		while (timer < muzzledisablefrequency) {
			timer += Time.deltaTime* muzzlespeedmultiplier;
			muzzleintensityvarianceca = muzzleIntensityvariance.Evaluate (timer * muzzlespeedmultiplier);
			muzrca = muzzlecolorRvariance.Evaluate (timer * muzzlespeedmultiplier);
			muzgca = muzzlecorlorGvariance.Evaluate (timer * muzzlespeedmultiplier);
			muzbca = muzzlecolorBvariance.Evaluate (timer * muzzlespeedmultiplier);
			muzrotateca = muzzleRotateVariance.Evaluate (timer * muzzlespeedmultiplier);
			muzzlelight.GetComponent<Light> ().intensity = muzzleintensityvarianceca;
			muzzlelight.GetComponent<Light> ().color = new Color (Mathf.Round(muzrca), Mathf.Round(muzgca), Mathf.Round(muzbca));
			if(rotatx){
				muzzleflash.transform.rotation = new Quaternion(muzrotateca * muzzleflash.transform.rotation.x,muzzleflash.transform.rotation.y, muzzleflash.transform.rotation.z, muzzleflash.transform.rotation.w);
			}
			if(rotaty){
				muzzleflash.transform.rotation = new Quaternion(muzzleflash.transform.rotation.x,muzzleflash.transform.rotation.y * muzrotateca, muzzleflash.transform.rotation.z, muzzleflash.transform.rotation.w);
			}
			if(rotatz){
				muzzleflash.transform.rotation = new Quaternion(muzzleflash.transform.rotation.x,muzzleflash.transform.rotation.y, muzzleflash.transform.rotation.z * muzrotateca, muzzleflash.transform.rotation.w);
			}
			if (muzzleflash.gameObject.activeSelf == true) {
				smoke.GetComponent<ParticleSystem> ().loop = true;
			}
			smoke.GetComponent<ParticleSystem> ().Play ();
			yield return null;
		}
		muzzleintiatied = true;
	}
	public IEnumerator readcurve () {
		float timer = 0f;
		//never used
		float weighting = parent.GetComponent<WeaponBehaviours> ().mainset.nonbasecurve.Evaluate (timer);

		//this code is never reached!!!!!!!!!!!!!fix later
		while (parent.GetComponent<WeaponBehaviours> ().mainset.nonbasecurve.Evaluate (timer) != null) {
			timer += Time.deltaTime;
			parent.GetComponent<WeaponBehaviours> ().mainset.active = true;
		}
		if(timer >= parent.GetComponent<WeaponBehaviours>().mainset.nonbasecurve.keys[parent.GetComponent<WeaponBehaviours>().mainset.nonbasecurve.keys.Length-1].time)
		{
			parent.GetComponent<WeaponBehaviours> ().mainset.active = false;
		}
		yield return null;
	}
	public void DisablePhysics  ()
	{
		Physics.IgnoreCollision (gameObject.GetComponent<BoxCollider>(),Characterc.GetComponent<CapsuleCollider>(),true);
		Physics.IgnoreCollision (gameObject.GetComponent<BoxCollider> (), parent.GetComponent<CharacterController> ().GetComponent<Collider>(),true);
	}

	public void Cycle (int me)
	{
		bool alreadychanged = false;
		if (carryingstatus != parent.GetComponent<WeaponBehaviours>().weaponlist.Count || parent.GetComponent<WeaponBehaviours>().switching == true) {
			if (parent.GetComponent<WeaponBehaviours> ().switching == true || (carryingstatus != parent.GetComponent<WeaponBehaviours>().weaponlist.Count && parent.GetComponent<WeaponBehaviours> ().switching == false)) {
	//			Debug.Log ("Here!s: " + gameObject.name + ":::" + parent.GetComponent<WeaponBehaviours>().switching);
				carryingstatus++;
				alreadychanged = true;
			}
	//		Debug.Log ("Here!s232334789: " + gameObject.name + ":::" + parent.GetComponent<WeaponBehaviours>().switching);
		} else {
			if (alreadychanged == false || parent.GetComponent<WeaponBehaviours>().switching == false) {
	//			Debug.Log ("in" + transform.name);
				carryingstatus = 1;
				parent.GetComponent<WeaponBehaviours> ().ActiveWeaponID = WeaponID;

				drawing = true;
			}
		}
		alreadychanged = false;
		parent.GetComponent<WeaponBehaviours> ().weaponlist [me].cyclespeed = 0f;
		//Debug.Log (transform.name + ":::" + parent.GetComponent<WeaponBehaviours>().weaponlist.Count + "::::" + parent.GetComponent<WeaponBehaviours>().switching);
	}
}
