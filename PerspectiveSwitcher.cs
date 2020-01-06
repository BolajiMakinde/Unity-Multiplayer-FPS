using UnityEngine;
using System.Collections;

[RequireComponent (typeof(MatrixBlender))]
public class PerspectiveSwitcher : MonoBehaviour
{
	private Matrix4x4   ortho,
	perspective;
	public float        fov     = 60f,
	near    = .3f,
	far     = 1000f,
	orthographicSize = 50f;
	private float       aspect;
	private MatrixBlender blender;
	private bool        orthoOn;
	public Camera cameraobject;
	public bool turnorthoon;
	public bool turnorthooff;
	public float duration = 1f;

	void Start()
	{
		aspect = (float) Screen.width / (float) Screen.height;
		ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
		perspective = Matrix4x4.Perspective(fov, aspect, near, far);
	//	cameraobject.projectionMatrix = ortho;
	//	orthoOn = true;
		blender = (MatrixBlender) GetComponent(typeof(MatrixBlender));
	}

	void Update ()
	{
		if (turnorthoon) {
			Orthoon (duration);
			turnorthoon = false;
		}
		if (turnorthooff) {
			Orthooff (duration);
			turnorthooff = false;
		}
	}

	void Orthoon(float duration)
	{
		orthoOn = true;
		blender.BlendToMatrix(ortho, duration);
	}
	void Orthooff(float duration)
	{
		orthoOn = false;
		blender.BlendToMatrix(perspective, duration);
	}
}