using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using Rewired;
//using UnityStandardAssets.ImageEffects;
using System.Text;
using WPM;

public class BehaviourScript : MonoBehaviour {

	public Camera maincamera;

	public Canvas Maincanvas;

	public float hidebutton;

//	public GameObject customizablecharacter;

	public GameObject charactercustomwind;

	public Vector3 charactercustomscale;

	public LensFlare mainlensflare;

	public float mainfadespeed;

	public float hiddenfadespeed;

	public float currentfadespeed;

	public Transform actuallensflaresource;

	public Transform lensflaresource;

	public WPM.WorldMapGlobe globe;

	public GameObject materialatmosphere;

	public bool transitioning;

	public float flareoccludespeed;

	public earthrotate earthrotation;

	public AutoIntensity autointense;

	public bool controllerpolling;

	public int playerpolling;

	public float zoomlevel;

	public RenderTexture targtexture;

	public float centeringspeed;

	public Vector3 savedhomee;

	public float zoomoffset;

	public string pathname;

	public float latitude;

	public float longitude;

	public bool inspace;

	public Camera zoomcame;

	public Vector3 rotationcaminitial;

	public bool useortho;

	public float rotateorthovalue;

	public float finishrotateorthovalue;

	public Transform Rotatecameratarget;

	public bool extendedzoom;

	public bool preextendedzoom;

	public float preextendedzoomgoal;

	public float prepreextendedzoomgoal;

	public float preextendedzoomspeed;

	public bool reorientcamera;

	public float rotatemultiplier;

	public float goalorthosize;

	public float goalswitchspeed;

	public float getzoomlevel1;

	public float getzoomlevel2;

	public GameObject kirtlandgo;

	public float alphspeed;

	public float distance = 10;

	public float goDepth = 4;

	public Vector3 v3ViewPort;

	public Vector3 v3BottomLeft;

	public Vector3 v3TopRight;

	public float TOD;

	public float TODoffsete;

	public float PPfadeinspeed;

	public float PPfadeoutspeed;

	public float extendedzoomerrorcheck = .02f;

	public PostProcessVolume currentPV;

	public int nofcontrollersconnected;

	public int maxPlayers = 8;

	public int rewiredPlayerIdCounter = 0;

	public List<int> assignedJoysticks;

	public GameObject controllermanager;

	public Material[] imgmats;

	[System.Serializable]

	public class Progression
	{
		public string scenename;

		public RenderMode canvasrendermode;

		public bool PerspectiveOrtho;

		public Light progressionlight;

		public AudioClip[] ambientaudioarray;

		public AudioClip playonhighlight;

		public AudioClip playondehighlight;

		public GameObject[] disables;

		public float waittimea;

		public float waittimeb;

		public bool useCharacterCustomization;

		public GameObject[] blackoutscreen;

		public Transform cameratransform;

		public float speeed;

		public GameObject[] stateobjects;

		public bool usebloom;

		public bool usesunshafts;

		public bool usenoise;

		public float waitfornextambientaudiotime;

		public float waitfornextambientaudiotimedefaultvalue;

		public float progcustomizationid;

		public float localaddcontroller;

		[System.Serializable]
		public class Maps
		{
			public string mapname;

			public GameObject mapprefab;

			public float mapid;

			public string cityName;

			public Texture2D[] imgs;

			public int numberOfQuadrants;

			public float latitude;

			public float longitude;

			public bool uselonlat;

			public float fadeintime;

			public GameObject realsun;

			public Vector3 locpos;

			public Vector3 locscale;

			public Vector3 locrot;

			public float flickramount;

			public float flickrspeed;

			[Tooltip("Distance untill lighting changes")]

			public float flickrmult;

			public float flickrmultiplier;

			public float flickrmultiplieron;

			public Texture2D alphaoutline;

			public Vector3 sunrotation;

			public Vector3 initialcampos;

			public bool useTOD = true;

			public Material skybox;

			public float exposuredivision = 20.0f;

			public float exposureoffset;

			public float maxexposure;
		}

		public Maps[] maps;

		public Maps selectedmap;

		[System.Serializable]
		public class NonSwitchObjects
		{
			public string nonswitchname;

			public GameObject[] nonswitchables;

			public float nonnumber;

			[System.Serializable]
			public class Variants
			{
				public string variantsname;

				public float variant;
				
				public GameObject[] progressionvariants;
			}
			public Variants[] variants;

			public int iteratorv;

			public GameObject[] nonselectables;

			public PostProcessVolume PV;

			public Transform camtrans;

			public float camtransspeed;

			public float Trajectoriesnumber;

			public bool enableearthrotate;

			public bool lockcam = true;

			public bool allowselection;

			public bool enableearthrotation;

			public bool sunrotate;

			public ProFlareBatch batchscript;

			public float sunfadeinspeed;

			public float sunfadeoutspeed;
		}

		public NonSwitchObjects[] nonswitchobjects;

		public float number;

		public AudioClip mainaudio;

		public Material skybox;
	}

	[System.Serializable]

	public class Profile
	{
		public string name;

		public Player player;

		public bool Male;
		
		public float Height;
		
		public float Weight;

		public float SkinColor;

		public float EyeColor;

		public bool Online;



		public class Clothes
		{

		}

		public class PrimaryWeapon
		{
			
		}
		
		public class SecondaryWeapon
		{
			
		}
		
		public class TertiaryWeapon
		{
			
		}
		
		public class PrimarySkill
		{
			
		}
		
		public class SecondarySkill
		{
			
		}
		
		public class TertiarySkill
		{
			
		}
		
		public class Contacts
		{
			
		}

		public class Scorestreaks
		{

		}
	}

	public Profile[] profiles;

	public Profile[] loadedprofiles;

	public Profile selectedprofile;

	public Renderer earthrender;

	public Vector2 offseter;

	public Vector2 offseterspeed;

	public float waitfortimeseconds;

	public float waitformenuseconds;

	public GameObject[] dashinglines;

	public Progression[] progression;

	public Progression currentprogession;

	public float progressionnumber;

	public float progressionvariation;

	public string dashingstring;

	void Awake () {
		
		assignedJoysticks = new List<int>();

		// Subscribe to controller connected events
		ReInput.ControllerConnectedEvent += OnControllerConnected;
		progression[0].selectedmap.skybox.SetFloat ("_Exposure", 0.0f);
	}

	// Use this for initialization
	void Start () {

		StartCoroutine (waitfortime ());
		currentprogession.selectedmap.mapprefab.SetActive (false);
		currentfadespeed = RenderSettings.flareFadeSpeed;

		// NOTE: On some platforms/input sources, joysticks are ready at this time, but on others they may not be ready yet.
		// Must also check in OnControllerConected event.

		// Assign all Joysticks to the System Player initially removing assignment from other Players.
		AssignAllJoysticksToSystemPlayer(true);
	}
	void OnControllerConnected(ControllerStatusChangedEventArgs args) {
		if(args.controllerType != ControllerType.Joystick) return;

		// Check if this Joystick has already been assigned. If so, just let Auto-Assign do its job.
		if(assignedJoysticks.Contains(args.controllerId)) return;

		// Joystick hasn't ever been assigned before. Make sure it's assigned to the System Player until it's been explicitly assigned
		ReInput.players.GetSystemPlayer().controllers.AddController<Joystick>(args.controllerId,true); // remove any auto-assignments that might have happened
	}
	void AssignPlayer(int plnumber, Controller co, Profile pr){
		pr.player = ReInput.players.GetPlayer(plnumber);
		pr.player.controllers.AddController (co, true);
		controllermanager.transform.GetChild (plnumber).GetChild (0).GetComponent<Text> ().text = pr.name;
		controllermanager.transform.GetChild (plnumber + 8).GetComponent<Image> ().color = Color.green;
	}
	public void Setpolling(bool val)
	{
		controllerpolling = val;
	}
	public void setplayerpolling (int val)
	{
		playerpolling = val;
	}
	public void setprofile (int val)
	{
		if(profiles.Length >= val+1)
		{
			selectedprofile = profiles [val];
		}
	}

	void AssignNextPlayer() {
		if(rewiredPlayerIdCounter >= maxPlayers) {
			Debug.Log("Max player limit already reached!");
			return;
		}

		// Get the next Rewired Player Id
		int rewiredPlayerId = GetNextGamePlayerId();

		Debug.Log ("Next player ID is: " + rewiredPlayerId);

		// Get the Rewired Player
		Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

		// Determine which Controller was used to generate the Start Action
		Player systemPlayer = ReInput.players.GetSystemPlayer();
		var inputSources = systemPlayer.GetCurrentInputSources("Start");

		foreach(var source in inputSources) {

			if(source.controllerType == ControllerType.Keyboard || source.controllerType == ControllerType.Mouse) { // Assigning keyboard/mouse

				// Assign KB/Mouse to the Player
				AssignKeyboardAndMouseToPlayer(rewiredPlayer);

				// Disable KB/Mouse Assignment category in System Player so it doesn't assign through the keyboard/mouse anymore
				ReInput.players.GetSystemPlayer().controllers.maps.SetMapsEnabled(false, ControllerType.Keyboard, "Assignment");
				ReInput.players.GetSystemPlayer().controllers.maps.SetMapsEnabled(false, ControllerType.Mouse, "Assignment");
				break;

			} else if(source.controllerType == ControllerType.Joystick) { // assigning a joystick

				// Assign the joystick to the Player. This will also un-assign it from System Player
				AssignJoystickToPlayer(rewiredPlayer, source.controller as Joystick);
				break;

			} else { // Custom Controller
				throw new System.NotImplementedException();
			}
		}

		// Enable UI map so Player can start controlling the UI
//		rewiredPlayer.controllers.maps.SetMapsEnabled(true, "UI");
	}

	private void AssignKeyboardAndMouseToPlayer(Player player) {
		// Assign mouse to Player
		player.controllers.hasMouse = true;

		// Load the keyboard and mouse maps into the Player
		player.controllers.maps.LoadMap(ControllerType.Keyboard, 0, "Default", "Default", true);
		player.controllers.maps.LoadMap(ControllerType.Mouse, 0, "Default", "Default", true);

		// Exclude this Player from Joystick auto-assignment because it is the KB/Mouse Player now
		player.controllers.excludeFromControllerAutoAssignment = true;

		Debug.Log("Assigned Keyboard/Mouse to Player " + player.name);
	}

	private void AssignJoystickToPlayer(Player player, Joystick joystick) {
		// Assign the joystick to the Player, removing it from System Player
		player.controllers.AddController(joystick, true);

		// Mark this joystick as assigned so we don't give it to the System Player again
		assignedJoysticks.Add(joystick.id);

		Debug.Log("Assigned " + joystick.name + " to Player " + player.name);
	}

	private int GetNextGamePlayerId() {
		return rewiredPlayerIdCounter++;
	}

	void AssignAllJoysticksToSystemPlayer(bool removeFromOtherPlayers) {
		foreach(var j in ReInput.controllers.Joysticks) {
			ReInput.players.GetSystemPlayer().controllers.AddController(j, removeFromOtherPlayers);
		}
	}
	
	// Update is called once per frame
	void Update () {
//		for(int i = 0; i < ReInput.players.playerCount; i++) {
//			if(ReInput.players.GetPlayer(i).GetButtonDown("Start")) {
//				//AssignNextPlayer(ReInput.players.GetPlayer(i));
//				AssignNextPlayer ();
//			}
//		}
		if (progressionnumber == 1.33f) {
			for (int i = 0; i < controllermanager.transform.childCount / 3; i++) {
				if (loadedprofiles.Length > 0 && i + 1 < loadedprofiles.Length && i < nofcontrollersconnected) {
					controllermanager.transform.GetChild (i).gameObject.SetActive (true);
					controllermanager.transform.GetChild (i + 8).gameObject.SetActive (true);
					controllermanager.transform.GetChild (i + 16).gameObject.SetActive (true);
					controllermanager.transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = (loadedprofiles [i].name);
				} else if (i + 1 == loadedprofiles.Length && i < nofcontrollersconnected) {
					controllermanager.transform.GetChild (i).gameObject.SetActive (true);
					controllermanager.transform.GetChild (i + 8).gameObject.SetActive (true);
					controllermanager.transform.GetChild (i + 16).gameObject.SetActive (true);
					//		if (players.Count < nofcontrollersconnected) {
					controllermanager.transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = ("+");
					//		}
				} else {
					controllermanager.transform.GetChild (i).gameObject.SetActive (false);
					controllermanager.transform.GetChild (i + 8).gameObject.SetActive (false);
					controllermanager.transform.GetChild (i + 16).gameObject.SetActive (false);
				}
			}
			nofcontrollersconnected = ReInput.controllers.GetControllerCount (ControllerType.Custom) + ReInput.controllers.GetControllerCount (ControllerType.Joystick) + ReInput.controllers.GetControllerCount (ControllerType.Mouse);
		}
		//Sets Time of Day from 0 to 24
		TOD = (Quaternion.Angle (kirtlandgo.transform.rotation,autointense.transform.rotation)+180)/360*24;
		//TOD = autointense.transform.localRotation.y / 360 * 24;
		//getzoomlevel1 = globe.GetZoomLevel ();
		globe.transform.rotation = earthrender.transform.rotation;
//		gameObject.GetComponent<FlareOcclusion> ().occludefadespeed = flareoccludespeed;
		//float zoomamount;
		//zoomamount = Mathf.Lerp (globe.GetZoomLevel(), zoomlevel, Time.deltaTime);
		//globe.SetZoomLevel(Mathf.Lerp (globe.GetZoomLevel(), zoomlevel, Time.deltaTime));
	//	currentfadespeed = RenderSettings.flareFadeSpeed;
		if(lensflaresource.gameObject.GetComponent<Renderer>().isVisible == true)
		{
			//		StartCoroutine(waitsetseconds(40));
			mainlensflare.fadeSpeed = hiddenfadespeed;
			RenderSettings.flareFadeSpeed = hiddenfadespeed;

			//	if (lensflaresource.gameObject.renderer.) {
			//	}

		}
		else
		{
			//	StartCoroutine(waitsetseconds(130));
			mainlensflare.fadeSpeed = mainfadespeed;
			RenderSettings.flareFadeSpeed = mainfadespeed;
		}
	//	Vector3 tempe = customizablecharacter.transform.localScale;
	//	tempe.x = charactercustomscale.x * charactercustomwind.GetComponent<RectTransform>().rect.height;
	//	tempe.y = charactercustomscale.y * charactercustomwind.GetComponent<RectTransform>().rect.height;
	//	tempe.z = charactercustomscale.z * charactercustomwind.GetComponent<RectTransform>().rect.height;
	//	customizablecharacter.transform.localScale = tempe;

		//if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0) 
		//{
		//	hidebutton =1;
		//}
		foreach (Progression progressionist in progression) {
			if (inspace == false) {
				//progressionist.selectedmap.skybox.SetFloat ("", TOD);
				progressionist.selectedmap.skybox.SetFloat ("_Exposure", progressionist.selectedmap.maxexposure * Mathf.Sin((TOD/progressionist.selectedmap.exposuredivision)*Mathf.PI+ progressionist.selectedmap.exposureoffset));
			}
		//	else{
			//	progressionist.selectedmap.skybox.SetFloat ("_Exposure", 0.0f);
		//	}
			if (waitformenuseconds == 0) {
				if (progressionist.waittimea >= 0) {
					progressionist.waittimea--;
				}
				if (progressionist.waittimea == 0) {
					foreach (GameObject disablee in progressionist.disables) {
						disablee.SetActive (false);
					}
				}
				if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
					progressionist.waittimea = progressionist.waittimeb;
				}
				foreach (GameObject progressionblackout in progressionist.blackoutscreen) {
					if (progressionist.number == progressionnumber && progressionblackout.activeSelf == true) {
						Color temp = progressionblackout.GetComponent<Image> ().color;
						temp.a = Mathf.Lerp (temp.a, 0, Time.deltaTime);
						progressionblackout.GetComponent<Image> ().color = temp;
						if (temp.a <= 0.01) {
							progressionblackout.SetActive (false);
							//							mainlensflare.fadeSpeed = 0;
							//							RenderSettings.flareFadeSpeed = 0;
						}
						//						else {
						//							mainlensflare.fadeSpeed = 3;
						//							RenderSettings.flareFadeSpeed = 3;

						//						}
					//} else {
					//	if (progressionblackout.activeSelf == true) {
					//		progressionblackout.SetActive (false);
					//	}
					//}
				}
			}
		//	RenderSettings.sun = progressionist.progressionlight;
			if (currentprogession.number < 1.9f  && (zoomcame.transform.localPosition.y != goalorthosize || kirtlandgo.GetComponent<Renderer> ().material.color.a > progressionist.selectedmap.flickrmult)) {
				progressionist.selectedmap.realsun.GetComponent<Light> ().intensity = Mathf.Lerp (progressionist.selectedmap.realsun.GetComponent<Light> ().intensity, autointense.gameObject.GetComponent<Light> ().intensity * Mathf.Pow (Mathf.Abs (1 - (Mathf.Abs (12 - TOD) / 12)), progressionist.selectedmap.flickrspeed) * progressionist.selectedmap.flickrmultiplieron, progressionist.selectedmap.flickrspeed);
				progressionist.selectedmap.realsun.GetComponent<Light> ().color = autointense.gameObject.GetComponent<Light> ().color;
			} else {
				progressionist.selectedmap.realsun.GetComponent<Light> ().intensity = Mathf.Lerp (progressionist.selectedmap.realsun.GetComponent<Light> ().intensity, autointense.gameObject.GetComponent<Light> ().intensity * Mathf.Pow (Mathf.Abs (1 - (Mathf.Abs (12 - TOD) / 12)), 1f) * progressionist.selectedmap.flickrmultiplier, progressionist.selectedmap.flickrspeed);
				progressionist.selectedmap.realsun.GetComponent<Light> ().color = autointense.gameObject.GetComponent<Light> ().color;
			}
			progressionist.selectedmap.realsun.transform.eulerAngles = new Vector3(Quaternion.Angle(kirtlandgo.transform.rotation, mainlensflare.transform.rotation),0,0);
			globe.earthScenicLightDirection = actuallensflaresource.position;
			materialatmosphere.GetComponent<Renderer> ().material.SetVector ("_SunLightDirection", actuallensflaresource.position);
			if (progressionnumber - progressionist.number >= 0 && progressionnumber - progressionist.number < 1 && progressionist.mainaudio != null && gameObject.GetComponent<AudioSource>().isPlaying == false) {
				gameObject.GetComponent<AudioSource>().clip = progressionist.mainaudio;
				gameObject.GetComponent<AudioSource>().Play();

			}
			if(progressionist.number == 1 && progressionist.waitfornextambientaudiotime <= 0)
			{
				progressionist.waitfornextambientaudiotime = progressionist.waitfornextambientaudiotimedefaultvalue;
				gameObject.GetComponents<AudioSource>()[2].clip = progressionist.ambientaudioarray[UnityEngine.Random.Range(1,progressionist.ambientaudioarray.Length)];
				gameObject.GetComponents<AudioSource>()[2].Play();
			}
			if(progressionist.number == 1)
			{
				progressionist.waitfornextambientaudiotime--;
			}
		//}
		//foreach (Progression progressionist in progression) {
			if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0) {
				progressionist.waittimea = progressionist.waittimeb;
			}

			if(progressionist.waittimea >= 0)
			{
				progressionist.waittimea--;
			}
			if (progressionist.waittimea <= 0) {
				foreach (GameObject disablee in progressionist.disables) {
					disablee.SetActive(false);
				}
			}
			if (progressionist.waittimea > 0) {
				foreach (GameObject disablee in progressionist.disables) {
					disablee.SetActive(true);
				}
			}
			if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
				progressionist.waittimea = progressionist.waittimeb;
			}
			if(progressionnumber - progressionist.number >= 0 && progressionnumber - progressionist.number < 1) {



				if(RenderSettings.skybox != progressionist.skybox)
				{
					RenderSettings.skybox = progressionist.skybox;
				}
			}
			foreach (GameObject stateobject in progressionist.stateobjects) {
				if (progressionnumber - progressionist.number < 0 || progressionnumber - progressionist.number >= 1 ) {
					if (stateobject.activeSelf == true && (stateobject != charactercustomwind || (stateobject == charactercustomwind && progressionist.useCharacterCustomization == false))) {

						stateobject.SetActive (false);
					}
				}
				if (progressionnumber - progressionist.number >= 0 && progressionnumber - progressionist.number < 1) {

					if(Maincanvas.renderMode != progressionist.canvasrendermode)
					{
						Maincanvas.renderMode = progressionist.canvasrendermode;
					}
					if(maincamera.orthographic == progressionist.PerspectiveOrtho)
					{
						maincamera.orthographic = !progressionist.PerspectiveOrtho;
					}

					currentprogession = progressionist;

					int g = 0;
					foreach (Material mat in imgmats)
					{
						if (currentprogession.selectedmap.imgs.Length >= g+1) {
							mat.mainTexture = currentprogession.selectedmap.imgs [g];
							g++;
						} else {
						}

					}

					if(progressionist.usebloom == true)
					{
						PlayerPrefs.SetString("useBloom", "true");
					}
					if(progressionist.usesunshafts == true)
					{
						PlayerPrefs.SetString("useSunShafts","true");
					}
					if(progressionist.usenoise == true)
					{
						PlayerPrefs.SetString("useNoise","true");
						//		maincamera.GetComponent<NoiseAndScratches>().enabled = true;
					}
					if(progressionist.usebloom == false)
					{
						PlayerPrefs.SetString("useBloom", "false");
					}
					if(progressionist.usesunshafts == false)
					{
						PlayerPrefs.SetString("useSunShafts","false");
					}
					if(progressionist.usenoise == false)
					{
						PlayerPrefs.SetString("useNoise","false");

						//				maincamera.GetComponent<NoiseAndScratches>().enabled = false;
					}

					if (stateobject.activeSelf == false)// && progressionist.waittimea > 0)
					{
						stateobject.SetActive (true);

						RenderSettings.skybox = progressionist.skybox;

						if(stateobject == earthrender && (offseterspeed.x > 0 || offseterspeed.y > 0))
						{
							earthrender.material.SetTextureOffset ("_MainTex", new Vector2 (earthrender.material.GetTextureOffset ("_MainTex").x + offseter.x * Time.deltaTime, earthrender.material.GetTextureOffset ("_MainTex").y + offseter.y * Time.deltaTime));
						}
						gameObject.GetComponent<AudioSource>().clip = progressionist.mainaudio;

						gameObject.GetComponent<AudioSource>().Play();

					}
				}
			}
			//currentprogession = progressionist;
			foreach (Progression.NonSwitchObjects nonswitchobject in progressionist.nonswitchobjects){
				
				if(nonswitchobject.PV !=null)
				{
					if (nonswitchobject.nonnumber != progressionnumber && nonswitchobject.PV.weight != 0 && nonswitchobject.PV != currentPV) {
						nonswitchobject.PV.weight = Mathf.MoveTowards (nonswitchobject.PV.weight, 0, PPfadeoutspeed);
					}
					else if(nonswitchobject.nonnumber == progressionnumber && progressionnumber == progressionist.localaddcontroller){
//						if(nonswitchobject.nonnumber)
						Debug.Log("in conditions to add, player count is: " + ReInput.players.playerCount);
						for(int i = 0; i < ReInput.players.allPlayerCount-1; i++) {
							if(ReInput.players.GetPlayer(i).GetButtonDown("Start") || ReInput.players.GetSystemPlayer().GetButtonDown("Start") && controllerpolling == true) {
								if (ReInput.players.GetPlayer (i).GetButtonDown ("Start")) {
									AssignPlayer (playerpolling, ReInput.players.GetPlayer (i).GetCurrentInputSources ("Start") [0].controller, selectedprofile);
									Debug.Log ("assigning " + i + " player: " + selectedprofile.name + " to controller: " + ReInput.players.GetSystemPlayer().GetCurrentInputSources("Start")[0].controller);
								}else{
									AssignPlayer (playerpolling, ReInput.players.GetSystemPlayer ().GetCurrentInputSources ("Start") [0].controller, selectedprofile);
									Debug.Log ("assigning system player: " + selectedprofile.name + " to controller: " + ReInput.players.GetSystemPlayer().GetCurrentInputSources("Start")[0].controller);
								}

							}
						}

						currentPV = nonswitchobject.PV;

						nonswitchobject.PV.weight = Mathf.MoveTowards(nonswitchobject.PV.weight,1,PPfadeinspeed);
					}
					else{
						//controllerpolling = false;
					}
				}
				foreach (GameObject nonswitchable in nonswitchobject.nonswitchables) {
					if (nonswitchobject.nonnumber != progressionnumber || transitioning == true) {

						if (nonswitchable.activeSelf == true) {

							nonswitchable.SetActive (false);

							if (nonswitchable.GetComponent<Button> () != null || nonswitchable.GetComponent<Slider> () != null) {

								nonswitchable.GetComponent<Selectable> ().interactable = false;

								nonswitchable.GetComponent<Selectable> ().enabled = false;
							}
						}
						foreach (Progression.NonSwitchObjects.Variants variant in nonswitchobject.variants) {
							foreach (GameObject variante in variant.progressionvariants) {
								if (variante.activeSelf == true) {
									variante.SetActive (false);
								}
							}
						}
					} else {
						//	if (nonswitchable.activeSelf == false && progressionist.waittimea > 0) {
						if (nonswitchable.GetComponent<Button> () != null || nonswitchable.GetComponent<Slider> () != null) {

						//	Debug.Log ("Here too");
							if (nonswitchable.GetComponent<Button> () != null) {
								nonswitchable.GetComponent<Button> ().interactable = true;

								nonswitchable.GetComponent<Button> ().enabled = true;
							} else {
								nonswitchable.GetComponent<Slider> ().interactable = true;

								nonswitchable.GetComponent<Slider> ().enabled = true;
							}
						}
				//		Debug.Log ("Inhere");
						nonswitchable.SetActive (true);
						if (gameObject.GetComponent<EventSystem> ().currentSelectedGameObject == null && nonswitchable.GetComponent<Button>()) {
							gameObject.GetComponent<EventSystem> ().SetSelectedGameObject(nonswitchable);
							nonswitchable.tag = "highlighted";


						}
					}
					if (nonswitchobject.nonnumber == progressionnumber) {
						earthrotation.enabled = nonswitchobject.enableearthrotate;
						nonswitchobject.batchscript.FadeInSpeed = nonswitchobject.sunfadeinspeed;
						nonswitchobject.batchscript.FadeOutSpeed = nonswitchobject.sunfadeoutspeed;
						globe.enableCountryHighlight = nonswitchobject.allowselection;
						globe.allowUserRotation = nonswitchobject.enableearthrotation;
						globe.keepStraight = nonswitchobject.enableearthrotation;
						progressionist.cameratransform = nonswitchobject.camtrans;
						autointense.enabled = nonswitchobject.sunrotate;
						if(nonswitchobject.lockcam == true)
						{
							maincamera.transform.position = Vector3.Lerp (maincamera.transform.position,progressionist.cameratransform.position, progressionist.speeed * Time.smoothDeltaTime);
							maincamera.transform.parent = progressionist.cameratransform;
							maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, progressionist.cameratransform.rotation, progressionist.speeed * Time.smoothDeltaTime);

						}
						int start = UnityEngine.Random.Range (0,globe.cities.Count);
						int end = UnityEngine.Random.Range (0, globe.cities.Count);
						float duration = UnityEngine.Random.Range (2.0f,6.0f);
						float fade = UnityEngine.Random.Range (2.0f, 5.0f);
						if (globe.transform.GetComponentsInChildren<WPM.LineRenderer2>().Length < nonswitchobject.Trajectoriesnumber) {
							globe.AddLine (globe.cities[start].unitySphereLocation, globe.cities[end].unitySphereLocation, Color.white, UnityEngine.Random.Range (0, 0.2f), UnityEngine.Random.Range (2.0f, 6.0f), UnityEngine.Random.Range (0.0005f, 0.002f), UnityEngine.Random.Range (2f, 5f));
						}
						foreach (Progression.NonSwitchObjects.Variants variante in nonswitchobject.variants) {
							foreach (GameObject progressionvariant in variante.progressionvariants) {
								if(variante.variant == progressionvariation)
								{
									if(progressionvariant.activeSelf == false && progressionist.waittimea > 0)
									{
										progressionvariant.SetActive(true);
									}
								}
								else
								{
									progressionvariant.SetActive(false);
								}
							}
						}
						if (nonswitchable.activeSelf == false && progressionist.waittimea > 0 && transitioning == false) {
							if (nonswitchable.GetComponent<Button> () != null || nonswitchable.GetComponent<Slider> () != null) {

								//Debug.Log ("Here too");

								nonswitchable.GetComponent<Button> ().interactable = true;

								nonswitchable.GetComponent<Button> ().enabled = true;
							}
					//		Debug.Log ("Inhere");
							nonswitchable.SetActive (true);


						}
					}
				}
			}
		}
	//	if (waitformenuseconds == 0)
	//	{
		//	foreach (Progression progressionist in progression) {
				
		//	}
		}
		if (waitformenuseconds > 0)
		{
			waitformenuseconds--;

		}
//		if (offseterspeed.x > 0 || offseterspeed.y > 0) {
//			earthrender.material.SetTextureOffset ("_MainTex", new Vector2 (earthrender.material.GetTextureOffset ("_MainTex").x + offseter.x * Time.deltaTime, earthrender.material.GetTextureOffset ("_MainTex").y + offseter.y * Time.deltaTime));

//		}

//		foreach (Progression progressionist in progression) {
//			
//		}
	}
	public void FlyToCityy (string cityname) {
//		globe.showCities = true;
		globe.ToggleCityHighlight (globe.GetCityIndex (cityname), Color.red, true);
		globe.FlyToCity (cityname);
		globe.BlinkCity (globe.GetCityIndex(cityname), Color.red, Color.white, 5.0f, 10.0f);
	}
	public void FlytoLoc (float duration) {
		currentprogession.selectedmap.mapprefab.SetActive (true);
		kirtlandgo.transform.position = Conversion.GetSpherePointFromLatLon (currentprogession.selectedmap.latitude, currentprogession.selectedmap.longitude);
	//	kirtlandgo.SetActive (true);
		globe.FlyToLocation (latitude,longitude, duration);
	}
	public void savehome ()
	{
		savedhomee = globe.GetCurrentMapLocation ();
	}
	public void gohome ()
	{
		globe.FlyToLocation (savedhomee);
	}
	public void enableextendedzoom (bool enable)
	{
		extendedzoom = enable;
	}

	public void setgoalortho (float goalortho)
	{
		goalorthosize = goalortho;
	}

	public void setgoalswitchspeed (float speede)
	{
		goalswitchspeed = speede;
	}
	public void SetZoomAmount (float zoomamount) {
		//zoomamount = Mathf.Lerp(globe.zoomMinDistance, 1, zoomamount);
		// Gets the max distance from the map
		float fv = maincamera.fieldOfView;
		float radAngle = fv * Mathf.Deg2Rad;
		float sphereY = transform.localScale.y * 0.5f * Mathf.Sin (radAngle);
		float sphereX = transform.localScale.y * 0.5f * Mathf.Cos (radAngle);
		float frustumDistance = sphereY / Mathf.Tan (radAngle * 0.5f) + sphereX;

		Vector3 oldCamPos = maincamera.transform.position;
		Vector3 camPos = transform.position + (maincamera.transform.position - transform.position).normalized * frustumDistance * zoomamount;
		maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, camPos,Time.deltaTime);
//		Debug.Log (maincamera.transform.position + ":::" + camPos);
		maincamera.transform.LookAt (transform.position);
		float radiusSqr = transform.localScale.z * globe.zoomMinDistance * transform.localScale.z * globe.zoomMinDistance;
		if ((maincamera.transform.position - transform.position).sqrMagnitude < radiusSqr) {
			maincamera.transform.position = oldCamPos;
		}
	}
	public void setzoomlevel (float LEVEL)
	{
		zoomlevel = LEVEL;
	}
	public void startzoom (float speed)
	{
		StartCoroutine(zoom2(zoomlevel,speed));
	}
	public void startzoom3 (float speed)
	{
		StartCoroutine (zoom3 (zoomlevel, speed));
	}
	public void enableearthrotate()
	{
		foreach (Progression.NonSwitchObjects nonswitchable in currentprogession.nonswitchobjects) {
			if (nonswitchable.nonnumber == currentprogession.progcustomizationid)
			{
				nonswitchable.enableearthrotate = true;
				nonswitchable.sunrotate = true;
			}
		}
	}

	public IEnumerator zoom2 (float zoomer, float speed)
	{
		transitioning = true;
//		float timercheck = 0;
//		while (timercheck < 500 && Camera.main.transform.localPosition.z != zoomer) {
//			timercheck++;
//			Camera.main.transform.localPosition = Vector3.MoveTowards(Camera.main.transform.localPosition, new Vector3(0,0,Camera.main.transform.localPosition.z + zoomer), speed);
//			yield return null;
//		}
		zoomcame.transform.localPosition = currentprogession.selectedmap.initialcampos;
		Progression.NonSwitchObjects nonswitcher;
		nonswitcher = currentprogession.nonswitchobjects [0];
		foreach (Progression.NonSwitchObjects nonswitch in currentprogession.nonswitchobjects) {
			if (nonswitch.nonnumber == progressionnumber && inspace == true) {
				nonswitch.enableearthrotate = false;
				nonswitch.sunrotate = false;
				nonswitcher = nonswitch;
			}
		}
		//Initialize
		kirtlandgo.SetActive(true);
		float azoomlevel = zoomer;
	//	azoomlevel = Mathf.Clamp01 (azoomlevel);
	//	azoomlevel = Mathf.Lerp(0.495f, 1, azoomlevel);
		// Gets the max distance from the map
		float fv = maincamera.fieldOfView;
		float radAngle = fv * Mathf.Deg2Rad;
		float sphereY = transform.localScale.y * 0.5f * Mathf.Sin (radAngle);
		float sphereX = transform.localScale.y * 0.5f * Mathf.Cos (radAngle);
		float frustumDistance = sphereY / Mathf.Tan (radAngle * 0.5f) + sphereX;
		Vector3 oldCamPos = maincamera.transform.position;
		Vector3 camPos = transform.position + (maincamera.transform.position - transform.position).normalized * frustumDistance * azoomlevel;
		//azoomlevel = Vector3.Dot((camPos - transform.position), 1.0f/((Camera.main.transform.position - globe.transform.position).normalized * frustumDistance));
		//
		float timercheck = 0;
		float initzoom = globe.GetZoomLevel() - zoomoffset;
	//	Debug.Log (globe.GetZoomLevel()- zoomoffset + "::: zoomzoom" + initzoom);
		//float initzoom = azoomlevel;
		zoomcame.transform.rotation = Quaternion.Euler(rotationcaminitial);
		nonswitcher.enableearthrotate = false;
		while (initzoom != zoomer && timercheck < 2000) {
			nonswitcher.enableearthrotate = false;
		//	Debug.Log ("disablingearthrotate" + nonswitcher.nonswitchname);
			initzoom = Mathf.MoveTowards(initzoom,zoomer,speed);
			globe.SetZoomLevel (initzoom);
			Color alph = kirtlandgo.GetComponent<Renderer> ().material.color;
			if (kirtlandgo.GetComponent<Renderer> ().material.color.a < 255) {
				if (alph.a + alphspeed * speed <= 255) {
					alph.a += alphspeed * speed;
				} else {
					alph.a = 255;
				}
				kirtlandgo.GetComponent<Renderer> ().material.color = alph;
				//kirtlandgo.GetComponent<Renderer> ().material.color = new Color (alph.r, alph.g, alph.b,alph.a+alphspeed * speed);
	//			Debug.Log (alph.a + alphspeed * speed + "newspeed");
			} else{
				//kirtlandgo.GetComponent<Renderer> ().material.color = new Color (alph.r, alph.g, alph.b, 255.0f);
			}
			timercheck++;
			yield return null;
		}
		enableearthrotate ();
		inspace = false;
		Vector3 temppos;
		temppos = zoomcame.transform.localPosition;
		temppos.x = 0;
		temppos.z = 0;
		zoomcame.transform.localPosition = temppos;
		if (preextendedzoom == true) {
			float timerchecke = 0;
			bool isset = false;
			while (zoomcame.fieldOfView != preextendedzoomgoal && timerchecke < 2000) {
				timerchecke++;
				//scaling code begin
				if(isset == true){
					zoomcame.depth = 1;
					zoomcame.targetTexture = null;
					maincamera.enabled = false;
				}
				isset = true;

				zoomcame.fieldOfView = Mathf.MoveTowards (zoomcame.fieldOfView, preextendedzoomgoal, preextendedzoomspeed * Time.deltaTime);
				yield return null;
			}
		}
		if (extendedzoom == true) {
			Color alph = kirtlandgo.GetComponent<Renderer> ().material.color;
			//kirtlandgo.GetComponent<Renderer> ().material.color = new Color (alph.r, alph.g, alph.b, 255.0f);
			kirtlandgo.GetComponent<Renderer> ().material.mainTexture = null;
			kirtlandgo.transform.parent = maincamera.transform;
			kirtlandgo.transform.localPosition -= new Vector3 (kirtlandgo.transform.localPosition.x, kirtlandgo.transform.localPosition.y, 0);
			//kirtlandgo.transform.localRotation -= Quaternion.Euler(kirtlandgo.transform.rotation.x, kirtlandgo.transform.rotation.y + 90, kirtlandgo.transform.localPosition.z + 90);
			distance = Vector3.Distance (kirtlandgo.transform.position, maincamera.transform.position);
//			Debug.Log (distance);
			//scaling code begin

			distance -= (goDepth * .5f);
			v3ViewPort.Set (0, 0, distance);
			v3BottomLeft = maincamera.ViewportToWorldPoint (v3ViewPort);
			v3ViewPort.Set (1, 1, distance);
			v3TopRight = maincamera.ViewportToWorldPoint (v3ViewPort);
			kirtlandgo.transform.localScale = new Vector3 (v3BottomLeft.x - v3TopRight.x,.001f,  v3BottomLeft.y - v3TopRight.y);
			//scaling code end
			if (useortho == true) {
				float timerchecke = 0;
				bool isset = false;
				while (zoomcame.orthographicSize != goalorthosize && timerchecke < 2000) {
					timerchecke++;
					//scaling code begin
					if(isset == true){
						zoomcame.depth = 1;
						zoomcame.targetTexture = null;
						maincamera.enabled = false;
					}
					isset = true;

					distance -= (goDepth * .5f);
					v3ViewPort.Set (0, 0, distance);
					v3BottomLeft = maincamera.ViewportToWorldPoint (v3ViewPort);
					v3ViewPort.Set (1, 1, distance);
					v3TopRight = maincamera.ViewportToWorldPoint (v3ViewPort);
					kirtlandgo.transform.localScale = new Vector3 (v3BottomLeft.x - v3TopRight.x,.001f,  v3BottomLeft.y - v3TopRight.y);
					//scaling code end
					zoomcame.orthographicSize = Mathf.Lerp (zoomcame.orthographicSize, goalorthosize, goalswitchspeed);
					if (zoomcame.orthographicSize <= rotateorthovalue && zoomcame.orthographicSize-finishrotateorthovalue > 0) {
						zoomcame.transform.rotation = Quaternion.RotateTowards (zoomcame.transform.rotation,Rotatecameratarget.rotation,(goalswitchspeed*rotatemultiplier)/(zoomcame.orthographicSize-finishrotateorthovalue));
					}
					yield return null;
				}
			} else {
				float timerchecke = 0;
				bool isset = false;
			//	currentprogession.selectedmap.realsun.GetComponent<Light>().intensity = currentprogession.selectedmap.flickramount;
				while (zoomcame.transform.localPosition.y != goalorthosize && timerchecke < 2000 && zoomcame.transform.localPosition.y-goalorthosize > extendedzoomerrorcheck) {
					timerchecke++;
//					//scaling code begin
//
//					distance -= (goDepth * .5f);
//					v3ViewPort.Set (0, 0, distance);
//					v3BottomLeft = maincamera.ViewportToWorldPoint (v3ViewPort);
//					v3ViewPort.Set (1, 1, distance);
//					v3TopRight = maincamera.ViewportToWorldPoint (v3ViewPort);
//					kirtlandgo.transform.localScale = new Vector3 (v3BottomLeft.x - v3TopRight.x, v3BottomLeft.y - v3TopRight.y, goDepth);
//					//scaling code end
					//camera switch begin
					if(isset == true){
						zoomcame.depth = 1;
						zoomcame.targetTexture = null;
						maincamera.enabled = false;
					}
					isset = true;

					//camera switch end
					Vector3 campos = zoomcame.transform.localPosition;
					campos.y = Mathf.Lerp (zoomcame.transform.localPosition.y, goalorthosize, goalswitchspeed);
					zoomcame.transform.localPosition = campos;
					if (zoomcame.transform.localPosition.y <= rotateorthovalue && zoomcame.transform.localPosition.y-finishrotateorthovalue > 0) {
						zoomcame.transform.rotation = Quaternion.RotateTowards (zoomcame.transform.rotation,Rotatecameratarget.rotation,(goalswitchspeed*rotatemultiplier)/(zoomcame.transform.localPosition.y-finishrotateorthovalue));
					}

					yield return null;
				}
//				foreach (Progression.NonSwitchObjects nonswitch in currentprogession.nonswitchobjects) {
//					if (nonswitch.nonnumber == progressionnumber) {
//						nonswitch.enableearthrotate = true;
//					}
//				}
			}
		}
		zoomcame.transform.parent = Rotatecameratarget;
		transitioning = false;
//		if (reorientcamera = true) {
//			float timercheckee = 0;
//			MatrixBlender mb = zoomcame.GetComponent<MatrixBlender> ();
//			while (mb. != goalorthosize && timercheckee < 2000) {
//				timercheckee++;
//				zoomcame.orthographicSize = Mathf.Lerp (zoomcame.orthographicSize, goalorthosize, goalswitchspeed);
//				yield return null;
//			}
//		}
	}
	public IEnumerator zoom3 (float zoomer, float speed) {
		transitioning = true;
		zoomcame.transform.parent = zoomcame.transform.parent.parent;
		kirtlandgo.GetComponent<Renderer> ().material.mainTexture = currentprogession.selectedmap.alphaoutline;
		Progression.NonSwitchObjects nonswitcher;
		nonswitcher = currentprogession.nonswitchobjects [0];
		foreach (Progression.NonSwitchObjects nonswitch in currentprogession.nonswitchobjects) {
			if (nonswitch.nonnumber == progressionnumber && inspace == false) {
				nonswitch.enableearthrotate = false;
				nonswitch.sunrotate = false;
				nonswitcher = nonswitch;
			}
		}
		if (extendedzoom == true) {
			if (useortho == true) {
				float timerchecke = 0;
				bool isset = false;

				while (zoomcame.orthographicSize != goalorthosize && timerchecke < 2000) {
					timerchecke++;
					//scaling code begin
			//		if(isset == true){
			//			zoomcame.depth = 1;
			//			zoomcame.targetTexture = null;
			//			maincamera.enabled = false;
			//		}
			//		isset = true;
			//		Debug.Log ("Here in 921");
					distance -= (goDepth * .5f);
					v3ViewPort.Set (0, 0, distance);
					v3BottomLeft = maincamera.ViewportToWorldPoint (v3ViewPort);
					v3ViewPort.Set (1, 1, distance);
					v3TopRight = maincamera.ViewportToWorldPoint (v3ViewPort);
					kirtlandgo.transform.localScale = new Vector3 (v3BottomLeft.x - v3TopRight.x,.001f,  v3BottomLeft.y - v3TopRight.y);
					//scaling code end
					zoomcame.orthographicSize = Mathf.InverseLerp (zoomcame.orthographicSize, goalorthosize, goalswitchspeed);
					if (zoomcame.transform.rotation.eulerAngles != new Vector3 (90,0,0)) {
						zoomcame.transform.rotation = Quaternion.RotateTowards (zoomcame.transform.rotation,Quaternion.Euler(90,0,0),(goalswitchspeed*rotatemultiplier)/(zoomcame.orthographicSize-finishrotateorthovalue));
					}
					yield return null;
					currentprogession.selectedmap.mapprefab.SetActive (false);
				}
			}
			else {
				kirtlandgo.transform.parent = globe.transform.Find("Markers");
				float timerchecke = 0;
				bool isset = false;
				//	currentprogession.selectedmap.realsun.GetComponent<Light>().intensity = currentprogession.selectedmap.flickramount;
				while (zoomcame.transform.localPosition.y != goalorthosize && timerchecke < 2000 && Mathf.Abs(zoomcame.transform.localPosition.y -goalorthosize) > extendedzoomerrorcheck) {//&& zoomcame.transform.localPosition.y - goalorthosize > extendedzoomerrorcheck) {
			//		Debug.Log ("Here in 941");
					timerchecke++;
					if (zoomcame.transform.rotation.eulerAngles != new Vector3 (90,0,0)) {
						zoomcame.transform.rotation = Quaternion.RotateTowards (zoomcame.transform.rotation, Quaternion.Euler(90,0,0), (goalswitchspeed * rotatemultiplier*6));// / (zoomcame.transform.localPosition.y - finishrotateorthovalue));
					}
					//					//scaling code begin
					//
					//					distance -= (goDepth * .5f);
					//					v3ViewPort.Set (0, 0, distance);
					//					v3BottomLeft = maincamera.ViewportToWorldPoint (v3ViewPort);
					//					v3ViewPort.Set (1, 1, distance);
					//					v3TopRight = maincamera.ViewportToWorldPoint (v3ViewPort);
					//					kirtlandgo.transform.localScale = new Vector3 (v3BottomLeft.x - v3TopRight.x, v3BottomLeft.y - v3TopRight.y, goDepth);
					//					//scaling code end
					//camera switch begin
				//	if (isset == true) {
				//		zoomcame.depth = 1;
				//		zoomcame.targetTexture = null;
				//		maincamera.enabled = false;
				//	}
				//	isset = true;

					//camera switch end
					Vector3 campos = zoomcame.transform.localPosition;
					campos.y = Mathf.MoveTowards (zoomcame.transform.localPosition.y, goalorthosize, 2*goalswitchspeed*(4+85/(Mathf.Pow(Mathf.Abs(zoomcame.transform.localPosition.y-goalorthosize),0.0005f))));
					zoomcame.transform.localPosition = campos;
					yield return null;
				}
				//	if (isset == true) {
				//		zoomcame.depth = 0;
				//		zoomcame.targetTexture = targtexture;
				//		maincamera.enabled = true;
				//	}
				//	isset = true;
			}
		}
		if (preextendedzoom == true) {
			float timerchecke = 0;
			bool isset = false;
			while (zoomcame.fieldOfView != prepreextendedzoomgoal && timerchecke < 2000) {
				timerchecke++;
			//   scaling code begin
			//	if(isset == true){
			//		zoomcame.depth = 1;
			//		zoomcame.targetTexture = null;
			//		maincamera.enabled = false;
			//	}
				isset = true;
	//			Debug.Log ("Here in 989");
				zoomcame.fieldOfView = Mathf.MoveTowards (zoomcame.fieldOfView, prepreextendedzoomgoal, preextendedzoomspeed * Time.deltaTime);
				kirtlandgo.SetActive (false);
				yield return null;
			}
			kirtlandgo.SetActive (false);
		}
		zoomcame.depth = 0;
		zoomcame.targetTexture = targtexture;
		maincamera.enabled = true;
	//	kirtlandgo.SetActive(true);
		globe.FlyToLocation (latitude, longitude, 0.01f);
//		Debug.Log ("Here");
		float azoomlevel = zoomer;
		//	azoomlevel = Mathf.Clamp01 (azoomlevel);
		//	azoomlevel = Mathf.Lerp(0.495f, 1, azoomlevel);
		// Gets the max distance from the map
		float fv = maincamera.fieldOfView;
		float radAngle = fv * Mathf.Deg2Rad;
		float sphereY = transform.localScale.y * 0.5f * Mathf.Sin (radAngle);
		float sphereX = transform.localScale.y * 0.5f * Mathf.Cos (radAngle);
		float frustumDistance = sphereY / Mathf.Tan (radAngle * 0.5f) + sphereX;
		Vector3 oldCamPos = maincamera.transform.position;
		Vector3 camPos = transform.position + (maincamera.transform.position - transform.position).normalized * frustumDistance * azoomlevel;
		//azoomlevel = Vector3.Dot((camPos - transform.position), 1.0f/((Camera.main.transform.position - globe.transform.position).normalized * frustumDistance));
		//
		float timercheck = 0;
		float initzoom = globe.GetZoomLevel() - zoomoffset;
		//	Debug.Log (globe.GetZoomLevel()- zoomoffset + "::: zoomzoom" + initzoom);
		//float initzoom = azoomlevel;
		//zoomcame.transform.rotation = Quaternion.Euler(rotationcaminitial);

		kirtlandgo.transform.localPosition = currentprogession.selectedmap.locpos;

		kirtlandgo.transform.localScale = currentprogession.selectedmap.locscale;

		kirtlandgo.transform.localRotation = Quaternion.Euler(currentprogession.selectedmap.locrot);

		while (initzoom != zoomer && timercheck < 2000) {
		//	Debug.Log ("Here in 1016");
		//	enableearthrotate();
	//		Debug.Log (currentprogession.number+ "enabled" + " " + nonswitcher.enableearthrotate);
		//	nonswitcher.enableearthrotate = true;
			nonswitcher.sunrotate = true;
			initzoom = Mathf.MoveTowards(initzoom,zoomer,speed);
			globe.SetZoomLevel (initzoom);
			Color alph = kirtlandgo.GetComponent<Renderer> ().material.color;
			if (kirtlandgo.GetComponent<Renderer> ().material.color.a < 255) {
				if (alph.a - alphspeed * speed * 4 >= 0) {
					alph.a -= alphspeed * speed * 4;
				} else {
					alph.a = 0;
				}
				kirtlandgo.GetComponent<Renderer> ().material.color = alph;
				//kirtlandgo.GetComponent<Renderer> ().material.color = new Color (alph.r, alph.g, alph.b,alph.a+alphspeed * speed);
				//			Debug.Log (alph.a + alphspeed * speed + "newspeed");
			} else{
				//kirtlandgo.GetComponent<Renderer> ().material.color = new Color (alph.r, alph.g, alph.b, 255.0f);
			}
			timercheck++;
//			Debug.Log ("line 1046");
			currentprogession.selectedmap.mapprefab.SetActive (false);
			yield return null;
		}
		currentprogession.selectedmap.mapprefab.SetActive (false);
		kirtlandgo.SetActive (false);
	//	enableearthrotate ();
		zoomcame.transform.localPosition = currentprogession.selectedmap.initialcampos;
		inspace = true;
		progression[0].selectedmap.skybox.SetFloat ("_Exposure", 0.0f);
		transitioning = false;
	}
	public void SetProgressionNumbertoFloat (float change) {
		progressionnumber = change;
	}
	public void setrotationspeed (float speed) {
		rotatemultiplier = speed;
	}
	public void seterror (float error)
	{
		extendedzoomerrorcheck = error;
	}
	public void setmap(int numb)
	{
		if (currentprogession.maps.Length >= numb) {
			currentprogession.selectedmap = currentprogession.maps [numb - 1];
			currentprogession.selectedmap.mapprefab.SetActive (true);
			foreach (Progression.Maps map in currentprogession.maps) {
				if (map == currentprogession.selectedmap) {
					map.mapprefab.SetActive (true);
				} else {
					map.mapprefab.SetActive (false);
				}
			}
		}
	}
	public void ActivationSound () {
		foreach (Progression progressionist in progression)
		{
			if (gameObject.GetComponent<AudioSource>() != null)
			{
				AudioSource audiosource2 = gameObject.GetComponents<AudioSource>()[1];
				audiosource2.clip = progressionist.playonhighlight;
				audiosource2.Play ();
			}
		}
	}
	
	public void DeactivationSound () {
		foreach (Progression progressionist in progression)
		{
			if (gameObject.GetComponent<AudioSource>() != null)
			{
				AudioSource audiosource2 = gameObject.GetComponents<AudioSource>()[1];
				audiosource2.clip = progressionist.playondehighlight;
				audiosource2.Play ();
			}
		}
	}
	public void CreateMarker (float scale) {
		GameObject markerd = Instantiate (Resources.Load<GameObject> (pathname));
		globe.AddMarker (markerd, Conversion.GetSpherePointFromLatLon(latitude,longitude), scale);
	}
	IEnumerator waitsetseconds(int waittime)
		{
		yield return new WaitForSeconds (waittime);
	}

	IEnumerator waitfortime() {
		yield return new WaitForSeconds(waitfortimeseconds);

		foreach (GameObject dashingline in dashinglines)
		{
			if(dashingline.GetComponent<Text>().text == dashingline.name)
			{
				dashingline.GetComponent<Text>().text = "I" + dashingline.GetComponent<Text>().text;
			}
			else
			{
				dashingline.GetComponent<Text>().text = dashingline.name;
			}
			
		}
		foreach (Progression progressionist in progression) {
			foreach (Progression.NonSwitchObjects nonswitchobject in progressionist.nonswitchobjects) {
				foreach (GameObject nonswitchable in nonswitchobject.nonswitchables) {
					if ((nonswitchobject.nonnumber == progressionnumber && nonswitchable.tag == "highlighted") || nonswitchable == gameObject.GetComponent<EventSystem>().currentSelectedGameObject) {
						if(nonswitchable.GetComponentInChildren<Text>() != null) {
							if(nonswitchable.GetComponentInChildren<Text>().text == nonswitchable.GetComponentInChildren<Text>().gameObject.name)
							{
								nonswitchable.GetComponentInChildren<Text>().text = dashingstring + nonswitchable.GetComponentInChildren<Text>().text;
							}
							else
							{
								nonswitchable.GetComponentInChildren<Text>().text = nonswitchable.GetComponentInChildren<Text>().gameObject.name;
							}
						}
					}
					if(nonswitchable.tag == "not highlighted")
					{
						if(nonswitchable.GetComponentInChildren<Text>() != null) {
							nonswitchable.GetComponentInChildren<Text>().text = nonswitchable.GetComponentInChildren<Text>().gameObject.name;
						}
					}
				}
			}
		}

		StartCoroutine (waitfortime());
	}
}
