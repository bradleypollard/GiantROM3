using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadingBayDoor : MonoBehaviour
{
	[Header("REFERENCES")]
	[SerializeField] Transform door;

	[Header("SETTINGS")]
	[SerializeField] float animationDuration;

	public bool vanPresent;
	bool doorClosing;
	List<Collider> players = new List<Collider> ();

	int currentTicket = 0;

	public IEnumerator
	DoorOpen(bool doorState)
	{
		if (vanPresent)
		{
			currentTicket++;
			int myTicket = currentTicket;

			animationDuration = Mathf.Clamp (animationDuration, 0.3f, 4f);
			float endY = 0;
			float lerpTime = 0f;

			if (doorState == true)
			{
				endY = 2.5f;
			}
			else
			{
				endY = 0f;
			}
			while (lerpTime <= 1f)
			{
				door.localPosition = Vector3.Lerp (door.transform.localPosition, new Vector3(door.transform.localPosition.x, endY, door.transform.localPosition.z), lerpTime);

				lerpTime += Time.deltaTime / animationDuration;

				if (myTicket != currentTicket)
				{
					break;
				}

				yield return null;
			}
		}
	}


	void
	OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<PlayerMovement> ())
		{
			players.Add (other);
			StartCoroutine (DoorOpen (true));
		}
	}


	void
	OnTriggerExit(Collider other)
	{
		if(other.GetComponent<PlayerMovement>())
		{
			players.Remove (other);

			if(players.Count == 0)
			{
				StartCoroutine(DoorOpen(false));
			}
		}
	}
}