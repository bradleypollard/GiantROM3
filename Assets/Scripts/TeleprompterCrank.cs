using UnityEngine;
using System.Collections;

public class TeleprompterCrank : MonoBehaviour
{
	[SerializeField] Transform crankHandle;
	[SerializeField] float animationDuration = 0.5f; //in seconds
	bool isCranking;


	void
	RotateHandle()
	{
		if (!isCranking)
		{
			isCranking = true;
			StartCoroutine (LerpHandle ());
			GetComponent<AudioSource> ().Play ();
		}
	}


	IEnumerator
	LerpHandle()
	{
		float zRotation = 0;
		float lerpTime = 0f;

		while (lerpTime <= 1f)
		{
			zRotation = Mathf.Lerp (0, 360, lerpTime);
			crankHandle.rotation = Quaternion.Euler (0, 0, zRotation);

			animationDuration = Mathf.Clamp (animationDuration, 0.5f, 4f);
			lerpTime += Time.deltaTime / animationDuration;

			yield return null;
		}

		isCranking = false;
	}

	void
	OnGUI()
	{
		if (GUI.Button (new Rect (2, 22, 300, 20), "Test Teleprompter Crank Animation"))
		{
			RotateHandle ();
		}
	}
}