using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingBayDoor : MonoBehaviour
{
	[Header ("REFERENCES")]
	[SerializeField] Transform door;
	[SerializeField] AudioClip open;

  [Header ("SETTINGS")]
	[SerializeField] float animationDuration;

	//public bool vanPresent;
	bool doorClosing;

	int currentTicket = 0;

	public IEnumerator
	DoorOpen (bool doorState)
	{
		//if (vanPresent)
		//{
			currentTicket++;
			int myTicket = currentTicket;

			animationDuration = Mathf.Clamp (animationDuration, 0.3f, 4f);
			float endY = 0;
			float lerpTime = 0f;

			if (doorState == true) {
				endY = 2.5f;
			} else {
				endY = 0f;
			}
			while (lerpTime <= 1f) {
				door.localPosition = Vector3.Lerp (door.transform.localPosition, new Vector3 (door.transform.localPosition.x, endY, door.transform.localPosition.z), lerpTime);

				lerpTime += Time.deltaTime / animationDuration;

				if (myTicket != currentTicket) {
					break;
				}

				yield return null;
			}
		//}
	}


	void
	OnTriggerEnter (Collider other)
	{
		if (other.name == "Truck") {
			StartCoroutine (DoorOpen (true));
      GetComponent<AudioSource>().clip = open;
      GetComponent<AudioSource>().Play();
    }
	}

	void
	OnTriggerExit (Collider other)
	{
		if (other.name == "Truck") {
			StartCoroutine (DoorOpen (false));
    }
	}
}