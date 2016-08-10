//This makes a gameobject always face the camera, and always appear as the same size, no matter how close or far away from the camera it gets.
//It is useful if we want to have GUI elements in world space.

using UnityEngine;
using System.Collections;

public class WorldSpaceUI : MonoBehaviour
{
	[Header("References")]
	[SerializeField] Transform camera;

	[Header("Settings")]
	[SerializeField] float size;
	[SerializeField] bool keepPixelSize; //makes sure object does not change size if camera gets further or nearer to it

	float distanceFromCamera;
	float percievedSize;
	float axisScale;


	void
	Start()
	{
		if (gameObject.activeInHierarchy)
		{
			if (camera == null && GameObject.Find ("Main Camera") != null)
				camera = GameObject.Find ("Main Camera").transform;

			distanceFromCamera = CalculateLocalDistanceTo (camera, transform, "z");
			percievedSize = distanceFromCamera / size;
		}
	}
		

	void Update ()
	{
		transform.LookAt (transform.position + camera.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);

		if (keepPixelSize)
		{
			axisScale = CalculateLocalDistanceTo (camera, transform, "z") / percievedSize;
			transform.localScale = new Vector3 (axisScale, axisScale, axisScale);
		}
	}


	//Calculate distance between trans1 and trans2 on a single axis local to trans1
	public static float CalculateLocalDistanceTo(Transform trans1, Transform trans2, string axis)
	{
		Vector3 distance = trans1.InverseTransformPoint (trans2.position);

		if (axis == "x")
			return distance.x;
		
		else if (axis == "y")
			return distance.y;
		
		else if (axis == "z")
			return distance.z;
		
		else
			return 0.0f;
	}
}