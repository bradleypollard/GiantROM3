using UnityEngine;
using System.Collections;

public enum Gender{Male, Female}

public class DressingRoomDoor : MonoBehaviour
{
	[Header("REFERENCES")]
	[SerializeField] Material maleMaterial;
	[SerializeField] Material femaleMaterial;
	[SerializeField] MeshRenderer door;

	[Header("SETTINGS")]
	[SerializeField] Gender gender;
	[SerializeField] float animationDuration;

	void
	OnValidate()
	{
		if (gameObject.activeInHierarchy)
		{
			if (gender == Gender.Male)
				door.material = maleMaterial;
			else
				door.material = femaleMaterial;
		}
	}

	public IEnumerator
	DoorOpen(bool doorState)
	{
		float yRotation = 0;
		float lerpTime = 0f;
		float newRotation;
		if (doorState == true)
			newRotation = 90;
		else
			newRotation = 0;

		while (lerpTime <= 1f)
		{
			yRotation = Mathf.Lerp(door.transform.rotation.eulerAngles.y, newRotation, lerpTime);
			door.transform.rotation = Quaternion.Euler(door.transform.rotation.eulerAngles.x, yRotation, door.transform.rotation.eulerAngles.z);

			animationDuration = Mathf.Clamp(animationDuration, 0.3f, 4f);
			lerpTime += Time.deltaTime / animationDuration;

			yield return null;
		}
	}

	void
	OnGUI()
	{
		if (GUI.Button (new Rect (2, 2, 200, 20), "Set door to open"))
		{
			StartCoroutine (DoorOpen (true));
		}
		if (GUI.Button (new Rect (2, 22, 200, 20), "Set door to closed"))
		{
			StartCoroutine (DoorOpen (false));
		}
	}
}