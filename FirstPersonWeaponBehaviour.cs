using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using RootMotion.FinalIK;
using UnityEditor;

public class FirstPersonWeaponBehaviour : MonoBehaviour {

	public enum WeaponType{
		SMG,
		AR,
		SHOTGUN,
		LMG,
		SNIPE,
		LAUNCH,
		MELEE,
		OTHER
	}

	[Tooltip("Choose one of the seven weapon types, this will be the base functionality of the weapon.")]
	public WeaponType myweapontype;

	[Tooltip("The Weapon GameObject")]
	public GameObject WeaponGO;

	[Tooltip("The Ammunition capacity of the weapon before needing to reload")]
	[Range(0, 2000)] public int MaxAmmoCount;

	[Tooltip("The amount of ammunition the weapon currently has. Set this to the max ammo count")]
	[Range(0, 2000)] public int CurrAmmoCount;

	[Tooltip("The maximum amount of reloads the weapon has")]
	[Range(0, 2000)] public static int MaxReloadCount;

	[Tooltip("The amount of reloads the weapon currently has. Set this to the max ammo count for them to have full reloads in start and lower than the max for them to start off without the full amounts of reload")]
	[Range(0, 2000)] public int CurrReloadCount;

	[Tooltip("The Fire Rate of the weapon in RPM with respect to Time. If 0 the weapon will never fire a round.")]
	public AnimationCurve FireRate;

	[Tooltip("The Damage of the weapon with respect to distance. If 0 the weapon will deal no damage. Note this should be somewhere in synch with FireRate")]
	public AnimationCurve DamageRate;

    [Tooltip("Check to have the weapon dual wield. If checked the WeaponGO Gameobject variable needs to have the two weapons as DIRECT children of the script")]
    public bool DualWieldable;

    [Tooltip("The AnimatorController component that is attached to the weapon. Root Motion must be deselected on the Animator Controller Component")]
	public Animator AnimController;
	
	[Tooltip("Check this if you have a preset idling animation to use. Leave unchecked if you would like to use a scripted idling animation")]
	public bool UseIdleAnimatation;

	public enum StartingAction
	{
		Draw,
		Idle,
		Reload
	}
	[Tooltip("Select whether the gun will draw, idle, or reload on start")]
	public StartingAction weaponStartingAction;

	[Tooltip("Check this if you have a preset drawing animation to use. Leave unchecked if you would like to use a scripted drawing animation")]
	public bool UseDrawAnimatation;

    [Tooltip("Check this if you have a preset aiming animation to use. Leave unchecked if you would like to use a scripted aiming animation")]
    public bool UseAimAnimatation;

    [Tooltip("Check this if you have a preset meleeing animation to use. Leave unchecked if you would like to use a scripted aiming meleeing")]
    public bool UseMeleeAnimatation;

    [Tooltip("The name of the Idle Parameter in the Animator Controller. ONLY NEED IF UseIdleAnimation is true. If you check this make sure you have a float parameter in the Animator controller and set its name equal to this variable name or vice versa. A value of one will idle and a value of zero is not idling")]
	public string IdleName;

	[Tooltip("The name of the Fire Parameter in the Animator Controller. If you check this make sure you have a float parameter in the Animator controller and set its name equal to this variable name or vice versa. A value of one will firing and a value of zero is not firing")]
	public string FireName;

	[Tooltip("The name of the Reload Parameter in the Animator Controller. If you check this make sure you have a float parameter in the Animator controller and set its name equal to this variable name or vice versa. A value of one will reloading and a value of zero is not reloading")]
	public string ReloadName;

	[Tooltip("The name of the Drawing Parameter in the Animator Controller. ONLY NEED IF UseDrawAnimation is true. If you check this make sure you have a float parameter in the Animator controller and set its name equal to this variable name or vice versa. A value of one will draw and a value of zero is not drawing")]
	public string DrawName;

    [Tooltip("The name of the Aiming Parameter in the Animator Controller. ONLY NEED IF UseAimAnimation is true. If you check this make sure you have a float parameter in the Animator controller and set its name equal to this variable name or vice versa. A value of one will aim and a value of zero is not aiming")]
    public string AimName;

    [Tooltip("The name of the Meleeing Parameter in the Animator Controller. ONLY NEED IF UseMeleeingAnimation is true. If you check this make sure you have a float parameter in the Animator controller and set its name equal to this variable name or vice versa. A value of one will melee and a value of zero is not meleeing")]
    public string MeleeName;

    // Use this for initialization
    void Start () {
		if (weaponStartingAction == StartingAction.Draw) {
			AnimController.SetFloat (DrawName, 1f);
			AnimController.SetFloat (IdleName, 0f);
			AnimController.SetFloat (ReloadName, 0f);
		}
		else if (weaponStartingAction == StartingAction.Idle) {
			AnimController.SetFloat (IdleName, 1f);
			AnimController.SetFloat (DrawName, 0f);
			AnimController.SetFloat (ReloadName, 0f);
		}
		else {
			AnimController.SetFloat (ReloadName, 1f);
			AnimController.SetFloat (DrawName, 0f);
			AnimController.SetFloat (IdleName, 0f);
		}
	}

	void FixedUpdate () {
        if (CrossPlatformInputManager.GetButton("Fire1"))
        {
            AnimController.SetFloat(FireName, 1f);
            AnimController.SetFloat(DrawName, 0f);
            AnimController.SetFloat(IdleName, 0f);
            AnimController.SetFloat(ReloadName, 0f);
            AnimController.SetFloat(AimName, 0f);
            AnimController.SetFloat(MeleeName, 0f);
        }
        else if (CrossPlatformInputManager.GetButton("Reload"))
        {
            AnimController.SetFloat(FireName, 0f);
            AnimController.SetFloat(DrawName, 0f);
            AnimController.SetFloat(IdleName, 0f);
            AnimController.SetFloat(ReloadName, 1f);
            AnimController.SetFloat(AimName, 0f);
            AnimController.SetFloat(MeleeName, 0f);
        }
        else if (CrossPlatformInputManager.GetButton("Fire2"))
        {
            AnimController.SetFloat(FireName, 0f);
            AnimController.SetFloat(DrawName, 0f);
            AnimController.SetFloat(IdleName, 0f);
            AnimController.SetFloat(ReloadName, 0f);
            AnimController.SetFloat(AimName, 1f);
            AnimController.SetFloat(MeleeName, 0f);
        }
        else if (CrossPlatformInputManager.GetButton("Melee"))
        {
            AnimController.SetFloat(FireName, 0f);
            AnimController.SetFloat(DrawName, 0f);
            AnimController.SetFloat(IdleName, 0f);
            AnimController.SetFloat(ReloadName, 0f);
            AnimController.SetFloat(AimName, 0f);
            AnimController.SetFloat(MeleeName, 1f);
        }
        else
        {
            AnimController.SetFloat(FireName, 0f);
            AnimController.SetFloat(DrawName, 0f);
            AnimController.SetFloat(IdleName, 1f);
            AnimController.SetFloat(ReloadName, 0f);
            AnimController.SetFloat(AimName, 0f);
            AnimController.SetFloat(MeleeName, 0f);
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (UseIdleAnimatation == true) {
			if (AnimController.GetFloat (FireName) == 0 && AnimController.GetFloat (ReloadName) == 0 && AnimController.GetFloat (DrawName) == 0) {
				AnimController.SetFloat (IdleName, 1f);
			}
		}
        if (CurrAmmoCount <= 0)
        {

        }
	}
}
